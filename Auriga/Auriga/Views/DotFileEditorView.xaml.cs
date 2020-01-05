using Bifrost.GraphElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
    /// <summary>
    /// Interaction logic for DotFileEditorView.xaml
    /// </summary>
    public partial class DotFileEditorView : UserControl, ICentralView
    {
        public DotFileEditorView()
        {
            InitializeComponent();
        }

        private void Button_LoadDot(object sender, RoutedEventArgs e)
        {
            if (Toolbar1.IsAncestorOf((DependencyObject)e.Source))
            {
                string content = new TextRange(TextEditor1.Document.ContentStart, TextEditor1.Document.ContentEnd).Text;
                GraphEditor1.LoadDotString(content);
            }
            if (Toolbar2.IsAncestorOf((DependencyObject)e.Source))
            {
                string content = new TextRange(TextEditor2.Document.ContentStart, TextEditor2.Document.ContentEnd).Text;
                GraphEditor2.LoadDotString(content);
            }
        }

        private void Button_SerializeDot(object sender, RoutedEventArgs e)
        {
            if (Toolbar1.IsAncestorOf((DependencyObject)e.Source))
            {
                string dot = GraphEditor1.SerializeGraphAsDotString();
                TextEditor1.Document.Blocks.Clear();
                TextEditor1.Document.Blocks.Add(new Paragraph(new Run(dot)));
            }
            if (Toolbar2.IsAncestorOf((DependencyObject)e.Source))
            {
                string dot = GraphEditor2.SerializeGraphAsDotString();
                TextEditor2.Document.Blocks.Clear();
                TextEditor2.Document.Blocks.Add(new Paragraph(new Run(dot)));
            }
        }
        
        private void Button_CopyToOther(object sender, RoutedEventArgs e)
        {
            if (Toolbar1.IsAncestorOf((DependencyObject)e.Source))
            {
                Graph g = GraphEditor1.CurrentGraph();
                GraphEditor2.LoadGraph(g);
            }
            if (Toolbar2.IsAncestorOf((DependencyObject)e.Source))
            {
                Graph g = GraphEditor2.CurrentGraph();
                GraphEditor1.LoadGraph(g);
            }
        }
    }
}
