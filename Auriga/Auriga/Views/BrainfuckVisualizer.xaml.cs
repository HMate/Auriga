using System;
using System.Collections.Generic;
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
    /// Interaction logic for BrainfuckVisualizer.xaml
    /// </summary>
    public partial class BrainfuckVisualizer : UserControl, ICentralView
    {
        public BrainfuckVisualizer()
        {
            InitializeComponent();
        }

        private void Button_LoadCode(object sender, RoutedEventArgs e)
        {

        }
    }
}
