namespace web
{
    partial class FormElements
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControlTop = new TabControl();
            tabPageElements = new TabPage();
            splitContainerElements = new SplitContainer();
            tabPage2 = new TabPage();
            tabControlTop.SuspendLayout();
            tabPageElements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerElements).BeginInit();
            splitContainerElements.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlTop
            // 
            tabControlTop.Appearance = TabAppearance.FlatButtons;
            tabControlTop.Controls.Add(tabPageElements);
            tabControlTop.Controls.Add(tabPage2);
            tabControlTop.Dock = DockStyle.Fill;
            tabControlTop.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlTop.Location = new Point(0, 0);
            tabControlTop.Name = "tabControlTop";
            tabControlTop.Padding = new Point(0, 0);
            tabControlTop.SelectedIndex = 0;
            tabControlTop.Size = new Size(630, 689);
            tabControlTop.TabIndex = 0;
            tabControlTop.DrawItem += tabControlTop_DrawItem;
            // 
            // tabPageElements
            // 
            tabPageElements.Controls.Add(splitContainerElements);
            tabPageElements.Location = new Point(4, 27);
            tabPageElements.Name = "tabPageElements";
            tabPageElements.Size = new Size(622, 658);
            tabPageElements.TabIndex = 0;
            tabPageElements.Text = "Elements";
            tabPageElements.UseVisualStyleBackColor = true;
            // 
            // splitContainerElements
            // 
            splitContainerElements.Dock = DockStyle.Fill;
            splitContainerElements.Location = new Point(0, 0);
            splitContainerElements.Name = "splitContainerElements";
            splitContainerElements.Size = new Size(622, 658);
            splitContainerElements.SplitterDistance = 205;
            splitContainerElements.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 27);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(622, 658);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Sources";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // FormElements
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(40, 40, 40);
            ClientSize = new Size(630, 689);
            Controls.Add(tabControlTop);
            DoubleBuffered = true;
            ForeColor = Color.FromArgb(227, 227, 227);
            Name = "FormElements";
            Text = "Elements";
            tabControlTop.ResumeLayout(false);
            tabPageElements.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerElements).EndInit();
            splitContainerElements.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlTop;
        private TabPage tabPageElements;
        private TabPage tabPage2;
        private SplitContainer splitContainerElements;
    }
}