﻿using System;
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

        public GraphNode AddNode(Point pos)
        {
            var element = new GraphNode("Default Name");

            // TODO: Width,Height Are Nan here, figure out how could we compute them
            SetLeft(element, pos.X - 50);
            SetTop(element, pos.Y - 10);

            Children.Add(element);
            return element;
        }

        private GraphNode movingNode;
        private Point moveStartNodePosOffset;

        public void GraphEditorArea_MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                UnselectAllNodes();
                if (e.Source is GraphNode node)
                {
                    movingNode = node;
                    movingNode.IsSelected = true;
                    moveStartNodePosOffset = e.GetPosition(node);
                }
                else
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
            }
        }

        public void GraphEditorArea_MouseLeaveEventHandler(object sender, MouseEventArgs e)
        {
            StopMovingNode();
        }

        public void GraphEditorArea_MouseMoveEventHandler(object sender, MouseEventArgs e)
        {
            if(movingNode != null)
            {
                var curPos = e.GetPosition(this);
                SetLeft(movingNode, curPos.X - moveStartNodePosOffset.X);
                SetTop(movingNode, curPos.Y - moveStartNodePosOffset.Y);
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
