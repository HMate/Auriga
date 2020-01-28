using Bifrost.Dot;
using Bifrost.GraphElements;
using Bifrost.GraphViz;
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
                LoadDotString(content);
            }
        }

        private void Button_ClickSaveDotFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (true == dialog.ShowDialog())
            {
                Debug.WriteLine($"Saving: {dialog.FileName}");
            }
        }

        public void LoadDotString(string dotContent)
        {
            DotGraph dg = DotLoader.Load(dotContent);
            if (!dg.GraphAttributes.ContainsKey("bb"))
            {
                dg = GraphVizWrapper.LayoutNodes(dg);
            }
            Graph gr = DotGraphConverter.ToGraph(dg);
            adjustNodesToCenterOfViewport(gr, GraphAreaZoom.Viewport);
            LoadGraph(gr);
        }

        private void adjustNodesToCenterOfViewport(Graph gr, Rect viewport)
        {
            Vector viewportCenterOffset = new Vector(viewport.Left + viewport.Width / 2d, viewport.Top + viewport.Height / 2d);
            foreach (var node in gr.Nodes)
            {
                node.Position += (Vector)node.Position + viewportCenterOffset;
            }
        }

        public string SerializeGraphAsDotString()
        {
            DotGraph gr = DotGraphConverter.ToDot(GraphArea.ToGraph());
            string dot = DotFileGenerator.Serialize(gr);
            return dot;
        }

        public Graph CurrentGraph() => GraphArea.ToGraph();

        public void LoadGraph(Graph gr)
        {
            GraphArea.ClearGraph();
            GraphArea.LoadGraph(gr);
        }
    }
}
