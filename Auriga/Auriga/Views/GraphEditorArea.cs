using Auriga.GraphControls;
using Bifrost.GraphElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Auriga.Views
{
    public class GraphEditorArea : Canvas
    {
        static GraphEditorArea()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphEditorArea), new FrameworkPropertyMetadata(typeof(GraphEditorArea)));
        }

        public GraphEditorArea()
        {
            MouseDown += GraphEditorArea_MouseDownEventHandler;
            MouseUp += GraphEditorArea_MouseUpEventHandler;
            MouseLeave += GraphEditorArea_MouseLeaveEventHandler;
            MouseMove += GraphEditorArea_MouseMoveEventHandler;

            Children.Clear();
        }

        public enum CreationModeType
        {
            Node,
            Arrow
        }

        public static readonly DependencyProperty CreationModeProperty =
            DependencyProperty.Register("CreationMode", typeof(CreationModeType), typeof(GraphEditorArea));

        public CreationModeType CreationMode
        {
            get { return (CreationModeType)GetValue(CreationModeProperty); }
            set { SetValue(CreationModeProperty, value); }
        }

        public void ClearGraph()
        {
            Children.Clear();
        }

        /// <summary>
        /// Creates a <see cref="Graph"/> object from the currently edited graph.
        /// </summary>
        public Graph ToGraph()
        {
            Graph graph = new Graph();
            foreach (GraphNode node in GetChildren<GraphNode>())
            {
                Vector offset = new Vector(node.ActualWidth / 2.0, node.ActualHeight / 2.0);
                Point pos = new Point(GetLeft(node), GetTop(node)) + offset;
                graph.AddNode(new Node(node.Id, node.NodeName, pos));
            }

            foreach (GraphArrow arrow in GetChildren<GraphArrow>())
            {
                if (arrow.StartNode == null || arrow.EndNode == null)
                    continue;
                graph.AddEdge(new Edge(arrow.StartNode.Id, arrow.EndNode.Id));
            }
            return graph;
        }

        public void DeleteSelectedNode()
        {
            List<UIElement> toRemove = new List<UIElement>();
            foreach (GraphNode node in GetChildren<GraphNode>())
            {
                if (node.IsSelected)
                {
                    toRemove.Add(node);
                    foreach (GraphArrow arrow in GetChildren<GraphArrow>())
                    {
                        if (arrow.StartNode == node || arrow.EndNode == node)
                        {
                            toRemove.Add(arrow);
                        }
                    }
                }
            }
            toRemove.ForEach(e => Children.Remove(e));
        }

        private IEnumerable<T> GetChildren<T>()
        {
            foreach (UIElement? item in Children)
            {
                if (item is T t)
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        /// Loads the given graph to the center of the viewport.
        /// </summary>
        /// <param name="gr"></param>
        public void LoadGraph(Graph gr)
        {
            var nodes = new Dictionary<string, GraphNode>();
            foreach (var node in gr.Nodes)
            {
                var nodeRect = AddNode(node.Id, node.NodeName, node.Position);
                nodes.Add(node.Id, nodeRect);
            }

            foreach (var edge in gr.Edges)
            {
                AddArrow(nodes[edge.StartId], nodes[edge.EndId]);
            }
        }

        public GraphNode AddNode(Point pos)
        {
            return AddNode(Guid.NewGuid().ToString(), "Default Name", pos);
        }

        public GraphNode AddNode(string id, string name, Point pos)
        {
            var element = new GraphNode(id, name);

            // Width, Height Are Nan here, ActualWidth/H are 0, so we force compute them
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            SetLeft(element, pos.X - (element.DesiredSize.Width / 2d));
            SetTop(element, pos.Y - (element.DesiredSize.Height / 2d));

            Children.Add(element);
            return element;
        }

        public GraphArrow AddArrow(GraphNode node, Point curPos)
        {
            var element = new GraphArrow
            {
                HeadHeight = 10,
                HeadWidth = 5
            };
            element.SetStartNode(node);
            // Set arrow position manually until we dont have ending box.
            element.SetEndPoint(curPos);

            Children.Add(element);
            return element;
        }

        public GraphArrow AddArrow(GraphNode start, GraphNode end)
        {
            var element = new GraphArrow
            {
                HeadHeight = 10,
                HeadWidth = 5
            };
            element.SetStartNode(start);
            // Set arrow position manually until we dont have ending box.
            element.SetEndNode(end);

            Children.Add(element);
            return element;
        }

        private GraphNode? movingNode;
        private Point moveStartNodePosOffset;

        private GraphArrow? arrowUnderCreation;

        public void GraphEditorArea_MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine($"Cliekced at {e.GetPosition(this)}");
            if (e.ChangedButton == MouseButton.Left)
            {
                UnselectAllNodes();
                if (e.Source is GraphNode node)
                {
                    if (CreationMode == CreationModeType.Arrow)
                    {
                        arrowUnderCreation = AddArrow(node, e.GetPosition(this));
                    }
                    else
                    {
                        movingNode = node;
                        movingNode.IsSelected = true;
                        moveStartNodePosOffset = e.GetPosition(node);
                    }
                }
                else if (CreationMode == CreationModeType.Node)
                {
                    var newNode = AddNode(e.GetPosition(this));
                    newNode.IsSelected = true;
                }
            }
        }

        public void GraphEditorArea_MouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                StopMovingNode();
                if (arrowUnderCreation != null && !(e.Source is GraphNode))
                {
                    Children.Remove(arrowUnderCreation);
                }
                arrowUnderCreation = null;
            }
        }

        public void GraphEditorArea_MouseLeaveEventHandler(object sender, MouseEventArgs e)
        {
            StopMovingNode();
            if (arrowUnderCreation != null)
            {
                Children.Remove(arrowUnderCreation);
            }
            arrowUnderCreation = null;
        }

        public void GraphEditorArea_MouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if (movingNode != null)
            {
                var curPos = e.GetPosition(this);
                SetLeft(movingNode, curPos.X - moveStartNodePosOffset.X);
                SetTop(movingNode, curPos.Y - moveStartNodePosOffset.Y);
            }
            if (arrowUnderCreation != null)
            {
                if (e.Source is GraphNode node)
                {
                    arrowUnderCreation.SetEndNode(node);
                }
                else
                {
                    var curPos = e.GetPosition(this);
                    arrowUnderCreation.SetEndPoint(curPos);
                }
            }
        }

        private void StopMovingNode()
        {
            if (movingNode != null)
            {
                movingNode = null;
            }
        }

        private void UnselectAllNodes()
        {
            foreach (var child in Children)
            {
                if (child is GraphNode node)
                {
                    node.IsSelected = false;
                }
            }
        }

        private void MoveChild(GraphNode node, Point offset)
        {
            var curPos = node.GetLeftTop();
            SetLeft(node, curPos.X + offset.X);
            SetTop(node, curPos.Y + offset.Y);
        }
    }
}
