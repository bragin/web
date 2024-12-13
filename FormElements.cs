using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using HtmlParserSharp;
using HtmlParserSharp.Common;
using HtmlParserSharp.Core;

namespace web
{
    public partial class FormElements : Form
    {
        private TreeView treeView;
        private ListView listView;
        private int TreeViewIndentLevel = 10;

        public FormElements()
        {
            InitializeComponent();

            // Initialize the TreeView
            treeView = new TreeView
            {
                Dock = DockStyle.Fill,
                Indent = 20,
                DrawMode = TreeViewDrawMode.OwnerDrawText,
                ShowPlusMinus = false,
                ShowLines = false,
                FullRowSelect = true,
                BackColor = BackColor,
                ForeColor = ForeColor
            };

            var db = treeView.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            db.SetValue(treeView, true, null);

            treeView.DrawNode += TreeView_DrawNode;
            treeView.NodeMouseClick += TreeView_NodeMouseClick;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.AfterCollapse += TreeView_NodeAfterCollapse;
            treeView.AfterExpand += TreeView_NodeAfterExpand;

            // Initialize the ListView
            listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                BackColor = BackColor,
                ForeColor = ForeColor,
                BorderStyle = BorderStyle.None
            };

            listView.Columns.Add("Attribute", 200, HorizontalAlignment.Left);
            listView.Columns.Add("Value", 400, HorizontalAlignment.Left);

            splitContainerElements.Panel1.Controls.Add(treeView);
            splitContainerElements.Panel2.Controls.Add(listView);
        }

        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            Rectangle fullRowBounds = new Rectangle(0, e.Bounds.Top, treeView.Width, e.Bounds.Height);

            Rectangle nodeBounds = e.Bounds;
            nodeBounds.Offset(TreeViewIndentLevel, 0); // Indent all contents to the right

