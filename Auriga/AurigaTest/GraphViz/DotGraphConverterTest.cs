using Bifrost;
using Bifrost.Dot;
using Bifrost.GraphElements;
using DotParser;
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
            GraphData g = DotLoader.LoadF(@"digraph {
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
    }
}

