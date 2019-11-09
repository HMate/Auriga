using Auriga.Models;
using Bifrost;
using DotParser;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AurigaTest
{
    public class DotFileLoaderTest
    {
        [Fact]
        public void LoadEmptyDot()
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
            Assert.True(g.Nodes.ContainsKey("a"));
            Assert.True(g.Nodes.ContainsKey("b"));
            Assert.True(g.Nodes.ContainsKey("cartographer"));
            Assert.Equal(3, g.Nodes.Count);

            Assert.True(g.Edges.ContainsKey(Tuple.Create("a", "b")));
            Assert.True(g.Edges.ContainsKey(Tuple.Create("b", "cartographer")));
            Assert.Equal(2, g.Edges.Count);
        }
    }
}
