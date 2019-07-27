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
using System.Windows.Shapes;

namespace KhanEngine.Form
{
    /// <summary>
    /// Interaction logic for frmAuto.xaml
    /// </summary>
    public partial class frmAuto : Window
    {
        public frmAuto()
        {
            InitializeComponent();
        }

        private void FormMove(object sender, MouseButtonEventArgs e)
        {
            try { this.DragMove(); } catch { }
        }

        private void Minimized(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}