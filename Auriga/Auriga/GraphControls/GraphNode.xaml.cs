using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Auriga.GraphControls
{
    /// <summary>
    /// Interaction logic for GraphNode.xaml
    /// </summary>
    public partial class GraphNode : UserControl
    {
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

        public GraphNode() : this("Default")
        {

        }

        public GraphNode(string name)
        {
            InitializeComponent();
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
