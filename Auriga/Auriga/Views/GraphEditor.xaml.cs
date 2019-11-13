using Bifrost;
using Bifrost.GraphElements;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Auriga.Views
{
    /// <summary>
    /// Interaction logic for GraphEditor.xaml
    /// </summary>
    public partial class GraphEditor : UserControl, ICentralView
    {
        public GraphEditor()
        {
            InitializeComponent();
        }

        private void Button_SelectNodeCreationMode(object sender, RoutedEventArgs e)
        {
            GraphArea.CreationMode = GraphEditorArea.CreationModeType.Node;
        }

        private void Button_SelectArrowCreationMode(object sender, RoutedEventArgs e)
        {
            GraphArea.CreationMode = GraphEditorArea.CreationModeType.Arrow;
        }

        private void Button_ClickClearGraph(object sender, RoutedEventArgs e)
        {
            GraphArea.ClearGraph();
        }

        private void Button_ClickDeleteNode(object sender, RoutedEventArgs e)
        {
            GraphArea.DeleteSelectedNode();
        }

        private void Button_ClickOpenDotFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (true == dialog.ShowDialog())
            {
                Debug.WriteLine($"Opening: {dialog.FileName}");
                string content = File.ReadAllText(dialog.FileName);
                Graph gr = DotGraphConverter.ToGraph(DotLoader.LoadF(content));

                GraphArea.LoadGraph(gr);
            }
        }
    }
}
