using Auriga.GraphControls;
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

        public GraphNode AddNode(Point pos)
        {
            var element = new GraphNode("Default Name");

            // Width, Height Are Nan here, ActualWidth/H are 0, so we force compute them
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            SetLeft(element, pos.X - (element.DesiredSize.Width / 2d));
            SetTop(element, pos.Y - (element.DesiredSize.Height / 2d));

            Children.Add(element);
            return element;
        }

        public GraphArrow AddArrow(Point startPos)
        {
            var element = new GraphArrow
            {
                X1 = 0,
                Y1 = 0,
                HeadHeight = 10,
                HeadWidth = 5
            };
            // TODO: Better to just make arrow position to depend on box positonm instead of setting it manually.
            SetLeft(element, startPos.X);
            SetTop(element, startPos.Y);

            Children.Add(element);
            return element;
        }

        private GraphNode movingNode;
        private Point moveStartNodePosOffset;

        private GraphArrow arrowUnderCreation;

        public void GraphEditorArea_MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                UnselectAllNodes();
                if (e.Source is GraphNode node)
                {
                    if (CreationMode == CreationModeType.Arrow)
                    {
                        var arrowPos = new Point(GetLeft(node), GetTop(node));
                        arrowPos.Offset(node.ActualWidth / 2d, node.ActualHeight);
                        arrowUnderCreation = AddArrow(arrowPos);
                    }
                    else
                    {
                        movingNode = node;
                        movingNode.IsSelected = true;
                        moveStartNodePosOffset = e.GetPosition(node);
                    }
                }
                else if(CreationMode == CreationModeType.Node)
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
            if(movingNode != null)
            {
                var curPos = e.GetPosition(this);
                SetLeft(movingNode, curPos.X - moveStartNodePosOffset.X);
                SetTop(movingNode, curPos.Y - moveStartNodePosOffset.Y);
            }
            if(arrowUnderCreation != null)
            {
                if (e.Source is GraphNode node)
                {
                    var nodePos = new Point(GetLeft(node), GetTop(node));
                    nodePos.Offset(node.ActualWidth / 2d, 0);
                    var arrowPos = new Point(GetLeft(arrowUnderCreation), GetTop(arrowUnderCreation));
                    arrowUnderCreation.X2 = nodePos.X - arrowPos.X;
                    arrowUnderCreation.Y2 = nodePos.Y - arrowPos.Y;
                }
                else
                {
                    var curPos = e.GetPosition(this);
                    var arrowPos = new Point(GetLeft(arrowUnderCreation), GetTop(arrowUnderCreation));
                    arrowUnderCreation.X2 = curPos.X - arrowPos.X;
                    arrowUnderCreation.Y2 = curPos.Y - arrowPos.Y;
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
            foreach(var child in Children)
            {
                if(child is GraphNode node)
                {
                    node.IsSelected = false;
                }
            }
        }
    }
}
