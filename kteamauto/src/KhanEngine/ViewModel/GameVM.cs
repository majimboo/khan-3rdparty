using Lang;
using Memory;
using Net.ControlModel;
using Net.Model;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;

namespace KhanEngine.ViewModel
{
    public class GameVM
    {
        private static Language Language = new Language();
        //[BrowsableAttribute(false)]
        //public bool IsActive { get; }

        #region call class

        public GameModel GameM { get; set; }
        public AccountModel Account { get; set; }

        public static int IndexCort = 0;
        public LoginModel LoginM { get; set; }

        public InfoView InfoV { get; set; }
        public static InfoView InfoVi { get; set; }

        #endregion call class

        #region ICommand

        public ICommand AutoCommand { get; }
        public ICommand ButtonAutoCommand { get; }
        public ICommand AutoBuffCommand { get; }
        public ICommand AutoTrainCommand { get; }
        public ICommand BlockCordCommand { get; }
        public ICommand LeftRightCommand { get; }
        public ICommand AddCordCommand { get; }
        public ICommand GetIDCommand { get; }
        public ICommand StartAttack2Command { get; }
        public ICommand SaveCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand RemoveAllCommand { get; }
        public ICommand FastClickCommand { get; }

        #endregion ICommand

        public GameVM(GameModel _gameM, LoginModel _loginM, InfoView _InfoV)
        {
            GameM = _gameM;
            LoginM = _loginM;
            InfoV = _InfoV;
            InfoVi = _InfoV;
            //GetWinActive();
            AddCordCommand = new ReplayCommand(StartAddCord);
            AutoBuffCommand = new ReplayCommand(StartBuffSkill);
            RemoveAllCommand = new ReplayCommand(ItemRemoveAll);
            LeftRightCommand = new ReplayCommand(ButtonMouse);
            FastClickCommand = new ReplayCommand(StartFastClick);
            SaveCommand = new ReplayCommand(StartSave);
            OpenCommand = new ReplayCommand(StartOpen);
            GetIDCommand = new ReplayCommand(StartGetIdSKill);
            StartAttack2Command = new ReplayCommand(StartSkill2);
            AutoCommand = new ReplayCommand(StartAuto);
            AutoTrainCommand = new ReplayCommand(StartAutoTrain);
            BlockCordCommand = new ReplayCommand(StartGetRange);
        }

        #region call start

        private void StartGetRange()
        {
            AccountModel iAccount = Account;
            try
            {
                Function F = iAccount.Func;
                if (iAccount.IsAuto)
                {
                    if (Account.IsBlockCord)
                    {
                        F.SetCord();
                        Account.XR = F.X;
                        Account.YR = F.Y;
                    }
                    else
                    {
                        Account.XR = 0;
                        Account.YR = 0;
                    }
                }
                else
                {
                    Account.IsBlockCord = false;
                }
            }
            catch
            {
                Account.IsBlockCord = false;
            }
        }

        private void StartAutoTrain()
        {
            AccountModel iAccount = Account;
            try
            {
                if (iAccount.IsAuto && iAccount.IsAutoTrain)
                {
                    iAccount.Func.ONTrain = true;

                    GameControl.AutoTrainIndex(iAccount, LoginM);
                }
                else
                {
                    iAccount.Func.ONTrain = false;
                }
            }
            catch { iAccount.IsAutoTrain = false; }
        }

        public void StartAuto()
        {
            AccountModel iAccount = Account;
            GameControl.StartAuto(GameM, iAccount, LoginM);
        }

        private void StartSkill2()
        {
            AccountModel iAccount = Account;
            try
            {
                if (iAccount.IsAuto && iAccount.IsAttack2)
                {
                    iAccount.SetSkill2 = new Thread(delegate () { GameControl.SetIdSKill(iAccount); })
                    {
                        IsBackground = true
                    };
                    iAccount.SetSkill2.Start();
                }
                else
                {
                    iAccount.IsAttack2 = false;
                    iAccount.SetSkill2.Abort();
                }
            }
            catch
            {
                iAccount.IsAttack2 = false;
            }
        }

