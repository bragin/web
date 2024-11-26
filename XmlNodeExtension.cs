using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SkiaSharpOpenGLBenchmark.css;

namespace SkiaSharpOpenGLBenchmark
{
    public static class XmlNodeExtensions
    {
        private static readonly Dictionary<XmlNode, CssNodeData> _data = new Dictionary<XmlNode, CssNodeData>();

        // select.c:1752
        // get_libcss_node_data()
        public static CssNodeData GetNodeData(this XmlNode node)
        {
            if (_data.TryGetValue(node, out CssNodeData data))
            {
                return data;
            }

            return null;
        }

        // select.c:1732
        // set_libcss_node_data()
        public static void SetNodeData(this XmlNode node, CssNodeData data)
        {
            _data[node] = data;
        }

        //TODO: Implement a method to remove data if necessary

        public static bool HasClass(this XmlNode node, string name)
        {
            var classes = node.Attributes["class"];
            if (classes == null) return false;

            var classesArray = classes.Value.Split(' ');

            foreach (var c in classesArray)
            {
                if (c == name) return true;
            }

            return false;
        }

        public static bool HasId(this XmlNode node, string name)
        {
            var classes = node.Attributes["id"];
            if (classes == null) return false;

            var classesArray = classes.Value.Split(' ');

            foreach (var c in classesArray)
            {
                if (c == name) return true;
            }

            return false;
        }

        public static bool IsLink(this XmlNode node)
        {
            return string.Equals(node.Name, "a", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsVisited(this XmlNode node)
        {
            return false;
        }
        public static bool IsHover(this XmlNode node)
        {
            return false;
        }
        public static bool IsActive(this XmlNode node)
        {
            return false;
        }
        public static bool IsFocus(this XmlNode node)
        {
            return false;
        }

        public static bool IsRoot(this XmlNode node)
        {
            if (node.ParentNode == null)
                return true;
            else
                return false;
        }

        public static bool IsEmpty(this XmlNode node)
        {
            Log.Unimplemented();
            return false;
        }

        // Just for debugging
        public static void Dump(this XmlNode node)
        {
            Console.Write($"Node: {node.Name} ({node.GetHashCode()}), type: {node.NodeType }");

            var data = GetNodeData(node);
            if (data != null )
            {
                Console.Write($" {data.ToString()}");
            }

            Console.WriteLine();
        }
    }
}
