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
            Children.Clear();
        }

        public void AddNode(double x, double y)
        {
            var element = new Rectangle();
            element.Width = 100;
            element.Height = 70;
            element.Style = (Style)(Application.Current.TryFindResource("defaultNodeStyle"));

            Canvas.SetLeft(element, x - element.Width/2);
            Canvas.SetTop(element, y - element.Height/2);

            Children.Add(element);
        }

        public void GraphEditorArea_MouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                Point pos = e.GetPosition(this);
                AddNode(pos.X, pos.Y);
            }
        }
    }
}
