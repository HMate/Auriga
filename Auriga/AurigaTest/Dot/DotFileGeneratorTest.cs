using Bifrost.Dot;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AurigaTest.Dot
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
            gr.Nodes.Add("abc", new DotNode() { Attributes = { ["label"] = "beta" } });
            gr.Nodes.Add("some id", new DotNode() { Attributes = { ["label"] = "delta" } });

            gr.Edges.Add(("abc", "some id"), new List<DotEdge> { new DotEdge() });

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""abc""[""label""=""beta""]
""some id""[""label""=""delta""]
""abc"" -> ""some id""
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void MultipleEdgeOnSameNodes()
        {
            DotGraph gr = new DotGraph();
            gr.Nodes.Add("abc", new DotNode(){ Attributes = { ["label"] = "beta" }});
            gr.Nodes.Add("some id", new DotNode(){ Attributes = { ["label"] = "delta" }});
            gr.Nodes.Add("third", new DotNode(){ Attributes = { ["other label"] = "asd" }});

            gr.Edges.Add(("abc", "some id"), new List<DotEdge> { new DotEdge(), new DotEdge(), new DotEdge() });

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""abc""[""label""=""beta""]
""some id""[""label""=""delta""]
""third""[""other label""=""asd""]
""abc"" -> ""some id""
""abc"" -> ""some id""
""abc"" -> ""some id""
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void EdgeAttributes()
        {
            DotGraph gr = new DotGraph();
            gr.Nodes.Add("abc", new DotNode() { Attributes = { ["label"] = "beta" } });
            gr.Nodes.Add("some id", new DotNode() { Attributes = { ["label"] = "delta" } });

            gr.Edges.Add(("abc", "some id"), new List<DotEdge> { new DotEdge() { Attributes = { ["edgeAttr"] = "edgy" } } });

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
""abc""[""label""=""beta""]
""some id""[""label""=""delta""]
""abc"" -> ""some id""[""edgeAttr""=""edgy""]
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void GraphAttributes()
        {
            DotGraph gr = new DotGraph();
            gr.GraphAttributes.Add("lex parse", "lorem");
            gr.Nodes.Add("abc", new DotNode() { Attributes = { ["label"] = "beta" } });

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
graph [""lex parse""=""lorem""]
""abc""[""label""=""beta""]
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public void NodeAttributes()
        {
            DotGraph gr = new DotGraph();
            gr.GraphAttributes.Add("lex parse", "lorem");
            gr.NodeAttributes.Add("node attr", "somethg");
            gr.Nodes.Add("abc", new DotNode() { Attributes = { ["label"] = "beta" } });

            string result = DotFileGenerator.Serialize(gr);
            Assert.Equal(@"digraph {
graph [""lex parse""=""lorem""]
node [""node attr""=""somethg""]
""abc""[""label""=""beta""]
}", result, ignoreLineEndingDifferences: true, ignoreWhiteSpaceDifferences: true);
        }
    }
}
