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
            var element = new Rectangle();
            element.Width = 100;
            element.Height = 70;
            element.Fill = new SolidColorBrush(Colors.Yellow);

            Canvas.SetLeft(element, 30);
            Canvas.SetTop(element, 60);

            Children.Add(element);
        }
    }
}
