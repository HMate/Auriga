using Bifrost;
using Bifrost.Dot;
using Bifrost.GraphElements;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xunit;

namespace AurigaTest.Dot
{
    public class DotGraphConverterTest
    {
        #region Dot -> Graph

        [Fact]
        public void DotToBifrostGraph()
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
        public void DotToBifrostGraphSingleNode()
        {
            DotGraph g = DotLoader.Load(@"graph {a}");
            Graph graph = DotGraphConverter.ToGraph(g);
            Assert.Equal(1, graph.Nodes.Count);
            var nodes = graph.Nodes.ToList();

            var nodeA = nodes[0];
            Assert.Equal("a", nodeA.Id);
            Assert.Equal("a", nodeA.NodeName);
            Assert.Equal(new Point(0, 0), nodeA.Position);
        }


        [Fact]
        public void DotToBifrostGraphUseLabelForName()
        {
            DotGraph g = DotLoader.Load(@"digraph {
            a[label=""good name""];
        }");
            Graph graph = DotGraphConverter.ToGraph(g);
            Assert.Equal(1, graph.Nodes.Count);
            var nodes = graph.Nodes.ToList();

            var nodeA = nodes[0];
            Assert.Equal("a", nodeA.Id);
            Assert.Equal("good name", nodeA.NodeName);
        }

        #endregion
        #region Graph -> Dot

        [Fact]
        public void BifrostToDotGraphSingleNode()
        {
            Graph g = new Graph();
            g.AddNode(new Node("054b46d7-6b2e-46a6-a7c3-16a31c9e1a53", "alpha"));
            DotGraph dot = DotGraphConverter.ToDot(g);

            assertDotContainsNode(dot, "054b46d7-6b2e-46a6-a7c3-16a31c9e1a53", "alpha");
        }

        /// <summary>
        /// Dot files contain unique ids. Make sure that the converter generates a new id
        /// if two nodes have the same id.
        /// </summary>
        [Fact]
        public void BifrostToDotGraphDuplicatedNodeKey()
        {
            Graph g = new Graph();
            g.AddNode(new Node("id1", "beta"));
            g.AddNode(new Node("id2", "theta"));
            g.AddNode(new Node("id1", "gamma"));
            DotGraph dot = DotGraphConverter.ToDot(g);

            assertDotContainsNode(dot, "id1", "beta");
            assertDotContainsNode(dot, "id2", "theta");
            assertDotContainsNode(dot, "id10", "gamma");
            Assert.Equal(3, dot.Nodes.Count);
        }

        [Fact]
        public void BifrostToDotGraphSingleEdge()
        {
            Graph g = new Graph();
            g.AddNode(new Node("id1", "beta"));
            g.AddNode(new Node("id2", "theta"));
            g.AddEdge(new Edge("id1", "id2"));
            DotGraph dot = DotGraphConverter.ToDot(g);

            assertDotContainsNode(dot, "id1", "beta");
            assertDotContainsNode(dot, "id2", "theta");
            Assert.Contains(("id1", "id2"), dot.Edges);
        }

        private void assertDotContainsNode(DotGraph dot, string id, string name)
        {
            Assert.Contains(id, dot.Nodes);
            var node = dot.Nodes[id];
            Assert.Contains("label", node.Attributes);
            Assert.Equal(name, node.Attributes["label"]);
        }

        #endregion
    }
}
