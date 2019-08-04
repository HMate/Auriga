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
    public class GraphAreaControl : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {
        static GraphAreaControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphAreaControl), new FrameworkPropertyMetadata(typeof(GraphAreaControl)));
        }

        public GraphAreaControl()
        {
            var graph = GenerateRandomGraph();
            LayoutGraph(graph);
        }

        private GraphExample GenerateRandomGraph()
        {
            Random Rand = new Random();

            //Create data graph object
            var graph = new GraphExample();

            //Create and add vertices using some DataSource for ID's
            foreach (var item in GenerateItems(100))
                graph.AddVertex(new DataVertex() { ID = item.ID, Text = item.Text });

            var vlist = graph.Vertices.ToList();
            //Generate random edges for the vertices
            foreach (var item in vlist)
            {
                if (Rand.Next(0, 50) > 25)
                    continue;

                var vertex2 = vlist[Rand.Next(0, graph.VertexCount - 1)];
                graph.AddEdge(new DataEdge(item, vertex2, Rand.Next(1, 50))
                {
                    Text = $"{item} -> {vertex2}"
                });
            }

            return graph;
        }

        private void LayoutGraph(GraphExample graph)
        {
            var logic = new GXLogicCoreExample();
            logic.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            logic.DefaultLayoutAlgorithmParams =
                               logic.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            ((KKLayoutParameters)logic.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            logic.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logic.DefaultOverlapRemovalAlgorithmParams =
                              logic.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);
            ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 50;
            ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 50;

            logic.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;
            logic.AsyncAlgorithmCompute = false;

            LogicCore = logic;

            GenerateGraph(graph);
        }


        private IEnumerable<ExampleItem> GenerateItems(int count)
        {
            var items = new List<ExampleItem>();
            for (int i = 0; i < count; i++)
            {
                items.Add(new ExampleItem { ID = i, Text = $"Item_{i}" });
            }
            return items;
        }

        public struct ExampleItem
        {
            public long ID;
            public string Text;
        }
    }
}
