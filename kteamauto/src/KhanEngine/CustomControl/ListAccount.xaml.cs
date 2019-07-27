using KhanEngine.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KhanEngine.CustomControl
{
    /// <summary>
    /// Interaction logic for ListAccount.xaml
    /// </summary>
    public partial class ListAccount : UserControl
    {
        public ListAccount()
        {
            InitializeComponent();
        }

        private void IsPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((ListViewItem)sender).IsSelected = true;
        }

        private void UseDefaultFoldersCB_Click(object sender, RoutedEventArgs e)
        {
            GameVM.InfoVi.IsEnabled = true;
        }
    }
}