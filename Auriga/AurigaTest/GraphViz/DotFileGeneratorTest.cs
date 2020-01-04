using Bifrost.Dot;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AurigaTest.GraphViz
{
    public class DotFileGeneratorTest
    {
        [Fact]
        public void SimpleNode()
        {
            DotGraph gr = new DotGraph();
            var node = new DotNode();
            gr.Nodes.Add("7cbeb388-085b-4087-9fec-50d7f180e334", node);

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""7cbeb388-085b-4087-9fec-50d7f180e334""
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void NodeWithAttribute()
        {
            DotGraph gr = new DotGraph();
            var node = new DotNode();
            node.Attributes.Add("label", "beta");
            gr.Nodes.Add("7cbeb388-085b-4087-9fec-50d7f180e334", node);

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""7cbeb388-085b-4087-9fec-50d7f180e334""[""label""=""beta""]
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void MultipleNodesWithAttributes()
        {
            DotGraph gr = new DotGraph();
            var node = new DotNode();
            node.Attributes.Add("label", "beta");
            gr.Nodes.Add("7cbeb388-085b-4087-9fec-50d7f180e334", node);
            node = new DotNode();
            node.Attributes.Add("label", "delta");
            node.Attributes.Add("pos", "43,67");
            node.Attributes.Add("color", "blueishgrey");
            gr.Nodes.Add("some id", node);
            node = new DotNode();
            node.Attributes.Add("label", "theta");
            node.Attributes.Add("type", "function-node");
            gr.Nodes.Add("other id", node);

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""7cbeb388-085b-4087-9fec-50d7f180e334""[""label""=""beta""]
""some id""[""label""=""delta"", ""pos""=""43,67"", ""color""=""blueishgrey""]
""other id""[""label""=""theta"", ""type""=""function-node""]
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void SingleEdge()
        {
            DotGraph gr = new DotGraph();
            var node = new DotNode();
            node.Attributes.Add("label", "beta");
            gr.Nodes.Add("7cbeb388-085b-4087-9fec-50d7f180e334", node);
            node = new DotNode();
            node.Attributes.Add("label", "delta");
            gr.Nodes.Add("some id", node);

            var edge = new DotEdge();
            var edgeList = new List<DotEdge>();
            edgeList.Add(edge);
            gr.Edges.Add(("7cbeb388-085b-4087-9fec-50d7f180e334", "some id"), edgeList);

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""7cbeb388-085b-4087-9fec-50d7f180e334""[""label""=""beta""]
""some id""[""label""=""delta""]
""7cbeb388-085b-4087-9fec-50d7f180e334"" -> ""some id""
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }
    }
}
