using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using GraphX.PCL.Logic.Algorithms.OverlapRemoval;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Auriga.Views
{
    public class GraphAreaControl : GraphArea<IncludeGraphNode, IncludeGraphEdge, BidirectionalGraph<IncludeGraphNode, IncludeGraphEdge>>
    {
        static GraphAreaControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphAreaControl), new FrameworkPropertyMetadata(typeof(GraphAreaControl)));
        }

        public GraphAreaControl()
        {
            var graph = GenerateCyclicGraph();
            LayoutGraph();
            GenerateGraph(graph);
        }

        private GraphExample GenerateRandomGraph()
        {
            var graph = new GraphExample();

            var nodes = new List<IncludeGraphNode>();
            nodes.Add(new IncludeGraphNode(0, "path/root/main.cpp", "main.cpp"));
            nodes.Add(new IncludeGraphNode(1, "path/root/lib/lib.h", "lib.h"));
            nodes.Add(new IncludeGraphNode(2, "path/root/lib/io.h", "io.h"));
            nodes.Add(new IncludeGraphNode(3, "path/root/utils/str.h", "str.h"));
            nodes.Add(new IncludeGraphNode(4, "path/root/utils/bytes.h", "bytes.h"));
            nodes.Add(new IncludeGraphNode(5, "path/root/utils/array.h", "array.h"));
            nodes.Add(new IncludeGraphNode(6, "path/root/lib/io.h", "io.h"));

            graph.AddVertexRange(nodes);

            graph.AddEdge(new IncludeGraphEdge(nodes[0], nodes[1]));
            graph.AddEdge(new IncludeGraphEdge(nodes[1], nodes[2]));
            graph.AddEdge(new IncludeGraphEdge(nodes[0], nodes[6]));
            graph.AddEdge(new IncludeGraphEdge(nodes[2], nodes[3]));
            graph.AddEdge(new IncludeGraphEdge(nodes[3], nodes[4]));
            graph.AddEdge(new IncludeGraphEdge(nodes[3], nodes[5]));

            return graph;
        }

        private GraphExample GenerateCyclicGraph()
        {
            var graph = new GraphExample();

            var nodes = new List<IncludeGraphNode>();
            nodes.Add(new IncludeGraphNode(0, "path/root/main.cpp", "main.cpp"));
            nodes.Add(new IncludeGraphNode(1, "path/root/lib/lib.h", "lib.h"));
            nodes.Add(new IncludeGraphNode(2, "path/root/lib/io.h", "io.h"));
            nodes.Add(new IncludeGraphNode(3, "path/root/utils/str.h", "str.h"));
            nodes.Add(new IncludeGraphNode(4, "path/root/utils/bytes.h", "bytes.h"));
            nodes.Add(new IncludeGraphNode(5, "path/root/utils/array.h", "array.h"));

            graph.AddVertexRange(nodes);

            graph.AddEdge(new IncludeGraphEdge(nodes[0], nodes[1]));
            graph.AddEdge(new IncludeGraphEdge(nodes[1], nodes[2]));
            graph.AddEdge(new IncludeGraphEdge(nodes[0], nodes[2]));
            graph.AddEdge(new IncludeGraphEdge(nodes[2], nodes[3]));
            graph.AddEdge(new IncludeGraphEdge(nodes[3], nodes[4]));
            graph.AddEdge(new IncludeGraphEdge(nodes[3], nodes[5]));
            graph.AddEdge(new IncludeGraphEdge(nodes[4], nodes[2]));

            return graph;
        }

        private void LayoutGraph()
        {
            var logic = new GXLogicCoreExample();
            logic.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.EfficientSugiyama;
            logic.DefaultLayoutAlgorithmParams =
                               logic.AlgorithmFactory.CreateLayoutParameters(logic.DefaultLayoutAlgorithm);
            EfficientSugiyamaLayoutParameters parameters = (EfficientSugiyamaLayoutParameters)logic.DefaultLayoutAlgorithmParams;
            parameters.Direction = LayoutDirection.TopToBottom;
            parameters.EdgeRouting = SugiyamaEdgeRoutings.Traditional;

            logic.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logic.DefaultOverlapRemovalAlgorithmParams =
                              logic.AlgorithmFactory.CreateOverlapRemovalParameters(logic.DefaultOverlapRemovalAlgorithm);
            OverlapRemovalParameters overlap = (OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams;
            overlap.HorizontalGap = 150;
            overlap.VerticalGap = 50;

            logic.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;
            logic.AsyncAlgorithmCompute = false;

            LogicCore = logic;
        }
    }
}
