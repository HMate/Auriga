﻿using Auriga.Common;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Auriga.GraphControls
{
    public class GraphArrow : Shape
    {
        static GraphArrow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphArrow), new FrameworkPropertyMetadata(typeof(GraphArrow)));
        }

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(GraphArrow),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(GraphArrow),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(GraphArrow),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(GraphArrow),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(GraphArrow),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(GraphArrow),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [TypeConverter(typeof(LengthConverter))]
        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadHeight
        {
            get { return (double)GetValue(HeadHeightProperty); }
            set { SetValue(HeadHeightProperty, value); }
        }

        [TypeConverter(typeof(LengthConverter))]
        public double HeadWidth
        {
            get { return (double)GetValue(HeadWidthProperty); }
            set { SetValue(HeadWidthProperty, value); }
        }

        public GraphNode? StartNode { private set; get; }
        public GraphNode? EndNode { private set; get; }

        public void SetStartNode(GraphNode node)
        {
            if (StartNode != node)
            {
                if (StartNode != null)
                {
                    RemoveNodePosChanged(StartNode, UpdateStartPositionEventHandler);
                }
                StartNode = node;
                UpdateStartPositionEventHandler(null, null);
                RegisterNodePosChanged(node, UpdateStartPositionEventHandler);
            }
        }

        public void SetEndNode(GraphNode node)
        {
            if (EndNode != node)
            {
                if (EndNode != null)
                {
                    RemoveNodePosChanged(EndNode, UpdateEndPositionEventHandler);
                }

                EndNode = node;
                UpdateEndPositionEventHandler(null, null);
                RegisterNodePosChanged(node, UpdateEndPositionEventHandler);
            }
        }

        public void SetEndPoint(Point end)
        {
            (X2, Y2) = end;
        }

        private void UpdateStartPositionEventHandler(object? sender, EventArgs? e)
        {
            if (StartNode != null)
            {
                Point arrowPos = StartNode!.GetBottomArrowAttachPoint();
                (X1, Y1) = arrowPos;
            }
        }

        private void UpdateEndPositionEventHandler(object? sender, EventArgs? e)
        {
            if (EndNode != null)
            {
                Point arrowPos = EndNode!.GetTopArrowAttachPoint();
                (X2, Y2) = arrowPos;
            }
        }

        // Source: https://www.codeproject.com/Articles/23116/WPF-Arrow-and-Custom-Shapes
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry
                {
                    FillRule = FillRule.EvenOdd
                };

                using (StreamGeometryContext context = geometry.Open())
                {
                    InternalDrawArrowGeometry(context);
                }

                // Freeze the geometry for performance benefits
                geometry.Freeze();
                return geometry;
            }
        }

        public void UpdateGeometry()
        {
            this.InvalidateVisual();
        }

        private void InternalDrawArrowGeometry(StreamGeometryContext context)
        {
            double theta = Math.Atan2(Y1 - Y2, X1 - X2);
            double sint = Math.Sin(theta);
            double cost = Math.Cos(theta);

            Point pt1 = new Point(X1, Y1);
            Point pt2 = new Point(X2, Y2);

            Point pt3 = new Point(
                X2 + (HeadHeight * cost - HeadWidth * sint),
                Y2 + (HeadHeight * sint + HeadWidth * cost));

            Point pt4 = new Point(
                X2 + (HeadHeight * cost + HeadWidth * sint),
                Y2 - (HeadWidth * cost - HeadHeight * sint));

            context.BeginFigure(pt1, true, false);
            context.LineTo(pt2, true, true);
            context.LineTo(pt3, true, true);
            context.LineTo(pt2, true, true);
            context.LineTo(pt4, true, true);
        }

        private void RegisterNodePosChanged(GraphNode node, EventHandler handler)
        {
            GetGraphNodePropDescriptor(Canvas.LeftProperty).AddValueChanged(node, handler);
            GetGraphNodePropDescriptor(Canvas.TopProperty).AddValueChanged(node, handler);
            GetGraphNodePropDescriptor(ActualWidthProperty).AddValueChanged(node, handler);
            GetGraphNodePropDescriptor(ActualHeightProperty).AddValueChanged(node, handler);
        }

        private void RemoveNodePosChanged(GraphNode node, EventHandler handler)
        {
            GetGraphNodePropDescriptor(Canvas.LeftProperty).RemoveValueChanged(node, handler);
            GetGraphNodePropDescriptor(Canvas.TopProperty).RemoveValueChanged(node, handler);
            GetGraphNodePropDescriptor(ActualWidthProperty).RemoveValueChanged(node, handler);
            GetGraphNodePropDescriptor(ActualHeightProperty).RemoveValueChanged(node, handler);
        }

        private DependencyPropertyDescriptor GetGraphNodePropDescriptor(DependencyProperty dp)
        {
            return DependencyPropertyDescriptor.FromProperty(dp, typeof(GraphNode));
        }
    }
}
