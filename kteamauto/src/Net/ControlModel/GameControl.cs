using Lang;
using Memory;
using Net.Model;
using Setting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Net.ControlModel
{
    public static class GameControl
    {
        public static int IndexT = 0;
        private static Language Language = new Language();
        private static int SleepClick = 0;
        private static int[] CortNow = new int[2];

        // set Start train
        public static void StartAll(AccountModel Account)
        {
            try
            {
                Function F = Account.Func;
                while (MAPN.ProcessExists(Account.Pid) && Account.IsAuto)
                {
                    try
                    {
                        if (Account.IsAutoTrain)
                        {
                            if (!Account.IsBlockCord && Account.ListCord.Count > 0 && CortNow != null)
                            {
                                try
                                {
                                    Account.Tene = Convert.ToInt32(F.ReadBlockRadius(CortNow));
                                    Account.IDCort = (IndexT + 1);
                                }
                                catch { }
                            }
                            else if (Account.IsBlockCord && Account.ListCord.Count <= 0)
                            {
                                Account.Tene = Convert.ToInt32(F.ReadBlockRadius());
                            }

                            F.AttackTH(Account.LagInput);

                            if (Account.RadiusInput == F.Radius && F.ID_SELECT == -1)
                            {
                                if (Account.ListCord.Count > 0 && (!Account.IsBlockCord))
                                {
                                    if (F.ReadState() == 0 && (F.ReadCharState() == 25 || F.ReadCharState() == 51) && F.ID_SELECT == -1)
                                    {
                                        F.SetState();
                                    }

                                    CortNow[0] = Account.ListCord[IndexT].X;
                                    CortNow[1] = Account.ListCord[IndexT].Y;
                                    F.Move(CortNow[0], CortNow[1]);
                                    if (F.FIsNear(CortNow[0], CortNow[1], Account.RadiusInput))
                                    {
                                        SetIndex(Account, ref IndexT);
                                    }
                                    CortNow[0] = Account.ListCord[IndexT].X;
                                    CortNow[1] = Account.ListCord[IndexT].Y;
                                    int LX = 0, LY = 0;
                                    while ((!F.FIsNear(CortNow[0], CortNow[1], Account.RadiusInput)) && Account.IsAutoTrain)
                                    {
                                        F.Move(CortNow[0], CortNow[1]);
                                        if (LX == F.ReadX() && LY == F.ReadY())
                                        {
                                            break;
                                        }
                                        LX = F.ReadX();
                                        LY = F.ReadY();
                                        Thread.Sleep(100);
                                    }
                                }
                                else if (Account.ListCord.Count <= 0 || Account.IsBlockCord)
                                {
                                    F.ReturnCord();
                                    CortNow = null;
                                }
                            }
                            if (F.IsMoveT && CortNow != null)
                            {
                                F.MoveTrain(CortNow);
                            }
                            if (!F.ONTrain)
                            {
                                Account.IsAutoTrain = F.ONTrain;
                            }
                            else if (Account.IsAutoBuff)
                            {
                                BuffSkill(Account);
                            }
                        }
                        else { }
                    }
                    catch (Exception E) { MessageBox.Show(E.ToString()); }
                    try
                    {
                        if (Account.IsCut)
                        {
                            if (F.ReadState() == 3)
                            {
                                F.SetCutSkill(Account.CutInput);
                            }
                        }
                    }
                    catch
                    {
                        Account.IsCut = false;
                    }
                    if (MAPN.WinActive(Account.Pid) && Account.IsAutoClick)
                    {
                        try
                        {
                            if (F.ReadMob() == 1 && Account.IsAutoClick && !Account.IsAutoTrain)
                            {
                                if (!Account.IsMouseClick)
                                {
                                    F.AttackMob(0, F.ReadMobId(), true);
                                }
                                else if (Account.IsMouseClick)
                                {
                                    F.AttackMob(1, F.ReadMobId(), true);
                                }
                            }
                        }
                        catch
                        {
                            Account.IsAutoClick = false;
                        }
                    }

                    Thread.Sleep(30 + SleepClick);
                }
            }
            catch { Account.IsAuto = false; }
        }

        public static void StartAuto(GameModel GameM, AccountModel iAccount, LoginModel LoginM)
        {
            try
            {
                if (!LoginM.Login)
                {
                    iAccount.IsAuto = false;
                    MessageBox.Show(Language.KichAccount, Language.Notification, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch { return; }

            try
            {
                if (iAccount.IsAuto)
                {
                    GameM.Clients = GetNumberClient(GameM) - 1;

                    if (LoginM.RegClient <= 0)
                    {
                        iAccount.IsAuto = false;
                        MessageBox.Show(Language.ZeroClients, Language.Notification, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (GameM.Clients < LoginM.RegClient)
                    {
                        if (iAccount.Name == Language.NA)
                        {
                            MessageBox.Show(Language.NullChar, Language.Notification, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            iAccount.IsAuto = false;
                            return;
                        }

                        GameM.Clients++;

                        if (iAccount.IsAutoBuff) { BuffSkill(iAccount, false); }

                        iAccount.TAuto = new Thread(delegate () { AutoBuff(iAccount, GameM); })
                        {
                            IsBackground = true
                        };
                        iAccount.TAuto.Start();

                        iAccount.TAutoTrain = new Thread(delegate () { StartAll(iAccount); })
                        {
                            IsBackground = true
                        };
                        iAccount.TAutoTrain.Start();
                    }
                    else
                    {
                        ReturnAll(iAccount, GameM);
                        MessageBox.Show(Language.UserClients, Language.Notification);
                        return;
                    }
                }
                else
                {
                    ReturnAll(iAccount, GameM);
                }
            }
            catch
            {
                iAccount.IsAuto = false;
            }
        }

        public static void ReturnAll(AccountModel iAccount, GameModel GameM)
        {
            iAccount.IsAuto = false;
            GameM.Clients = GetNumberClient(GameM);
            iAccount.TAuto.Abort();
            iAccount.TAutoTrain.Abort();
            iAccount.FastClick.Abort();
            iAccount.IsClickSpeed = false;
            iAccount.IsAttack2 = false;
        }

        // set Start Auto Hp/Mp/Exp/Shutdown
        public static void AutoBuff(AccountModel Account, GameModel GameM)
        {
            Function Funcs = Account.Func;
            int TimeAutoHP = 0, hp = 0, mp = 0, exp = 0;
            try
            {
                while (MAPN.ProcessExists(Account.Pid) && Account.IsAuto)
                {
                    try
                    {
                        TimeAutoHP = Account.AutoInput;
                        if (Account.IsHp)
                        {
                            hp = Account.HpInput;
                            if (Funcs.ReadHp() <= hp)
                            {
                                Funcs.SendKeyQ();
                                Thread.Sleep(10);
                            }
                        }

                        if (Account.IsMp)
                        {
                            mp = Account.MpInput;
                            if (Funcs.ReadMp() <= mp)
                            {
                                Funcs.SendKeyW();
                                Thread.Sleep(10);
                            }
                        }

                        if (Account.IsExP)
                        {
                            exp = Account.ExpInput;
                            if (Funcs.ReadExp() >= exp)
                            {
                                Account.IsAuto = false;
                                GameM.Clients--;
                                Console.Beep();
                                MessageBox.Show(Language.FullExp, Language.Set(Language.NotificationT, Account.Name));
                            }
                        }
                        if (Account.IsShutdown && Account.IsAutoTrain)
                        {
                            if (Funcs.ReadHp() <= Account.HpShutDown)
                            {
                                Account.IsAuto = false;
                                Console.Beep();
                                Console.Beep();
                                Process.GetProcessById(Account.Pid).Kill();
                            }
                        }

                        Thread.Sleep(TimeAutoHP + 10);
                    }
                    catch (ThreadAbortException) { Account.IsAuto = false; break; }
                }
            }
            catch (ThreadAbortException) { Account.IsAuto = false; }
        }

        // buff skill
        public static void BuffSkill(AccountModel Account, bool Start = true)
        {
            try
            {
                Function F = Account.Func;
                if (Start)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Account.IsBuff[i] && double.Parse(Account.TimeBuff[i].Elapsed.ToString()) >= Account.STimeSkill[i])
                        {
                            F.SetSkill(Account.ListIdSkill[i + 1], 25);
                            LoadBuff(Account, Account.LoadSkill[i]);
                            Account.TimeBuff[i] = Stopwatch.StartNew();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (Account.IsBuff[i])
                        {
                            F.SetSkill(Account.ListIdSkill[i + 1], 25);
                            LoadBuff(Account, Account.LoadSkill[i]);
                            Account.TimeBuff[i] = Stopwatch.StartNew();
                        }
                    }
                }
            }
            catch { }
        }

        public static void LoadBuff(AccountModel Account, int NumberB)
        {
            try
            {
                Function F = Account.Func;
                for (int i = 0; i < NumberB; i++)
                {
                    F.BuffPT(25);
                    F.SetCutSkill(0);
                    F.BuffPT(25);
                    F.SetCutSkill(0);
                    Thread.Sleep(100);
                }
                F.MoveBuff(50);
                F.SetSkill(Account.ListIdSkill[0], 25);
            }
            catch { }
        }

        // speed mouse attack mob
        public static void FastClick(AccountModel Account)
        {
            try
            {
                Function F = Account.Func;
                while (Account.IsClickSpeed && Account.IsAuto)
                {
                    try
                    {
                        if (Account.IsClickSpeed && MAPN.WinActive(Account.Pid))
                        {
                            while (MAPN.KeyIsDown(Keys.LButton) && F.ReadMob() == 1 && Account.IsAuto)
                            {
                                Thread.Sleep(250);
                                while (MAPN.KeyIsDown(Keys.LButton) && Account.IsAuto && F.ReadMob() == 1)
                                {
                                    F.AttackMob(0, F.ReadMobId(), true);
                                }
                            }

                            while (MAPN.KeyIsDown(Keys.RButton) && F.ReadMob() == 1 && Account.IsAuto)
                            {
                                Thread.Sleep(250);
                                while (MAPN.KeyIsDown(Keys.RButton) && Account.IsAuto && F.ReadMob() == 1)
                                {
                                    F.AttackMob(1, F.ReadMobId(), true);
                                }
                            }
                        }
                    }
                    catch
                    {
                        Account.IsClickSpeed = false;
                    }
                    Thread.Sleep(200);
                }
            }
            catch
            {
                Account.IsClickSpeed = false;
            }
        }

        // set skill attack
        public static void SetIdSKill(AccountModel Account)
        {
            try
            {
                Function F = Account.Func;
                while (Account.IsAttack2 && Account.IsAuto)
                {
                    if (F.ReadState() == 0 && F.ReadCutSkill() != 1800)
                    {
                        F.SetSkill(Account.IDSKIll, true);
                    }
                    Thread.Sleep(Account.TimeSkill);
                }
            }
            catch
            {
                Account.IsAttack2 = false;
                Account.SetSkill2.Abort();
            }
        }

        // Save and open

        public static void Saveing(AccountModel R)
        {
            try
            {
                #region main

                Mill.IniWrite(KeyString.Main[0], KeyString.Main[1], R.AutoInput);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[2], R.IsHp);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[3], R.HpInput);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[4], R.IsMp);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[5], R.MpInput);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[6], R.IsExP);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[7], R.ExpInput);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[8], R.IsCut);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[9], R.CutInput);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[10], R.IsClickSpeed);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[11], R.IsAutoClick);
                Mill.IniWrite(KeyString.Main[0], KeyString.Main[12], R.IsMouseClick);

                #endregion main

                #region train

                Mill.IniWrite(KeyString.Train[0], KeyString.Train[1], R.IsKillToDie);
                Mill.IniWrite(KeyString.Train[0], KeyString.Train[2], R.IsAutoBuff);
                Mill.IniWrite(KeyString.Train[0], KeyString.Train[3], R.IsShutdown);
                Mill.IniWrite(KeyString.Train[0], KeyString.Train[4], R.HpShutDown);
                Mill.IniWrite(KeyString.Train[0], KeyString.Train[5], R.ReadRangeInput);
                Mill.IniWrite(KeyString.Train[0], KeyString.Train[6], R.RadiusInput);
                Mill.IniWrite(KeyString.Train[0], KeyString.Train[7], R.LagInput);

                #endregion train

                #region Skill

                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[1], R.IComboxName);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[2], R.IsAttack);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[3], R.ISkillAttack);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[4], R.IsBuff[0]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[5], R.ISkillBuff_1);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[6], R.STimeSkill[0]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[7], R.LoadSkill[0]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[8], R.IsBuff[1]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[9], R.ISkillBuff_2);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[10], R.STimeSkill[1]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[11], R.LoadSkill[1]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[12], R.IsBuff[2]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[13], R.ISkillBuff_3);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[14], R.STimeSkill[2]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[15], R.LoadSkill[2]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[16], R.IsBuff[3]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[17], R.ISkillBuff_4);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[18], R.STimeSkill[3]);
                Mill.IniWrite(KeyString.Skill[0], KeyString.Skill[19], R.LoadSkill[3]);

                #endregion Skill

                if (R.ListCord.Count > 0)
                {
                    foreach (var item in R.ListCord)
                    {
                        Mill.IniWrite(KeyString.NameAdd[1], Mill.Change(item.Id), item.X, item.Y);
                        Thread.Sleep(1);
                    }
                }

                MessageBox.Show(Language.Set(Language.Saveing, Mill.IniFile.Path()), Language.Notification);
            }
            catch
            {
                MessageBox.Show(Language.ErrorSaveing, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Open(AccountModel R)
        {
            try
            {
                try
                {
                    #region main

                    R.AutoInput = Mill.Change(KeyString.Main[0], KeyString.Main[1], R.AutoInput);
                    R.IsHp = Mill.Change(KeyString.Main[0], KeyString.Main[2], R.IsHp);
                    R.HpInput = Mill.Change(KeyString.Main[0], KeyString.Main[3], R.HpInput);
                    R.IsMp = Mill.Change(KeyString.Main[0], KeyString.Main[4], R.IsMp);
                    R.MpInput = Mill.Change(KeyString.Main[0], KeyString.Main[5], R.MpInput);
                    R.IsExP = Mill.Change(KeyString.Main[0], KeyString.Main[6], R.IsExP);
                    R.ExpInput = Mill.Change(KeyString.Main[0], KeyString.Main[7], R.ExpInput);
                    R.IsCut = Mill.Change(KeyString.Main[0], KeyString.Main[8], R.IsCut);
                    R.CutInput = Mill.Change(KeyString.Main[0], KeyString.Main[9], R.CutInput);
                    R.IsClickSpeed = Mill.Change(KeyString.Main[0], KeyString.Main[10], R.IsClickSpeed);
                    R.IsAutoClick = Mill.Change(KeyString.Main[0], KeyString.Main[11], R.IsAutoClick);
                    R.IsMouseClick = Mill.Change(KeyString.Main[0], KeyString.Main[12], R.IsMouseClick);

                    #endregion main

                    #region train

                    R.IsKillToDie = Mill.Change(KeyString.Train[0], KeyString.Train[1], R.IsKillToDie);
                    R.IsAutoBuff = Mill.Change(KeyString.Train[0], KeyString.Train[2], R.IsAutoBuff);
                    R.IsShutdown = Mill.Change(KeyString.Train[0], KeyString.Train[3], R.IsShutdown);
                    R.HpShutDown = Mill.Change(KeyString.Train[0], KeyString.Train[4], R.HpShutDown);
                    R.ReadRangeInput = Mill.Change(KeyString.Train[0], KeyString.Train[5], R.ReadRangeInput);
                    R.RadiusInput = Mill.Change(KeyString.Train[0], KeyString.Train[6], R.RadiusInput);
                    R.LagInput = Mill.Change(KeyString.Train[0], KeyString.Train[7], R.LagInput);

                    #endregion train

                    #region Skill

                    R.IComboxName = Mill.Change(KeyString.Skill[0], KeyString.Skill[1], R.IComboxName);
                    R.IsAttack = Mill.Change(KeyString.Skill[0], KeyString.Skill[2], R.IsAttack);
                    R.ISkillAttack = Mill.Change(KeyString.Skill[0], KeyString.Skill[3], R.ISkillAttack);
                    R.IsBuff[0] = Mill.Change(KeyString.Skill[0], KeyString.Skill[4], R.IsBuff[0]);
                    R.ISkillBuff_1 = Mill.Change(KeyString.Skill[0], KeyString.Skill[5], R.ISkillBuff_1);
                    R.STimeSkill[0] = Mill.Change(KeyString.Skill[0], KeyString.Skill[6], R.STimeSkill[0]);
                    R.LoadSkill[0] = Mill.Change(KeyString.Skill[0], KeyString.Skill[7], R.LoadSkill[0]);
                    R.IsBuff[1] = Mill.Change(KeyString.Skill[0], KeyString.Skill[8], R.IsBuff[1]);
                    R.ISkillBuff_2 = Mill.Change(KeyString.Skill[0], KeyString.Skill[9], R.ISkillBuff_2);
                    R.STimeSkill[1] = Mill.Change(KeyString.Skill[0], KeyString.Skill[10], R.STimeSkill[1]);
                    R.LoadSkill[1] = Mill.Change(KeyString.Skill[0], KeyString.Skill[11], R.LoadSkill[1]);
                    R.IsBuff[2] = Mill.Change(KeyString.Skill[0], KeyString.Skill[12], R.IsBuff[2]);
                    R.ISkillBuff_3 = Mill.Change(KeyString.Skill[0], KeyString.Skill[13], R.ISkillBuff_3);
                    R.STimeSkill[2] = Mill.Change(KeyString.Skill[0], KeyString.Skill[14], R.STimeSkill[2]);
                    R.LoadSkill[2] = Mill.Change(KeyString.Skill[0], KeyString.Skill[15], R.LoadSkill[2]);
                    R.IsBuff[3] = Mill.Change(KeyString.Skill[0], KeyString.Skill[16], R.IsBuff[3]);
                    R.ISkillBuff_4 = Mill.Change(KeyString.Skill[0], KeyString.Skill[17], R.ISkillBuff_4);
                    R.STimeSkill[3] = Mill.Change(KeyString.Skill[0], KeyString.Skill[18], R.STimeSkill[3]);
                    R.LoadSkill[3] = Mill.Change(KeyString.Skill[0], KeyString.Skill[19], R.LoadSkill[3]);

                    #endregion Skill
                }
                catch { }
                string[] SetGet = null;
                R.ListCord.Clear();

                for (int i = 1; i <= 999999999; i++)
                {
                    try
                    {
                        ListCord Cort = new ListCord();
                        SetGet = Mill.IniRead(KeyString.NameAdd[1], i);
                        Cort.Id = i;
                        Cort.X = Mill.Change(Cort.X, SetGet[0]);
                        Cort.Y = Mill.Change(Cort.Y, SetGet[1]);
                        R.ListCord.Add(Cort);
                        Thread.Sleep(10);
                    }
                    catch
                    {
                        break;
                    }
                }
                MessageBox.Show(Language.Success, Language.Notification);
            }
            catch
            {
                MessageBox.Show(Language.ErrorOpen, Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static int GetNumberClient(GameModel GameM)
        {
            int ID = 0;
            foreach (var item in GameM.ListAccount)
            {
                if (item.IsAuto)
                {
                    ID++;
                }
            }
            return ID;
        }

        public static void AutoTrainIndex(AccountModel iAccount, LoginModel LoginM)
        {
            try
            {
                if (iAccount.IsAutoTrain)
                {
                    Function F = iAccount.Func;
                    F.Max_limit_crep = LoginM.Max_limit_crep;
                    F.Min_limit_crep = 0;
                    IndexT = 0;
                    ReadIdMove(iAccount);
                }
                else
                {
                    iAccount.Limit_Crep = 0;
                }
            }
            catch { iAccount.IsAutoTrain = false; }
        }

        public static void ReadIdMove(AccountModel account)
        {
            try
            {
                Function F = account.Func;
                IndexT = 0;
                int[] Cort = new int[2];
                double ReturnID = 9000;
                for (int i = 0; i < account.ListCord.Count; i++)
                {
                    Cort[0] = account.ListCord[i].X;
                    Cort[1] = account.ListCord[i].Y;
                    if (F.ReadBlockRadius(Cort) < ReturnID)
                    {
                        ReturnID = F.ReadBlockRadius(Cort);
                        IndexT = i;
                    }
                }
                CortNow[0] = account.ListCord[IndexT].X;
                CortNow[1] = account.ListCord[IndexT].Y;
            }
            catch { }
        }

        private static void SetIndex(AccountModel account, ref int Index)
        {
            Index++;
            if (Index >= account.ListCord.Count) { Index = 0; }
        }
    }
}