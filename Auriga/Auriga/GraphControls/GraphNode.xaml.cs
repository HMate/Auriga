using System;
using System.Windows;
using System.Windows.Controls;

namespace Auriga.GraphControls
{
    /// <summary>
    /// Interaction logic for GraphNode.xaml
    /// </summary>
    public partial class GraphNode : UserControl
    {
        public string Id { get; }

        public static readonly DependencyProperty NodeNameProperty =
            DependencyProperty.Register("NodeName", typeof(string), typeof(GraphNode));

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(GraphNode));

        public string NodeName
        {
            get { return (string)GetValue(NodeNameProperty); }
            set { SetValue(NodeNameProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public GraphNode() : this("Default") { }

        public GraphNode(string name) : this(Guid.NewGuid().ToString(), name) { }

        public GraphNode(string id, string name)
        {
            InitializeComponent();
            Id = id;
            NodeName = name;
        }

        public Point GetLeftTop()
        {
            return new Point((double)GetValue(Canvas.LeftProperty), (double)GetValue(Canvas.TopProperty));
        }

        public Point GetBottomArrowAttachPoint()
        {
            var pos = GetLeftTop();
            Size offset = new Size(ActualWidth / 2d, ActualHeight);
            if(offset.Width == 0d || offset.Height == 0d)
            {
                offset = new Size(DesiredSize.Width / 2d, DesiredSize.Height);
            }
            pos.Offset(offset.Width, offset.Height);
            return pos;
        }

        internal Point GetTopArrowAttachPoint()
        {
            var pos = GetLeftTop();
            Size offset = new Size(ActualWidth / 2d, 0d);
            if (offset.Width == 0d)
            {
                offset = new Size(DesiredSize.Width / 2d, 0);
            }
            pos.Offset(offset.Width, offset.Height);
            return pos;
        }
    }
}
