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
        private List<string>.Enumerator tokenIter;

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
            tokenIter = tokens.GetEnumerator();

            string? first = nextToken();
            if (string.IsNullOrWhiteSpace(first) || 
                (!first.Equals("graph", casei) && 
                !first.Equals("digraph", casei) && 
                !first.Equals("strict", casei)))
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
                string? lastToken = null;
                IDictionary<string, string> attributeContext = gr.GraphAttributes;
                for (string? token = nextToken(); token != "}" && token != null; token = nextToken())
                {
                    if(token == "--" || token == "->")
                    {
                        string? nextNode = nextToken();
                        if (nextNode == null || nextNode == "}")
                        {
                            break;
                        }
                        gr.Nodes.TryAdd(nextNode, new Dot.DotNode());
                        if (lastToken != null)
                        {
                            gr.Nodes.TryAdd(lastToken, new Dot.DotNode());
                            gr.Edges.Add((lastToken, nextNode), new Dot.DotEdge());
                        }
                        lastToken = nextNode;
                    }
                    else if (token == "[")
                    {
                        if (lastToken != null)
                        {
                            gr.Nodes.TryAdd(lastToken, new Dot.DotNode());
                            attributeContext = gr.Nodes[lastToken].Attributes;
                        }
                        else
                        {
                            attributeContext = gr.GraphAttributes;
                        }
                        lastToken = null;
                    }
                    else if (token == "]")
                    {
                        attributeContext = gr.GraphAttributes;
                    }
                    else if(token == "=")
                    {
                        string? val = nextToken();
                        if (val == null || val == "}")
                        {
                            break;
                        }
                        if (lastToken != null)
                            attributeContext.Add(lastToken, val);
                        lastToken = null;
                    }
                    else
                    {
                        if (lastToken != null)
                        {
                            gr.Nodes.TryAdd(lastToken, new Dot.DotNode());
                        }
                        lastToken = token;
                    }
                }

                if (lastToken != null)
                {
                    gr.Nodes.TryAdd(lastToken, new Dot.DotNode());
                }
            }


            return gr;
        }

        private string? nextToken()
        {
            tokenIter.MoveNext();
            return tokenIter.Current;
        }

        private static List<string> tokenize(string text)
        {
            List<string> tokens = new List<string>(
                text.Split(new[] { " ", "\t", "\n", "\r", "\r\n" }, 
                StringSplitOptions.RemoveEmptyEntries));
            tokens = tokens.SelectMany(t => Regex.Split(t, @"([\][}{]|--|->|=)"))
                .Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
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
