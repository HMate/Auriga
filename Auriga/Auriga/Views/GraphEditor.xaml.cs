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

        private void GraphAreaZoom_CurrentViewChanged(object sender, Xceed.Wpf.Toolkit.Zoombox.ZoomboxViewChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Current view changed {e.NewValue}");
        }
    }
}
