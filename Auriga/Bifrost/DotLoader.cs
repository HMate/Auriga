using DotParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bifrost
{
    public class DotLoader
    {
        private string dotString;
        private int pos = 0;
        private int trypos = 0;
        private const StringComparison casei = StringComparison.OrdinalIgnoreCase;

        public DotLoader(string dotString)
        {
            this.dotString = dotString;
        }

        public static Dot.DotGraph Load(string dotString)
        {
            DotLoader loader = new DotLoader(dotString);
            return loader.Load();
        }

        public Dot.DotGraph Load()
        {
            Dot.DotGraph gr = new Dot.DotGraph();
            dotString = dotString.Trim();

            if(dotString.Length < 7)
            {
                return gr;
            }

            List<string> tokens = tokenize(dotString);
            var tokenIter = tokens.GetEnumerator();

            tokenIter.MoveNext();
            string first = tokenIter.Current;
            if (!first.Equals("graph", casei) && !first.Equals("digraph", casei) && !first.Equals("strict", casei))
            {
                return gr;
            }

            if(first.Equals("strict", casei))
            {
                gr.IsStrict = true;
                tokenIter.MoveNext();
            }
            gr.IsDirected = tokenIter.Current.Equals("digraph", casei);
            tokenIter.MoveNext();

            if (tokenIter.Current != "{")
            {
                tokenIter.MoveNext();
            }
            if (tokenIter.Current == "{")
            {
                tokenIter.MoveNext();
                string node = tokenIter.Current;

                if (tokenIter.Current != "}")
                {
                    gr.Nodes.Add(node, new Dot.DotNode());
                }
            }
            //string token = readUntil(' ', '{');
            //if(token.Equals("graph", casei) || token.Equals("digraph", casei))
            //{
            //    if(current() != '{')
            //    {
            //        string name = readUntil('{').Trim();
            //    }

            //    while (current() != '}')
            //    {
            //        tryReadNodeStatement();
            //    } 
            //}

            return gr;
        }

        private static List<string> tokenize(string text)
        {
            List<string> tokens = new List<string>(text.Split(new[] { " ", "\t", "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            tokens = tokens.SelectMany(t => Regex.Split(t, @"([\][}{])")).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            return tokens;
        }

        private bool tryReadNodeStatement()
        {
            resetTryPos();
            return tryReadID() && tryReadAttrList();
        }

        private bool tryReadAttrList()
        {
            throw new NotImplementedException();
        }

        private bool tryReadID()
        {
            while (char.IsWhiteSpace(dotString, trypos))
                trypos++;
            if (trypos >= dotString.Length)
            {
                return false;
            }
            if(dotString[trypos] == '"' )
            {
                trypos++;
                while (!(dotString[trypos] == '"' && dotString[trypos-1] == '\\') && trypos < dotString.Length)
                    trypos++;
                if (trypos >= dotString.Length)
                {
                    return false;
                }
            }
            return true;
        }

        private bool tryReadEdgeStatement()
        {
            throw new NotImplementedException();
        }

        private bool tryReadAttrStatement()
        {
            throw new NotImplementedException();
        }

        private bool tryReadIDAssignment()
        {
            throw new NotImplementedException();
        }

        private bool tryReadSubGraphStatement()
        {
            throw new NotImplementedException();
        }

        private void resetTryPos()
        {
            trypos = pos;
        }

        private void advancePos()
        {
            pos = trypos;
        }

        private string readUntil(params char[] separators)
        {
            int index = dotString.IndexOfAny(separators, pos);
            int charCount = index - pos;
            if (index < 0 || dotString.Length - pos - 1 < charCount)
            {
                charCount = dotString.Length - pos - 1;
            }
            string token = dotString.Substring(pos, charCount);
            pos = index;
            return token;
        }

        private char current()
        {
            return dotString[pos];
        }

        public static GraphData LoadF(string dotString)
        {
            try
            {
                GraphData gr = DotParser.DotParser.parse(dotString);
                return gr;
            }
#pragma warning disable CA1031 // Do not catch general exception types -> Sadly DotParser throws System.Exception
            catch (Exception)
            {
                return DotParser.DotParser.parse("graph{}");
            }
#pragma warning restore CA1031 // Do not catch general exception types

        }
    }
}
