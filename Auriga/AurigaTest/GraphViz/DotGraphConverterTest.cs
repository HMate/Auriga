using Bifrost;
using Bifrost.Dot;
using Bifrost.GraphElements;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xunit;

namespace AurigaTest.GraphViz
{
    public class DotGraphConverterTest
    {
        [Fact]
        public void ConvertDotGraphToBifrostGraph()
        {
            DotGraph g = DotLoader.Load(@"digraph {
            graph [bb=""0, 0, 54, 108""];
            node[label = ""\N""];
            a[height = 0.5,
                    pos = ""27,90"",
                    width = 0.75];
            b[height = 0.5,
                    pos = ""27,18"",
                    width = 0.75];
            a->b[pos = ""e,27,36.104 27,71.697 27,63.983 27,54.712 27,46.112""];
        }");
            Graph graph = DotGraphConverter.ToGraph(g);
            Assert.Equal(2, graph.Nodes.Count);
            var nodes = graph.Nodes.ToList();

            var nodeA = nodes[0];
            Assert.Equal("a", nodeA.NodeName);
            Assert.Equal(new Point(27, 18), nodeA.Position);

            var nodeB = nodes[1];
            Assert.Equal("b", nodeB.NodeName);
            Assert.Equal(new Point(27, 90), nodeB.Position);

            List<Edge> edges = graph.Edges.ToList();
            Assert.Equal(nodeA, graph.FindNode(edges[0].StartId));
            Assert.Equal(nodeB, graph.FindNode(edges[0].EndId));
        }

        [Fact]
        public void ConvertBifrostGraphToDotGraphSingleNode()
        {
            Graph g = new Graph();
            g.AddNode(System.Guid.Parse("054b46d7-6b2e-46a6-a7c3-16a31c9e1a53"), "alpha");
            DotGraph dot = DotGraphConverter.ToDot(g);

            assertDotContainsNode(dot, "054b46d7-6b2e-46a6-a7c3-16a31c9e1a53", "alpha");
        }

        [Fact]
        public void ConvertBifrostGraphToDotGraphSingleEdge()
        {
            Graph g = new Graph();
            g.AddNode(System.Guid.Parse("7cbeb388-085b-4087-9fec-50d7f180e334"), "beta");
            g.AddNode(System.Guid.Parse("29a0c384-49b7-472b-814a-01e8e5453025"), "theta");
            g.AddEdge(System.Guid.Parse("7cbeb388-085b-4087-9fec-50d7f180e334"), 
                System.Guid.Parse("29a0c384-49b7-472b-814a-01e8e5453025"));
            DotGraph dot = DotGraphConverter.ToDot(g);

            assertDotContainsNode(dot, "7cbeb388-085b-4087-9fec-50d7f180e334", "beta");
            assertDotContainsNode(dot, "29a0c384-49b7-472b-814a-01e8e5453025", "theta");

            Assert.Contains(("7cbeb388-085b-4087-9fec-50d7f180e334", "29a0c384-49b7-472b-814a-01e8e5453025"), dot.Edges);
        }

        private void assertDotContainsNode(DotGraph dot, string id, string name)
        {
            Assert.Contains(id, dot.Nodes);
            var node = dot.Nodes[id];
            Assert.Contains("label", node.Attributes);
            Assert.Equal(name, node.Attributes["label"]);
        }
    }
}

