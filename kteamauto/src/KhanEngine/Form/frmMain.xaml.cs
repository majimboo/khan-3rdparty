using KhanEngine.View;
using Lang;
using Net.Connect;
using Net.Model;
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
using System.Windows.Shapes;

namespace KhanEngine.Form
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        private MainView DT;

        private new Language Language = new Language();

        public frmMain()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            InitializeComponent();
            DT = (MainView)DataContext;
            if (DT.InfoV.IsCheckOpen)
                TokenStart();
        }

        private void TokenStart()
        {
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                if (args[1] == "Version")
                {
                    MessageBox.Show(string.Format(
                        "Client = {0}\n" +
                        "Data = {1}\n" +
                        "Language = {2}\n" +
                        "Net = {3}\n",
                        "3.0.3.9",
                        "1.0.3.5",
                        "1.0.1.0",
                        "3.0.3.9"
                        ));
                    App.Current.Shutdown();
                }
                else if (args[1] != HardwareAnalyzer.GetMD5(HardwareAnalyzer.GetMacAddress()))
                {
                    MessageBox.Show(Language.OpenClients, Language.Error, MessageBoxButton.OK, MessageBoxImage.Stop);
                    App.Current.Shutdown();
                }
            }
            catch
            {
                MessageBox.Show(Language.OpenClients, Language.Error, MessageBoxButton.OK, MessageBoxImage.Stop);
                App.Current.Shutdown();
            }
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            ConnectData.Logout();
        }

        private void FormMove(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); } catch { }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}