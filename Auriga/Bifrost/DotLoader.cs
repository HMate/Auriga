using DotParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            List<string> tokens = Tokenizer.Tokenize(dotString);
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
                        lastToken = nextNode.Trim('"');
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
                        lastToken = token.Trim('"');
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

        public class Tokenizer
        {
            public static List<string> Tokenize(string text)
            {
                List<(bool quoted, string token)> quotedTokens = SplitQuotedParts(text);
                List<string> tokens = new List<string>();
                foreach (var token in quotedTokens)
                {
                    if (token.quoted)
                    {
                        tokens.Add(token.token);
                    }
                    else
                    {
                        var parts = token.token.Split(new[] { " ", "\t", "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        tokens.AddRange(parts.SelectMany(t => Regex.Split(t, @"([\][}{]|--|->|=)"))
                            .Where(t => !string.IsNullOrWhiteSpace(t)));
                    }
                }
                return tokens;
            }

            public static List<(bool isQuoted, string token)> SplitQuotedParts(string text)
            {
                // possible inputs:
                // asd "ff" dd
                // asd "f \"dasd\" f" dd
                // "f \"dasd\" f"
                List<(bool isQuoted, string token)> tokens = new List<(bool isQuoted, string token)>();
                bool inQuote = false;
                StringBuilder builder = new StringBuilder();
                void saveToken()
                {
                    string token = builder.ToString();
                    if (!string.IsNullOrEmpty(token))
                    {
                        tokens.Add((false, builder.ToString()));
                        builder.Clear();
                    }
                }
                int escapeCount = 0;
                foreach (var ch in text)
                {
                    if (!inQuote && ch == '"' && escapeCount % 2 == 0)
                    {
                        saveToken();
                        inQuote = true;
                        builder.Append(ch);
                        escapeCount = 0;
                    }
                    else if (inQuote && ch == '"' && escapeCount % 2 == 0)
                    {
                        inQuote = false;
                        builder.Append(ch);
                        tokens.Add((true, builder.ToString()));
                        builder.Clear();
                        escapeCount = 0;
                    }
                    else if (ch == '\\')
                    {
                        escapeCount++;
                        builder.Append(ch);
                    }
                    else
                    {
                        escapeCount = 0;
                        builder.Append(ch);
                    }
                }
                saveToken();

                return tokens;
            }
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
