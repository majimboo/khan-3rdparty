using KhanEngine.View;
using Net.Connect;
using Net.Model;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace KhanEngine.PageFrame
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private MainView DT;

        public LoginPage()
        {
            InitializeComponent();
            DT = (MainView)DataContext;
            DT.LoginM.Id = Mill.GetXaml();
            if (DT.LoginM.Id == "")
            {
                Username.Focus();
            }
            else
            {
                Passwords.Focus();
            }
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                dynamic Data = ConnectData.Website();
                Process.Start(Data.Register.ToString());
            }
            catch
            {
                Process.Start("http://home.kteamauto.com/");
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Passwords.Focus();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (LoadWor.Read)
                {
                    LoadWor.Read = false;
                    Passwords.SelectAll();
                }
            }
        }
    }
}