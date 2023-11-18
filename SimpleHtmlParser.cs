using HtmlParserSharp.Core;
using HtmlParserSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

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

        public XmlDocument ParseUrl(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);

            var doc = Parse(sr);
            //string result = sr.ReadToEnd();

            sr.Close();
            myResponse.Close();

            return doc;
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
}