using KhanEngine.Form;
using KhanEngine.PageFrame;
using Lang;
using Net.Connect;
using Net.Model;
using Net.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace KhanEngine.ViewModel
{
    public class LoginVM
    {
        private static Language Language = new Language();
        public Thread ClientScan { get; set; }

        private LoginModel LoginM { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand PaymentCommand { get; }
        private GameModel GameM { get; set; }

        public LoginVM(LoginModel _loginModel, GameModel _GameM)
        {
            GameM = _GameM;
            LoginM = _loginModel;
            CPayment();
            PayClient();
            LoginCommand = new ReplayCommand(Login);
            ReturnCommand = new ReplayCommand(Return);
            PaymentCommand = new ReplayCommand(Payment);
        }

        private void Payment()
        {
            try
            {
                int client = LoginM.IClients.ID, payment = LoginM.IPayment.ID;
                MessageBoxResult _returnReg = MessageBox.Show(Language.Set(Language.Payment, new string[3] { LoginM.IPayment.Name, client.ToString(), LoginM.PayCostAll }), Language.Notification, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (_returnReg == MessageBoxResult.No)
                {
                    return;
                }
                string _data = ConnectData.Payment(client, payment);
                if (_data == "false")
                {
                    MessageBox.Show(Language.ErrorConnect, Language.Notification, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                dynamic data = JsonConvert.DeserializeObject(_data);
                string Title = data.title, Text = data.text, Msg = data.msg; if (Msg == "error")
                {
                    MessageBox.Show(Text, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                GetDataInfo();
                MessageBox.Show(Text, Title);
            }
            catch
            {
                MessageBox.Show(Language.ErrorConnect, Language.Notification, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CPayment()
        {
            try
            {
                bool EventMsg = false;
                LoginM.MsgEvent = Language.Event;
                LoginM.TMsgEvent = "\n";
                ObservableCollection<ComboxP> _NPaymemt = new ObservableCollection<ComboxP>();
                string Paycost = ConnectData.CostPay();
                dynamic ItemPay = JsonConvert.DeserializeObject(Paycost);
                foreach (var item in ItemPay)
                {
                    if (item.EventPay != "")
                    {
                        EventMsg = true;
                        LoginM.TMsgEvent = LoginM.TMsgEvent + item.EventPay + "\n";
                        LoginM.MsgEvent = LoginM.MsgEvent + " : " + item.Name;
                    }
                    _NPaymemt.Add(new ComboxP { Name = item.Name, ID = item.Type, Cost = item.Cost, Msg = item.EventPay });
                }
                if (EventMsg == false)
                {
                    LoginM.MsgEvent = "";
                    LoginM.TMsgEvent = "";
                }
                LoginM.SPaymemts = _NPaymemt;
            }
            catch
            {
                ObservableCollection<ComboxP> _NPaymemt = new ObservableCollection<ComboxP>
                {
                    new ComboxP { Name = Language.Maintenance, ID = 0, Cost = 0, Msg = Language.Maintenance }
                };
                LoginM.SPaymemts = _NPaymemt;
            }
        }

        private void PayClient()
        {
            ObservableCollection<ComboxP> _NClient = new ObservableCollection<ComboxP>();
            for (int i = 1; i < 11; i++)
            {
                _NClient.Add(new ComboxP { Name = i.ToString() + " " + Language.Clients, ID = i, Cost = i });
            }
            LoginM.SClients = _NClient;
        }

        private bool IsNullFill()
        {
            string ID = LoginM.Id;
            string Password = LoginM.Password;
            if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Password))
                return true;
            return false;
        }

        private bool IsNullData(string ID, string Password)
        {
            if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Password))
                return true;
            return false;
        }

        private void Return()
        {
            Window Auto = Window.GetWindow(App.Current.MainWindow);
            (Auto as frmMain).MainLogin.Content = new LoginPage();
        }

        private void GetDataInfo()
        {
            string GetDataInfoResult = ConnectData.GetDataInfo();
            dynamic data = JsonConvert.DeserializeObject(GetDataInfoResult);
            LoginM.Balance = (Convert.ToInt32(data.credits)).ToString("#,###") + " xu";
            LoginM.RegClient = Convert.ToInt32(data.account);

            string OnlineLimit = ConnectData.Limit();
            try
            {
                LoginM.Max_limit_crep = Convert.ToInt32(OnlineLimit);
            }
            catch { LoginM.Max_limit_crep = 10; }
        }

        private void Login()
        {
            if (IsNullFill())
            {
                MessageBox.Show(Language.NullFill, Language.Notification);
                return;
            }
            ConnectData.RandomToken();
            string ID = LoginM.Id; ConnectData.Username = ID;
            string Password = LoginM.Password; ConnectData.Password = Password;
            string loginResult = ConnectData.Login();
            switch (loginResult)
            {
                case "NullFill":
                    MessageBox.Show(Language.NullFill, Language.Notification);
                    return;

                case "false":
                    MessageBox.Show(Language.ErrorConnect, Language.Notification);
                    return;

                case "Exception":
                    MessageBox.Show(Language.ErrorAccount, Language.Notification);
                    return;

                case "wrong":
                    LoadWor.Read = true;
                    MessageBox.Show(Language.Wrong, Language.Notification, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;

                case "Shell":
                    LoadWor.Read = true;
                    MessageBox.Show(Language.Wrong, Language.ShellNew, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
            dynamic data = JsonConvert.DeserializeObject(loginResult);
            if (data.banned == "1")
            {
                MessageBox.Show(Language.BannedAccount, Language.Notification, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoginM.Type = Language.TypeMember[data.is_admin];
            LoginM.Email = data.email;
            LoginM.Gender = data.gender;
            LoginM.RegDate = data.register_date;
            LoginM.Server = ConnectData.ServerName;

            dynamic Online = ConnectData.Online();

            if (Online.Online == "true")
            {
                MessageBoxResult _returnReg = MessageBox.Show(Language.Set(Language.OutNow, ID), Language.Notification, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (_returnReg == MessageBoxResult.No)
                {
                    return;
                }
                if (_returnReg == MessageBoxResult.Yes)
                {
                    ConnectData.Logout(true);
                    return;
                }
            }
            else if (Online.Online == "Error")
            {
                MessageBox.Show(Language.ErrorConnect, Language.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GetDataInfo();
            string InOnline = ConnectData.InInsertOnline();
            if (InOnline == "false" || InOnline == "Error")
            {
                MessageBox.Show(Language.ErrorConnect, Language.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ConnectData.LogoutKey = true;
            if (Mill.FilePath())
            {
                Mill.CreateXaml(LoginM.Id);
            }
            else
            {
                Mill.SetXaml(LoginM.Id);
            }

            Start();
        }

        private void Start()
        {
            LoginM.CheckOnline = new Thread(delegate () { Func.GetOnline(GameM, LoginM); })
            {
                IsBackground = true
            };
            LoginM.CheckOnline.Start();

            ClientScan = new Thread(delegate () { Func.GetClient(GameM, LoginM); })
            {
                IsBackground = true
            };
            ClientScan.Start();
            Window Auto = Window.GetWindow(App.Current.MainWindow);
            frmAuto A = new frmAuto();
            Auto.Close();
            A.Show();
        }
    }
}