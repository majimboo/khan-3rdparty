using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace KhanEngine.CustomControl
{
    /// <summary>
    /// Interaction logic for Tab1.xaml
    /// </summary>
    public partial class Tab1 : UserControl
    {
        public Tab1()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}