            if (e.Node.IsSelected)
            {
                e.Graphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(60, 60, 60)), fullRowBounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, e.Node.NodeFont ?? treeView.Font, nodeBounds, treeView.ForeColor, TextFormatFlags.GlyphOverhangPadding);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(treeView.BackColor), fullRowBounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, e.Node.NodeFont ?? treeView.Font, nodeBounds, treeView.ForeColor, TextFormatFlags.GlyphOverhangPadding);
            }

            int width = 8;
            int height = 12;
            int padding = 4;

            if (e.Node.Nodes.Count > 0)
            {
                Point trianglePoint = new Point(nodeBounds.Left - width - padding, nodeBounds.Top + nodeBounds.Height / 2);

                using (Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(199, 199, 199)))
                using (Graphics g = e.Graphics)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    Point[] triangle;
                    if (e.Node.IsExpanded)
                    {
                        triangle = new Point[]
                        {
                            new Point(trianglePoint.X, trianglePoint.Y - width / 2),
                            new Point(trianglePoint.X + height, trianglePoint.Y - width / 2),
                            new Point(trianglePoint.X + height / 2, trianglePoint.Y + width / 2)
                        };
                    }
                    else
                    {
                        triangle = new Point[]
                        {
                            new Point(trianglePoint.X, trianglePoint.Y - height / 2),
                            new Point(trianglePoint.X + width, trianglePoint.Y),
                            new Point(trianglePoint.X, trianglePoint.Y + height / 2)
                        };
                    }
                    g.FillPolygon(brush, triangle);
                    g.SmoothingMode = SmoothingMode.None;


                    // DEBUG
                    /*{
                        Rectangle triangleBounds = new Rectangle(nodeBounds.Left-12 - 4, nodeBounds.Top, 18, nodeBounds.Height-1);
                        var pen = new Pen(System.Drawing.Color.Red, 1);
                        g.DrawRectangle(pen, triangleBounds);
                    }*/

                }
            }
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Rectangle nodeBounds = e.Node.Bounds;
            nodeBounds.Offset(TreeViewIndentLevel, 0); // Indent all contents to the right

            Rectangle triangleBounds = new Rectangle(nodeBounds.Left - 12 - 4, nodeBounds.Top, 18, nodeBounds.Height - 1);

            // Check if click is in the triangle area
            if (triangleBounds.Contains(e.Location))
            {
                if (e.Node.IsExpanded)
                {
                    e.Node.Collapse();
                }
                else
                {
                    e.Node.Expand();
                }
            }
        }
        private void TreeView_NodeAfterCollapse(object sender, TreeViewEventArgs e)
        {
            UpdateNodeText(e.Node, collapsed: true);
            //RemoveClosingTagNode(e.Node);
            // Defer removal of the closing tag node, otherwise it'd fail later
            this.BeginInvoke((Action)(() =>
            {
                RemoveClosingTagNode(e.Node);
            }));
        }

        private void TreeView_NodeAfterExpand(object sender, TreeViewEventArgs e)
        {
            UpdateNodeText(e.Node, collapsed: false);
            InsertClosingTagNode(e.Node);
        }

        private void UpdateNodeText(TreeNode node, bool collapsed)
        {
            XmlNode htmlNode = node.Tag as XmlNode;
            if (htmlNode != null)
            {
                node.Text = GetNodeText(htmlNode, collapsed);
            }
        }

        private string GetNodeText(XmlNode htmlNode, bool collapsed)
        {
            if (htmlNode.NodeType == XmlNodeType.Element)
            {
                // No children
                if (htmlNode.ChildNodes.Count == 0)
                {
                    return $"<{htmlNode.Name}></{htmlNode.Name}>";
                }

                // Contains children
                if (collapsed)
                    return $"<{htmlNode.Name}>...</{htmlNode.Name}>";
                else
                    return $"<{htmlNode.Name}>";
            }
            else if (htmlNode.NodeType == XmlNodeType.Text)
            {
                return htmlNode.InnerText;
            }
            else
            {
                return htmlNode.Name;
            }
        }
        private void InsertClosingTagNode(TreeNode node)
        {
            XmlNode htmlNode = node.Tag as XmlNode;
            if (htmlNode != null &&
                htmlNode.NodeType == XmlNodeType.Element && // closing tag only for elements
                htmlNode.ChildNodes.Count > 0 // only for those containing children
                )
            {
                TreeNode closingTagNode = new TreeNode($"</{htmlNode.Name}>")
                {
                    ForeColor = System.Drawing.Color.Gray,
                    Tag = null // No associated XmlNode
                };

                // Ensure the closing tag node is not duplicated
                if (node.Parent != null)
                {
                    TreeNodeCollection siblings = node.Parent.Nodes;
                    if (siblings.Count <= node.Index + 1 || siblings[node.Index + 1].Text != closingTagNode.Text)
                    {
                        siblings.Insert(node.Index + 1, closingTagNode);
                    }
                }
            }
        }
        private void RemoveClosingTagNode(TreeNode node)
        {
            if (node.Parent != null)
            {
                TreeNodeCollection siblings = node.Parent.Nodes;
                if (siblings.Count > node.Index + 1 &&
                    siblings[node.Index + 1].Text.StartsWith("</") &&
                    siblings[node.Index + 1].Text.EndsWith(">") &&
                    siblings[node.Index + 1].Tag == null)
                {
                    siblings.RemoveAt(node.Index + 1);
                }
            }
        }
        private void TreeView_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            treeView.Invalidate();
        }

        public void LoadHtmlToTreeView(string html)
        {
            try
            {
                // Parse HTML using HtmlParserSharp
                var parser = new SimpleHtmlParser();
                var doc = parser.ParseString(html);

                // Clear any existing nodes
                treeView.Nodes.Clear();

                // Add the root node to the TreeView
                TreeNode rootNode = new TreeNode(doc.Name);
                rootNode.Tag = doc;
                treeView.Nodes.Add(rootNode);

                // Populate the TreeView recursively
                AddNodes(doc, rootNode);

                // Expand the tree to show the root initially
                treeView.ExpandAll();

                treeView.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing HTML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddNodes(XmlNode xmlNode, TreeNode treeNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                TreeNode childTreeNode = new TreeNode(GetNodeText(childNode, collapsed: false));
                childTreeNode.Tag = childNode;

                // Add the new TreeNode to the parent TreeNode
                treeNode.Nodes.Add(childTreeNode);

                // Recursively add children to the TreeNode
                AddNodes(childNode, childTreeNode);
            }

            // Insert a closing tag node for this tree node
            InsertClosingTagNode(treeNode);
        }

        private void tabControlTop_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            Graphics g = e.Graphics;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabBounds = e.Bounds;

            // Set background color for all tabs
            g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(60, 60, 60)),
                tabBounds);

            // Highlight selected tab with an underline
            if (e.State == DrawItemState.Selected)
            {
                g.DrawLine(Pens.LightCyan, tabBounds.Left, tabBounds.Bottom - 4, tabBounds.Right, tabBounds.Bottom - 4);
            }

            // Draw the tab text
            StringFormat stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString(tabPage.Text, e.Font, Brushes.Wheat, tabBounds, stringFormat);
        }
    }
}
