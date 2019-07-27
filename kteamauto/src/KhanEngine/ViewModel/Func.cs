using KhanEngine;
using KhanEngine.ViewModel;
using Lang;
using Net.Connect;
using Net.ControlModel;
using Net.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Net.ViewModel
{
    public static class Func
    {
        private static Language Language = new Language();

        //[DllImport("user32.dll")]
        //public static extern bool GetAsyncKeyState(Keys vKey);

        public static void GetOnline(GameModel GameM, LoginModel LoginM)
        {
            dynamic JOnline = ConnectData.Online();
            int ErrorCount = 0, UncheckClient = 0;
            try
            {
                while (LoginM.Login)
                {
                    try
                    {
                        JOnline = ConnectData.Online();
                        try
                        {
                            LoginM.Max_limit_crep = JOnline.Limit;
                        }
                        catch { LoginM.Max_limit_crep = 100; }
                        if (JOnline.Online == "true")
                        {
                            if (JOnline.token == ConnectData.Token)
                            {
                                ErrorCount = 0;
                                LoginM.RegClient = Convert.ToInt32(JOnline.Client);
                            }
                            else
                            {
                                ErrorCount = 100;
                            }
                        }

                        if (JOnline.Online == "false")
                        {
                            ErrorCount = 100;
                        }
                        else if (JOnline.Online == "off")
                        {
                            ErrorCount = 100;
                            CleanAll(GameM, LoginM);
                            LoginM.Login = false;
                            MessageBox.Show(Language.Maintenances, Language.Maintenance, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else if (JOnline.Online == "Error")
                        {
                            ErrorCount++;
                        }
                        else if (LoginM.RegClient < GameM.Clients)
                        {
                            UncheckClient = GameM.Clients - LoginM.RegClient;
                            foreach (AccountModel A in GameM.ListAccount)
                            {
                                if (UncheckClient > 0 && A.IsAuto)
                                {
                                    A.IsAuto = false;
                                    UncheckClient--;
                                    GameM.Clients--;
                                }
                            }
                        }
                    }
                    catch { ErrorCount++; }

                    if (ErrorCount > 4)
                    {
                        ConnectData.LogoutKey = false;
                        CleanAll(GameM, LoginM);
                        LoginM.Login = false;
                        LoginM.Login = false;
                        MessageBox.Show(Language.KichAccount, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Thread.Sleep(55000);
                }
            }
            catch (ThreadAbortException)
            {
                CleanAll(GameM, LoginM);
                LoginM.Login = false;
                ConnectData.LogoutKey = false;
                MessageBox.Show(Language.KichAccount, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void GetClient(GameModel GameM, LoginModel LoginM)
        {
            while (LoginM.Login)
            {
                try
                {
                    bool found = false;
                    WindowsFind.FindWindows(Mill.ProcessesByName);
                    foreach (NameProcess p in WindowsFind.Get)
                    {
                        if (ProcessExists(p.Name.Id))
                        {
                            found = true;
                            if (GameM.ListAccount.Count <= 0 || GameM.ListAccount == null) { GameM.ListAccount = new System.Collections.ObjectModel.ObservableCollection<AccountModel>(); }
                            foreach (AccountModel A in GameM.ListAccount)
                            {
                                if (p.Name.Id == A.Pid)
                                {
                                    found = false;
                                    break;
                                }
                            }
                            if (found)
                            {
                                AccountModel AM = new AccountModel
                                {
                                    Pid = p.Name.Id
                                };
                                if (AM.Func == null) { AM.Func = new Memory.Function(); }
                                AM.Func.Login = LoginM.Login;
                                AM.Func.KeyStart = LoginM.KeyStart;
                                AM.Func.SetFunction(p.Name.Id);
                                MAPN.SetWindowText(p.Name.MainWindowHandle, "Khan Online1");
                                AM.AddClassChar();
                                App.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    GameM.ListAccount.Add(AM);
                                });

                                break;
                            }
                        }
                    }
                    // xoa danh sach
                    foreach (AccountModel A in GameM.ListAccount)
                    {
                        if (!ProcessExists(A.Pid))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                GameM.ListAccount.Remove(A);
                            });
                            if (GameM.ListAccount.Count == 0) { GameVM.InfoVi.IsEnabled = false; }
                            break;
                        }
                    }
                    GetName(GameM);
                }
                catch (ThreadAbortException) { break; }
                catch { }
                Thread.Sleep(600);
            }
        }

        public static void GetName(GameModel GameM)
        {
            foreach (AccountModel A in GameM.ListAccount)
            {
                try
                {
                    if (A.Name == Language.NA)
                    {
                        if (A.Func.ReadLoading())
                        {
                            UpdateChar(A);
                        }
                    }
                    else
                    {
                        if (A.Func.ReadLoading())
                        {
                            UpdateChar(A);
                        }
                        else
                        {
                            A.IsAuto = false;
                            A.Name = Language.NA;
                            A.Hp = Language.NA;
                            A.Mp = Language.NA;
                            A.Exp = Language.NA;
                            A.Level = Language.NA;
                        }
                    }
                    Thread.Sleep(300);
                }
                catch { }
            }
        }

        private static void UpdateChar(AccountModel A)
        {
            try
            {
                string name = A.Func.ReadCharRead();
                if (!string.IsNullOrEmpty(RemoveUnwantedChar(name)))
                {
                    A.Name = RemoveUnwantedChar(name);
                    A.Hp = A.Func.ReadHp().ToString() + "%";
                    A.Mp = A.Func.ReadMp().ToString() + "%";
                    A.Exp = A.Func.ReadExp().ToString() + "%";
                    A.Level = A.Func.ReadLevel().ToString();
                    A.Limit_Crep = A.Func.Min_limit_crep;
                }
            }
            catch { }
        }

        private static void CleanAll(GameModel GameM, LoginModel LoginM)
        {
            LoginM.RegClient = 0;
            if (GameM != null)
            {
                foreach (AccountModel A in GameM.ListAccount)
                {
                    try
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            A.Func.Login = false;
                            A.IsAuto = false;
                        });
                    }
                    catch
                    {
                        try
                        {
                            A.Func.Login = false;
                            A.IsAuto = false;
                        }
                        catch { }
                    }
                }
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    GameM.ListAccount.Clear();
                });

                AccountModel AM = new AccountModel
                {
                    Name = Language.KichAccountT
                };
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    GameM.ListAccount.Add(AM);
                });
            }
        }

        public static string RemoveUnwantedChar(string input)
        {
            StringBuilder builder = new StringBuilder(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetterOrDigit(input[i]))
                {
                    builder.Append(input[i]);
                }
            }
            return builder.ToString();
        }

        public static bool ProcessExists(int id)
        {
            try
            {
                return Process.GetProcesses().Any(x => x.Id == id);
            }
            catch
            {
                return false;
            }
        }
    }
}