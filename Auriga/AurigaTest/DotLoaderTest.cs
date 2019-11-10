using Bifrost;
using DotParser;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AurigaTest
{
    public class DotLoaderTest
    {
        [Fact]
        public void LoadEmptyDot()
        {
            GraphData g = DotLoader.Load("");
            Assert.Equal(0, g.Nodes.Count);
        }

        [Fact]
        public void LoadInvalid()
        {
            GraphData g = DotLoader.Load("défáûéqlgf3i59üöflB:B%!ÖIWRJV");
            Assert.Equal(0, g.Nodes.Count);
        }

        [Fact]
        public void LoadEmptyGraph()
        {
            GraphData g = DotLoader.Load("graph {}");
            Assert.Equal(0, g.Nodes.Count);
        }

        [Fact]
        public void LoadUndirected()
        {
            GraphData g = DotLoader.Load(@"graph {
	cartographer[shape=rect]
	a -- b
	b -- cartographer
}");
            Assert.False(g.IsDirected);
            Assert.True(g.Nodes.ContainsKey("a"));
            Assert.True(g.Nodes.ContainsKey("b"));
            Assert.True(g.Nodes.ContainsKey("cartographer"));
            Assert.Equal(3, g.Nodes.Count);

            Assert.True(g.Edges.ContainsKey(Tuple.Create("a", "b")));
            Assert.True(g.Edges.ContainsKey(Tuple.Create("b", "cartographer")));
            Assert.Equal(2, g.Edges.Count);
        }

        [Fact]
        public void LoadDetailed()
        {
            GraphData g = DotLoader.Load(@"graph {
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
    }
}
