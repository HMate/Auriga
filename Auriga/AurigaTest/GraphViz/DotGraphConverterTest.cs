using Bifrost;
using Bifrost.GraphElements;
using DotParser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Xunit;

namespace AurigaTest.GraphViz
{
    public class DotGraphConverterTest
    {
        [Fact]
        public void ConvertDotGraphToBifrostGraph()
        {
            GraphData g = DotLoader.Load(@"digraph {
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
            var iter = graph.Nodes.GetEnumerator();

            iter.MoveNext();
            Assert.Equal("a", iter.Current.NodeName);
            Assert.Equal(new Point(27, 18), iter.Current.Position);

            iter.MoveNext();
            Assert.Equal("b", iter.Current.NodeName);
            Assert.Equal(new Point(27, 90), iter.Current.Position);
        }
    }
}

