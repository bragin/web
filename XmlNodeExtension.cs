﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SkiaSharpOpenGLBenchmark.css;

namespace SkiaSharpOpenGLBenchmark
{
    public static class XmlNodeExtensions
    {
        private static readonly Dictionary<XmlNode, CssNodeData> _data = new Dictionary<XmlNode, CssNodeData>();

        public static CssNodeData GetNodeData(this XmlNode node)
        {
            if (_data.TryGetValue(node, out CssNodeData data))
            {
                return data;
            }

            return null;
        }

        public static void SetNodeData(this XmlNode node, CssNodeData data)
        {
            _data[node] = data;
        }

        //TODO: Implement a method to remove data if necessary
    }
}