        private void StartGetIdSKill()
        {
            AccountModel iAccount = Account;
            try
            {
                if (iAccount.IsAuto)
                {
                    Function F = iAccount.Func;
                    iAccount.IDSKIll = F.GetIdSkill();
                }
            }
            catch
            {
                iAccount.IsAttack2 = false;
            }
        }

        private void StartSave()
        {
            AccountModel iAccount = Account;
            try
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = Language.NameFile,
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string Path = dialog.FileName.ToString();
                    Mill.IniFile.ChangePath(Path);
                    GameControl.Saveing(iAccount);
                }
            }
            catch { MessageBox.Show(Language.ErrorSaveing, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void StartOpen()
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = Language.NameFile,
                    FilterIndex = 1,
                    RestoreDirectory = true
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AccountModel iAccount = Account;
                    string Path = dialog.FileName.ToString();
                    Mill.IniFile.ChangePath(Path);
                    GameControl.Open(iAccount);
                }
            }
            catch { MessageBox.Show(Language.ErrorOpen, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void StartFastClick()
        {
            AccountModel iAccount = Account;
            try
            {
                if (iAccount.IsClickSpeed)
                {
                    iAccount.FastClick = new Thread(delegate () { GameControl.FastClick(iAccount); })
                    {
                        IsBackground = true
                    };
                    iAccount.FastClick.Start();
                }
                else
                {
                    iAccount.FastClick.Abort();
                }
            }
            catch
            {
                iAccount.IsClickSpeed = false;
                iAccount.FastClick.Abort();
            }
        }

        private void ButtonMouse()
        {
            AccountModel iAccount = Account;
            try
            {
                if (iAccount.IsAutoClick)
                {
                    iAccount.Click = Language.MouseClick[1];
                }
                else
                {
                    iAccount.Click = Language.MouseClick[0];
                }
            }
            catch
            {
                iAccount.IsAutoClick = false;
            }
        }

        private void ItemRemoveAll()
        {
            AccountModel iAccount = Account;
            try
            {
                iAccount.ListCord.Clear();
            }
            catch { }
        }

        private void StartAddCord()
        {
            AccountModel iAccount = Account;
            try
            {
                if (!iAccount.IsAuto)
                {
                    MessageBox.Show(Language.StartAuto, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (iAccount.IsAuto && !iAccount.IsAddCort)
                {
                    iAccount.IsAddCort = true;
                    iAccount.TAddcort = new Thread(delegate () { AddCoordinates(iAccount); })
                    {
                        IsBackground = true
                    };
                    iAccount.TAddcort.Start();

                    iAccount.AddCortText = Language.Addcort[1];
                }
                else
                {
                    iAccount.TAddcort.Abort();
                    iAccount.AddCortText = Language.Addcort[0];
                    iAccount.IsAddCort = false;

                    iAccount.CortStart = 0;
                }
            }
            catch
            {
                iAccount.AddCortText = Language.Addcort[0];
                iAccount.IsAddCort = false;
            }
        }

        private void StartBuffSkill()
        {
        }

        #endregion call start

        // add list AddCoordinates

        public static void AddCoordinates(AccountModel Account)
        {
            try
            {
                int[] ReadStart = new int[2] { 0, 0 }, ReadReply = new int[2] { 0, 0 };
                Function F = Account.Func;
                int ID = Account.ListCord.Count, Index = 0;

                while (Account.IsAddCort && Account.IsAuto)
                {
                    try
                    {
                        Index = Account.ListCord.Count - 1;

                        try
                        {
                            if (Index > -1)
                            {
                                ReadStart[0] = Account.ListCord[0].X;
                                ReadStart[1] = Account.ListCord[0].Y;
                                ReadReply[0] = Account.ListCord[Index].X;
                                ReadReply[1] = Account.ListCord[Index].Y;
                                Account.CortStart = Convert.ToInt32(F.ReadBlockRadius(ReadStart));
                            }
                        }
                        catch { }
                        if (Account.ListCord.Count <= 0 || F.ReadBlockRadius(ReadReply) >= Account.RadiusInput)
                        {
                            ID++;
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                Account.ListCord.Add(new ListCord { Id = ID, X = F.ReadX(), Y = F.ReadY() });
                            });
                        }
                    }
                    catch { break; }
                    Thread.Sleep(200);
                }
            }
            catch { }
        }
    }
}