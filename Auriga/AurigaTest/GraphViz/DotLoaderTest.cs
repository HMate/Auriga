using Bifrost;
using Bifrost.Dot;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AurigaTest.GraphViz
{
    public class DotLoaderTest
    {
        [Theory]
        [InlineData(@"graph {}")]
        [InlineData(@"graph{}")]
        [InlineData(@"graph G { }")]
        [InlineData(@"graph G{}")]
        [InlineData(@"")]
        [InlineData(@"ápwrkglápwkg48zu,.,kp3u9u{}")]
        [InlineData(@"graph")]
        [InlineData(@"graph {")]
        [InlineData(@"strict")]
        public void LoadEmptyGraph(string dot)
        {
            DotGraph g = DotLoader.Load(dot);
            Assert.Empty(g.Nodes);
            Assert.False(g.IsDirected);
            Assert.False(g.IsStrict);
        }

        [Theory]
        [InlineData(@"digraph {}")]
        [InlineData(@"digraph { }")]
        [InlineData(@"digraph G {}")]
        [InlineData(@"digraph{}")]
        [InlineData(@"digraph{  \n\n }")]
        [InlineData(@"digraph G{}")]
        [InlineData(@"strict digraph asd {}")]
        [InlineData(@"strict digraph {}")]
        public void LoadEmptyDirectedGraph(string dot)
        {
            DotGraph g = DotLoader.Load(dot);
            Assert.True(g.IsDirected);
        }

        [Theory]
        [InlineData(@"strict graph{}")]
        [InlineData(@"strict digraph {}")]
        [InlineData(@"strict digraph{}")]
        [InlineData(@"strict digraph asd {}")]
        public void LoadStrictGraph(string dot)
        {
            DotGraph g = DotLoader.Load(dot);
            Assert.True(g.IsStrict);
        }

        [Theory]
        [InlineData(@"graph {Welcome}")]
        [InlineData(@"graph G {Welcome}")]
        [InlineData(@"strict graph asd {Welcome}")]
        [InlineData(@"strict graph {Welcome}")]
        public void LoadOneNode(string dot)
        {
            DotGraph g = DotLoader.Load(dot);
            Assert.Contains("Welcome", g.Nodes);
            Assert.Equal(1, g.Nodes.Count);
        }

        [Theory]
        [InlineData(@"graph G {Welcome}", new[] { "Welcome" })]
        [InlineData(@"graph {Welcome a b}", new[] { "Welcome", "a", "b" })]
        [InlineData(@"graph test {a -- b}", new[] { "a", "b" })]
        [InlineData(@"graph test {a--b -- c}", new[] { "a", "b", "c" })]
        [InlineData(@"graph test {a -- b -- a}", new[] { "a", "b" })]
        [InlineData(@"graph test {a -- b -- c f}", new[] { "a", "b", "c", "f" })]
        [InlineData(@"graph test {a -- }", new[] { "a" })]
        [InlineData(@"graph test {a -- ", new[] { "a" })]
        [InlineData(@"strict graph {""quoted""}", new[] { "quoted" })]
        [InlineData(@"strict graph {""quoted text node""}", new[] { "quoted text node" })]
        [InlineData(@"strict graph {""quoted \""text\"" node""}", new[] { "quoted \"text\" node" })]
        [InlineData(@"strict graph {a;b;}", new[] { "a", "b" })]
        [InlineData(@"strict graph {a[ke=ve];b;}", new[] { "a", "b" })]
        [InlineData(@"strict graph {a--b;}", new[] { "a", "b" })]
        public void LoadMultipleNodes(string dot, string[] keys)
        {
            DotGraph g = DotLoader.Load(dot);
            foreach (var key in keys)
            {
                Assert.Contains(key, g.Nodes);
            }
            foreach (var key in g.Nodes.Keys)
            {
                Assert.Contains(key, keys);
            }
            Assert.Equal(keys.Length, g.Nodes.Count);
        }

        [Theory]
        [InlineData(@"graph test {a -- b}", new [] { "a", "b" })]
        [InlineData(@"graph test {a--b}", new[] { "a", "b" })]
        [InlineData(@"graph test {a--b -- c}", new[] { "a", "b", "b", "c" })]
        [InlineData(@"digraph test {a -> b}", new[] { "a", "b" })]
        [InlineData(@"digraph test {a->b}", new[] { "a", "b" })]
        [InlineData(@"digraph test {a->b; d->e}", new[] { "a", "b", "d", "e" })]
        public void LoadGraphEdges(string dot, string[] edgeNodes)
        {
            DotGraph g = DotLoader.Load(dot);

            var edgePairs = toTupleList(edgeNodes);
            foreach (var edge in edgePairs)
            {
                Assert.Contains(edge, g.Edges);
            }
            foreach (var key in g.Edges.Keys)
            {
                Assert.Contains(key, edgePairs);
            }
            Assert.Equal(edgePairs.Count, g.Edges.Count);
        }

        [Fact]
        public void LoadGraphMultiEdges()
        {
            DotGraph g = DotLoader.Load(@"graph test {a -- b[color=red]; a--b}");

            Assert.Equal(2, g.Edges[("a", "b")].Count);
            Assert.Equal("red", g.Edges[("a", "b")].First().Attributes["color"]);
            Assert.Equal(1, g.Edges[("a", "b")].First().Attributes.Count);
            Assert.Equal(0, g.Edges[("a", "b")][1].Attributes.Count);
        }
        
        [Theory]
        [InlineData(@"graph test {key=val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key = val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key= val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key =val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key = val size=test}", new[] { "key", "val", "size", "test" })]
        [InlineData(@"graph test {[key = val]}", new[] { "key", "val" })]
        [InlineData(@"graph test {a graph [key = val]}", new[] { "key", "val" })]
        public void LoadGraphAttributes(string dot, string[] keyvals)
        {
            DotGraph g = DotLoader.Load(dot);

            var attributes = toDictionary(keyvals);
            foreach (var attr in attributes)
            {
                Assert.Contains(attr, g.GraphAttributes);
            }
            foreach (var key in g.GraphAttributes)
            {
                Assert.Contains(key, attributes);
            }
        }

        [Theory]
        [InlineData(@"graph test {node [key = val]}", new[] { "key", "val" })]
        [InlineData(@"graph test {a node[key = val]}", new[] { "key", "val" })]
        public void LoadGlobalNodeAttributes(string dot, string[] keyvals)
        {
            DotGraph g = DotLoader.Load(dot);

            var attributes = toDictionary(keyvals);
            foreach (var attr in attributes)
            {
                Assert.Contains(attr, g.NodeAttributes);
            }
            foreach (var key in g.NodeAttributes)
            {
                Assert.Contains(key, attributes);
            }
        }


        [Theory]
        [InlineData(@"graph test {a[key=val]}", new[] { "key", "val" })]
        [InlineData(@"graph test {a[key=val test=""rand""]}", new[] { "key", "val", "test", "rand" })]
        public void LoadNodeAttributes(string dot, string[] keyvals)
        {
            DotGraph g = DotLoader.Load(dot);

            Assert.Contains("a", g.Nodes);
            Assert.Equal(1, g.Nodes.Count);

            DotNode node = g.Nodes["a"];
            var attributes = toDictionary(keyvals);
            foreach (var attr in attributes)
            {
                Assert.Contains(attr, node.Attributes);
            }
            foreach (var key in g.GraphAttributes)
            {
                Assert.Contains(key, attributes);
            }
        }

        [Fact]
        public void LoadGraphWithNodes()
        {
            DotGraph g = DotLoader.Load(@"graph test {
	cartographer[shape=rect]
	a -- b
	b -- cartographer
}");
            Assert.True(g.Nodes.ContainsKey("a"));
            Assert.True(g.Nodes.ContainsKey("b"));
            Assert.True(g.Nodes.ContainsKey("cartographer"));
            Assert.Equal(3, g.Nodes.Count);

            Assert.True(g.Edges.ContainsKey(("a", "b")));
            Assert.True(g.Edges.ContainsKey(("b", "cartographer")));
            Assert.Equal(2, g.Edges.Count);
        }

        [Fact]
        public void LoadDetailed()
        {
            DotGraph g = DotLoader.Load(@"graph {
	graph [bb=""0, 0, 87, 180""];
    node[label = ""\N""];
            cartographer[height = 0.5,
                pos = ""43.5,18"",
                shape = rect,
                width = 1.2083];
            a[height = 0.5,
                pos = ""43.5,162"",
                width = 0.75];
            b[height = 0.5,
                pos = ""43.5,90"",
                width = 0.75];
            a-- b[pos = ""43.5,143.7 43.5,132.85 43.5,118.92 43.5,108.1""];
            b-- cartographer[pos = ""43.5,71.697 43.5,60.846 43.5,46.917 43.5,36.104""];
        }");
            Assert.False(g.IsDirected);
            Assert.Equal("0, 0, 87, 180", g.GraphAttributes["bb"]);
            Assert.Equal("\\N", g.NodeAttributes["label"]);

            Assert.Equal("43.5,162", g.Nodes["a"].Attributes["pos"]);
            Assert.Equal("43.5,143.7 43.5,132.85 43.5,118.92 43.5,108.1", g.Edges[("a", "b")].First().Attributes["pos"]);
        }

        [Fact]
        public void LoadSameAttribute()
        {
            DotGraph g = DotLoader.Load(@"graph {
            cartographer[height = 0.5,
                pos = ""43.5,18"",
                shape = rect,
                width = 1.2083,
                width = 1.2084,
                width = 1.2086,
                shape = ""rose,""];
            a[color=red] a -- b[color=red, color=""blue""] a[color=green]
        }");
            Assert.Equal("1.2086", g.Nodes["cartographer"].Attributes["width"]);
            Assert.Equal("rose,", g.Nodes["cartographer"].Attributes["shape"]);
            Assert.Equal("green", g.Nodes["a"].Attributes["color"]);
            Assert.Equal(3, g.Nodes.Count);
        }

        [Fact]
        public void LoadDirected()
        {
            DotGraph g = DotLoader.Load(@"digraph {	a -> b;}");
            Assert.True(g.IsDirected);
            Assert.Equal(2, g.Nodes.Count);
            Assert.Contains(("a", "b"), g.Edges);
        }

        [Theory]
        [InlineData(@"graph {key}", new[] { "graph", "{", "key", "}" })]
        [InlineData(@"graph {key=val}", new[] { "graph", "{", "key", "=", "val", "}" })]
        [InlineData(@"graph {key=val foo=bar}", new[] { "graph", "{", "key", "=", "val", "foo", "=", "bar", "}" })]
        [InlineData(@"graph {key=val, foo=bar}", new[] { "graph", "{", "key", "=", "val", ",", "foo", "=", "bar", "}" })]
        [InlineData(@"graph {""key""=""val""}", new[] { "graph", "{", "key", "=", "val", "}" })]
        [InlineData(@"graph {""key=val""}", new[] { "graph", "{", "key=val", "}" })]
        [InlineData(@"graph {""key=\""val\""""}", new[] { "graph", "{", "key=\"val\"", "}" })]
        [InlineData(@"graph {""\""some value\""""}", new[] { "graph", "{", "\"some value\"", "}" })]
        [InlineData(@"graph G {Welcome}", new[] { "graph", "G", "{", "Welcome", "}" })]
        public void TokenizerTest(string input, string[] tokens)
        {
            var parts = DotLoader.Tokenizer.Tokenize(input);
            for(int i = 0; i< tokens.Count(); i++)
            {
                if (parts.Count() > i)
                {
                    Assert.Equal(tokens[i], parts[i]);
                }
                else
                {
                    Assert.True(false, $"[{string.Join(",", parts)}] contains too few elements");
                }
            }
        }

        [Theory]
        [InlineData(@"graph ""key"" size", new[] { "graph ", "\"key\"", " size" }, new[] { false, true, false })]
        [InlineData(@"""key asd d""", new[] { "\"key asd d\"" }, new[] { true })]
        [InlineData(@"""key \""quoted\"" test""", new[] { "\"key \"quoted\" test\"" }, new[] { true})]
        [InlineData(@"foo ""key second"" woah ""common""", new[] { "foo ", "\"key second\"", " woah ", "\"common\"" }, new[] { false, true, false, true })]
        public void TokenizerQuoteSplitTest(string input, string[] tokens, bool[] isQuoted)
        {
            var parts = DotLoader.Tokenizer.SplitQuotedParts(input);
            for (int i = 0; i < tokens.Count(); i++)
            {
                if (parts.Count() > i)
                {
                    Assert.Equal(tokens[i], parts[i].token);
                    Assert.Equal(isQuoted[i], parts[i].isQuoted);
                }
                else
                {
                    Assert.True(false, $"[{string.Join(",", parts)}] contains too few elements");
                }
            }
        }

        private static Dictionary<string, string> toDictionary(string[] keyvals)
        {
            return keyvals.Zip(keyvals.Skip(1)).Where((t, i) => i % 2 == 0)
                            .ToDictionary(kv => kv.First, kv => kv.Second);
        }

        private static List<(string First, string Second)> toTupleList(string[] edgeNodes)
        {
            return edgeNodes.Zip(edgeNodes.Skip(1)).Where((t, i) => i % 2 == 0).ToList();
        }
    }
}
