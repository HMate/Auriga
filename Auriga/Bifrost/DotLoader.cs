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

            string? lastNode = null;
            if (tokenIter.Current == "{")
            {
                tokenIter.MoveNext();

                while (tokenIter.Current != "}")
                {
                    string token = tokenIter.Current;

                    if(token == "--")
                    {
                        tokenIter.MoveNext();
                        string nextNode = tokenIter.Current;
                        gr.Nodes.Add(nextNode, new Dot.DotNode());
                        if(lastNode != null)
                            gr.Edges.Add((lastNode, nextNode), new Dot.DotEdge());
                    }
                    else
                    {
                        gr.Nodes.Add(token, new Dot.DotNode());
                        lastNode = token;
                    }

                    tokenIter.MoveNext();
                }
            }


            return gr;
        }

        private static List<string> tokenize(string text)
        {
            List<string> tokens = new List<string>(text.Split(new[] { " ", "\t", "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            tokens = tokens.SelectMany(t => Regex.Split(t, @"([\][}{]|--)")).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
            return tokens;
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
