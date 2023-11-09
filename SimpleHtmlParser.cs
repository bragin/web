using HtmlParserSharp.Core;
using HtmlParserSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SkiaSharpOpenGLBenchmark
{


    public class SimpleHtmlParser
    {
        private Tokenizer tokenizer;
        private XmlTreeBuilder treeBuilder;

        public XmlDocumentFragment ParseStringFragment(string str, string fragmentContext)
        {
            using (var reader = new StringReader(str))
                return ParseFragment(reader, fragmentContext);
        }

        public XmlDocument ParseString(string str)
        {
            using (var reader = new StringReader(str))
                return Parse(reader);
        }

        public XmlDocument Parse(string path)
        {
            using (var reader = new StreamReader(path))
                return Parse(reader);
        }

        public XmlDocument Parse(TextReader reader)
        {
            Reset();
            Tokenize(reader);
            return treeBuilder.Document;
        }

        public XmlDocumentFragment ParseFragment(TextReader reader, string fragmentContext)
        {
            Reset();
            treeBuilder.SetFragmentContext(fragmentContext);
            Tokenize(reader);
            return treeBuilder.getDocumentFragment();
        }

        private void Reset()
        {
            treeBuilder = new XmlTreeBuilder();
            tokenizer = new Tokenizer(treeBuilder, false);
            treeBuilder.WantsComments = false;

            // optionally: report errors and more

            //treeBuilder.ErrorEvent +=
            //    (sender, a) =>
            //    {
            //        ILocator loc = tokenizer as ILocator;
            //        Console.WriteLine("{0}: {1} (Line: {2})", a.IsWarning ? "Warning" : "Error", a.Message, loc.LineNumber);
            //    };
            //treeBuilder.DocumentModeDetected += (sender, a) => Console.WriteLine("Document mode: " + a.Mode.ToString());
            //tokenizer.EncodingDeclared += (sender, a) => Console.WriteLine("Encoding: " + a.Encoding + " (currently ignored)");
        }

        private void Tokenize(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader was null.");
            }

            tokenizer.Start();
            bool swallowBom = true;

            try
            {
                char[] buffer = new char[2048];
                UTF16Buffer bufr = new UTF16Buffer(buffer, 0, 0);
                bool lastWasCR = false;
                int len = -1;
                if ((len = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    int streamOffset = 0;
                    int offset = 0;
                    int length = len;
                    if (swallowBom)
                    {
                        if (buffer[0] == '\uFEFF')
                        {
                            streamOffset = -1;
                            offset = 1;
                            length--;
                        }
                    }
                    if (length > 0)
                    {
                        tokenizer.SetTransitionBaseOffset(streamOffset);
                        bufr.Start = offset;
                        bufr.End = offset + length;
                        while (bufr.HasMore)
                        {
                            bufr.Adjust(lastWasCR);
                            lastWasCR = false;
                            if (bufr.HasMore)
                            {
                                lastWasCR = tokenizer.TokenizeBuffer(bufr);
                            }
                        }
                    }
                    streamOffset = length;
                    while ((len = reader.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        tokenizer.SetTransitionBaseOffset(streamOffset);
                        bufr.Start = 0;
                        bufr.End = len;
                        while (bufr.HasMore)
                        {
                            bufr.Adjust(lastWasCR);
                            lastWasCR = false;
                            if (bufr.HasMore)
                            {
                                lastWasCR = tokenizer.TokenizeBuffer(bufr);
                            }
                        }
                        streamOffset += len;
                    }
                }
                tokenizer.Eof();
            }
            finally
            {
                tokenizer.End();
            }
        }
    }

    public enum NodeType : byte
    {
        /// <summary>
        /// An element node.
        /// </summary>
        ELEMENT_NODE = 1,

        //ATTRIBUTE_NODE =2,
        /// <summary>
        /// A text node.
        /// </summary>
        TEXT_NODE = 3,
        /// <summary>
        /// A CDATA node.
        /// </summary>
        CDATA_SECTION_NODE = 4,
        //ENTITY_REFERENCE_NODE = 5,
        //ENTITY_NODE=  6,
        //PROCESSING_INSTRUCTION_NODE =7,
        /// <summary>
        /// A comment node.
        /// </summary>
        COMMENT_NODE = 8,
        /// <summary>
        /// A document node.
        /// </summary>
        DOCUMENT_NODE = 9,
        /// <summary>
        /// The DOCTYPE node.
        /// </summary>
        DOCUMENT_TYPE_NODE = 10,
        /// <summary>
        /// A document fragment node.
        /// </summary>
        DOCUMENT_FRAGMENT_NODE = 11,
        //NOTATION_NODE  =12
    }

    public interface INodeList : IEnumerable<IDomObject>, IList<IDomObject>, ICollection<IDomObject>
    {
        /// <summary>
        /// The number of nodes in this INodeList
        /// </summary>

        int Length { get; }

        /// <summary>
        /// Get the item at the specified index
        /// </summary>
        ///
        /// <param name="index">
        /// Zero-based index of the item
        /// </param>
        ///
        /// <returns>
        /// An item
        /// </returns>

        IDomObject Item(int index);

        /// <summary>
        /// Event raised when the NodeList changes
        /// </summary>

        //event EventHandler<NodeEventArgs> OnChanged;
    }

    public interface IDomNode : ICloneable
    {
        INodeList ChildNodes { get; }
        NodeType NodeType { get; }
    }

    public interface IDomObject : IDomNode
    {
        IDomObject this[int index] { get; }
        IDomContainer ParentNode { get; }
        IDomObject NextSibling { get; }
        string Id { get; set; }
        string Name { get; set; }
        IEnumerable<string> Classes { get; }
    }

    public interface IDomContainer : IDomObject
    {
        /// <summary>
        /// An enumeration of clones of the children of this object
        /// </summary>
        ///
        /// <returns>
        /// An enumerator 
        /// </returns>

        IEnumerable<IDomObject> CloneChildren();
    }
}