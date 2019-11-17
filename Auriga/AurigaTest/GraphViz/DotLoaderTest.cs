using Bifrost;
using Bifrost.Dot;
using DotParser;
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
        [InlineData(@"�pwrkgl�pwkg48zu,.,kp3u9u{}")]
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
        public void LoadGraphEdges(string dot, string[] edgeNodes)
        {
            DotGraph g = DotLoader.Load(dot);

            var edgePairs = edgeNodes.Zip(edgeNodes.Skip(1)).Where((t, i) => i % 2 == 0).ToList();
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

        // TODO: Missing multiple edges

        [Theory]
        [InlineData(@"graph test {key=val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key = val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key= val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key =val}", new[] { "key", "val" })]
        [InlineData(@"graph test {key = val size=test}", new[] { "key", "val", "size", "test" })]
        public void LoadGraphAttributes(string dot, string[] keyvals)
        {
            DotGraph g = DotLoader.Load(dot);

            var attributes = keyvals.Zip(keyvals.Skip(1)).Where((t, i) => i % 2 == 0)
                .ToDictionary(kv => kv.First, kv => kv.Second);
            foreach (var attr in attributes)
            {
                Assert.Contains(attr, g.GraphAttributes);
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
            GraphData g = DotLoader.LoadF(@"graph {
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
            Assert.Equal("N", g.NodeAttributes["label"]);

            Assert.Equal("43.5,162", g.Nodes.GetValueOrDefault("a").GetValueOrDefault("pos"));
            Assert.Equal("43.5,143.7 43.5,132.85 43.5,118.92 43.5,108.1", g.Edges.GetValueOrDefault(Tuple.Create("a", "b")).Head.GetValueOrDefault("pos"));
        }

        [Fact]
        public void LoadDirected()
        {
            GraphData g = DotLoader.LoadF(@"digraph {
	a -> b;
}");
            Assert.True(g.IsDirected);
            Assert.Equal(2, g.Nodes.Count);
            Assert.True(g.Edges.ContainsKey(Tuple.Create("a", "b")));
        }

        [Fact]
        public void LoadWrongFormat()
        {
            GraphData g = DotLoader.LoadF(@"digraph {
	a -> b.
}");
            Assert.Equal(0, g.Nodes.Count);
        }
    }
}
