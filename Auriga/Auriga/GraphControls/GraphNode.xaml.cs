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
        public Guid Id { get; }

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

        public GraphNode(string name) : this(Guid.NewGuid(), name) { }

        public GraphNode(Guid id, string name)
        {
            InitializeComponent();
            Id = id;
            NodeName = name;
        }

        public Point GetBottomArrowAttachPoint()
        {
            var arrowPos = new Point((double)GetValue(Canvas.LeftProperty), (double)GetValue(Canvas.TopProperty));
            arrowPos.Offset(ActualWidth / 2d, ActualHeight);
            return arrowPos;
        }

        internal Point GetTopArrowAttachPoint()
        {
            var arrowPos = new Point((double)GetValue(Canvas.LeftProperty), (double)GetValue(Canvas.TopProperty));
            arrowPos.Offset(ActualWidth / 2d, 0);
            return arrowPos;
        }
    }
}
