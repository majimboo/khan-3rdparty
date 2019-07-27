using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using EncryptDataSet.Data;
using System.Windows.Forms;
using System.IO;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Patterns;
using ClosedXML.Excel;

namespace TheForce
{

    public partial class TheForce : Form
    {
        bool app_exit = false;
        int kPid = 0;
        CutAdd CA = new CutAdd();


        KWatch KW = new KWatch();
        aClick AC = new aClick();
        cntrl EN = new cntrl();
        mentry MN = new mentry();
        mem_hack MeM = new mem_hack();
        Scan_Param t_scan = new Scan_Param();
        udet u = new udet();
        bot BT = new bot();
        HKeys hk = new HKeys();
        DataView dv;
        TabControlHelper tab_ctrl = new TabControlHelper();
        List<pListChar> pList;

        public DataSet extern_dataset;
        public decimal dec_x = 32;
        public decimal dec_y = -32;
        string fname = "";
        string oname = "";
        //ProcessMemoryReader khan = new ProcessMemoryReader();
        public ProcessSharp k;
        public pSupport sup = new pSupport();
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetAsyncKeyState(int vkey);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public static class thc
        {

            delegate void SetTextCallback(Form f, Control ctrl, string text);
            /// <summary>
            /// Set text property of various controls
            /// </summary>
            /// <param name="form">The calling form</param>
            /// <param name="ctrl"></param>
            /// <param name="text"></param>
            public static void SetText(Form form, Control ctrl, string text)
            {
                // InvokeRequired required compares the thread ID of the 
                // calling thread to the thread ID of the creating thread. 
                // If these threads are different, it returns true. 
                if (ctrl.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    form.Invoke(d, new object[] { form, ctrl, text });
                }
                else
                {
                    ctrl.Text = text;
                }
            }
        }


        public TheForce()
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            EN.Me = false;
            MN.pHack = false;
            InitDefault();
            runner.RunWorkerAsync();
            if (EN.Me)
            {
                ReloadOptions();
                cntr_overide();
            }

            btn_temp.Visible = EN.Me;

            this.Hide();
            Client fr = new Client(ref CA.game_name, EN.Me);
            fr.ShowDialog();
            CA.game_name = fr.cn;
            this.BringToFront();
            // }
            if (EN.Expr)
                checker.RunWorkerAsync();
            FindPLock.RunWorkerAsync();

        }

        public Keys GetHKSet(string obj)
        {
            Keys ret = Keys.None;
            bool IsObj = false;
            bool IsKey = false;
            string objKey = "";
            uint objHK = 0;
            foreach (DataRow dr in hot_keys.Rows)
            {
                if (dr["Object"].ToString() == obj)
                {
                    IsObj = true;
                    objKey = dr["Key"].ToString();
                    break;
                }
            }
            if (IsObj)
            {
                foreach (DataRow dr in tab_key_list.Rows)
                {
                    if (dr["Key"].ToString() == objKey)
                    {
                        objHK = Convert.ToUInt32(dr["Code"].ToString());
                        IsKey = true;
                    }
                }
            }
            if (IsKey)
                ret = (Keys)objHK;

            return ret;
        }

        public void AdminHK()
        {
            string[] hk = new string[] { "Auto Click" , "Exp Watch", "Force Remove", "Fast Run", "Lock On", "Heal"
                                        ,"Multi Target", "No Animation", "Pick Item", "Use Skill", "Use Skill List"
                                        ,"Set Item", "Start", "Npc", "Start Bot"};
            bool add = true;
            foreach (string st in hk)
            {
                add = true;
                foreach (DataRow hk_lst in hot_keys.Rows)
                {
                    if (st == hk_lst["Object"].ToString())
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    hot_keys.Rows.Add(st);
            }

        }
        public void Init_Keys()
        {
            tab_key_list.Rows.Add("", 0);
            tab_key_list.Rows.Add("F1", Keys.F1);
            tab_key_list.Rows.Add("F2", Keys.F2);
            tab_key_list.Rows.Add("F3", Keys.F3);
            tab_key_list.Rows.Add("F4", Keys.F4);
            tab_key_list.Rows.Add("F5", Keys.F5);
            tab_key_list.Rows.Add("F6", Keys.F6);
            tab_key_list.Rows.Add("F7", Keys.F7);
            tab_key_list.Rows.Add("F8", Keys.F8);
            tab_key_list.Rows.Add("F9", Keys.F9);
            tab_key_list.Rows.Add("F10", Keys.F10);
            tab_key_list.Rows.Add("F11", Keys.F11);
            tab_key_list.Rows.Add("F12", Keys.F12);
            tab_key_list.Rows.Add("0", Keys.D0);
            tab_key_list.Rows.Add("1", Keys.D1);
            tab_key_list.Rows.Add("2", Keys.D2);
            tab_key_list.Rows.Add("3", Keys.D3);
            tab_key_list.Rows.Add("4", Keys.D4);
            tab_key_list.Rows.Add("5", Keys.D5);
            tab_key_list.Rows.Add("6", Keys.D6);
            tab_key_list.Rows.Add("7", Keys.D7);
            tab_key_list.Rows.Add("8", Keys.D8);
            tab_key_list.Rows.Add("9", Keys.D9);
            tab_key_list.Rows.Add("Space", Keys.Space);
            tab_key_list.Rows.Add("L_Control", Keys.LControlKey);
            tab_key_list.Rows.Add("R_Control", Keys.RControlKey);

            hot_keys.Rows.Add("Auto Click");
            hot_keys.Rows.Add("Exp Watch");
            hot_keys.Rows.Add("Force Remove");
            hot_keys.Rows.Add("Fast Run");
            hot_keys.Rows.Add("Lock On");
            hot_keys.Rows.Add("Heal");
            hot_keys.Rows.Add("Multi Target");
            hot_keys.Rows.Add("No Animation");
            hot_keys.Rows.Add("Pick Item");
            hot_keys.Rows.Add("Use Skill");
            hot_keys.Rows.Add("Use Skill List");
            hot_keys.Rows.Add("Set Item");
            hot_keys.Rows.Add("Start");
            hot_keys.Rows.Add("Npc");
            hot_keys.Rows.Add("Start Bot");
        }
        public void RemHK(string obj)
        {
            int rem_ind = -1;
            foreach (DataRow dr in hot_keys.Rows)
            {
                rem_ind++;
                if (dr["Object"].ToString() == obj)
                {
                    hot_keys.Rows.RemoveAt(rem_ind);
                    break;
                }
            }
        }


        public void cntr_overide()
        {

            // Booster Pack
            txt_boost.Visible = EN.OpenBooster;
            txt_boost.Enabled = EN.OpenBooster;

            // Extreme Pack
            g_extreme.Visible = EN.Extreme;
            g_extreme.Enabled = EN.Extreme;
            g_ldq_bug.Visible = EN.Extreme;
            g_ldq_bug.Enabled = EN.Extreme;
            txt_esc_delay.Visible = EN.Extreme;
            g_armor.Enabled = EN.ArmorHack;
            g_armor.Visible = EN.ArmorHack;
            btn_item_stat.Enabled = EN.WeapMod;
            btn_item_stat.Visible = EN.WeapMod;
            txtRange.Visible = EN.Extreme;
            if (!EN.Extreme)
            {
                tab_ctrl.HidePage(support);
                RemHK("Pick Item");
                RemHK("Fast Run");
            }
            if (!EN.Bot)
            {
                RemHK("Start Bot");
                tab_ctrl.HidePage(bot);
                tab_ctrl.HidePage(admin);
                tab_ctrl.HidePage(loot);
            }
            if (!EN.Items)
            {
                g_Items_obj.Enabled = false;
                g_Items_obj.Visible = false;
                RemHK("Set Item");
            }
            if (!EN.Me)
            {
                btn_my_account.Visible = false;
                //tab_ctrl.HidePage(admin);
            }
            //if (EN.Me)
            //btn_temp.Visible = EN.Me;
            btn_temp.Enabled = EN.Me;

            // Multi Target Pack
            //KW.mt.mtarget = EN.Multi_Targ;
            KW.mt.style1 = EN.Multi_Target;
            KW.mt.style2 = EN.Multi_Target;

            //if (EN.Multi_Target)
            //{
            g_MultiTarget.Visible = EN.Multi_Target;
            g_MultiTarget.Enabled = EN.Multi_Target;
            //g_extreme.Visible = EN.Extreme;
            //}

            if (!EN.Multi_Target)
                RemHK("Multi Target");

            if (!EN.NPC)
            {
                RemHK("Npc");
                KW.npc = false;
                g_npc_obj.Enabled = false;
            }
            if (!EN.Items && !EN.NPC)
                tab_ctrl.HidePage(npc);

            pHack.Enabled = MN.pHack;
            btnSave.Enabled = MN.pHack;
            btnStart.Enabled = MN.pHack;
            btnLoad.Enabled = MN.pHack;
            // TS
            ts_cut_on.Visible = KW.start_force;
            ts_use_skill.Visible = KW.use_skill;
            ts_bot.Visible = KW.bot;
            ts_multi.Visible = KW.mt.mtarget;
            ts_speed.Visible = KW.run_fast;
            ts_auto_click.Visible = AC.enabled;
            ts_pick.Visible = KW.a_pick;
            ts_use_slist.Visible = KW.use_slist;
            ts_protect.Visible = KW.rem_dbuf;
            ts_boost.Visible = KW.boost;
            ts_lockon.Visible = KW.lockon;
            ts_heal.Visible = (KW.h_hp || KW.h_mp) && pHack.Enabled;
            ts_bot_rec.Visible = BT.start_rec;


        }


        public void InitDefault()
        {
            extern_dataset = dta_SKills as DataSet;
            tab_ctrl.ReadTabControl(pHack);

            // Access Control

            if (EN.Me)
            {
                MN.pHack = EN.Me;
                EN.cClient = EN.Me;
                EN.CheckAccount = !EN.Me;
                EN.OpenBooster = EN.Me;
                EN.Extreme = EN.Me;
                EN.Bot = EN.Me;
                EN.Multi_Target = EN.Me;
                EN.NPC = EN.Me;
                EN.Items = EN.Me;
                EN.ArmorHack = EN.Me;
                EN.WeapMod = EN.Me;
                EN.ex = new DateTime(2018, 5, 28);
                //cntr_overide();
            }
            // Enab Main

            // Address
            //CA.cBase = 0x007198BC;

            /*
            CA.frz.c_fr1 = new uint[1] { 0X71E };
            CA.frz.c_fr2 = new uint[1] { 0X77E };
            CA.frz.c_E8d = new uint[1] { 0x71E };
            CA.frz.c_mov = new uint[1] { 0X7DC }; 
            CA.c_E8 =  0x70E ;
            */
            /*
            CA.c_pvp = new uint[1] { 0x77A };
            CA.c_pvp2 = new uint[1] { 0x794 };
            CA.Next_Target = new uint[1] { 0x796 };
            CA.Next_Action = new uint[1] { 0x794 };
            CA.Next_Flag = new uint[1] { 0x798 };
            CA.Next_X = new uint[1] { 0x788 };
            CA.Next_Y = new uint[1] { 0x78C };
            CA.c_skill = new uint[1] { 0X74C };
            CA.c_source_skill = new uint[1] { 0X74A };
            CA.cInf.run_off = new uint[1] { 0x7D7 };
            CA.cInf.loc = new uint[1] { 0x634 };
            */
            CA.isDebuggerPresent = false;
            // Change when game exe name is diffent
            CA.dbg = false; // check debbugger
            CA.game_name = "KhanClient";
            CA.cInf = new cinf();
            CA.frz = new freez();
            CA.cInf.d_pick = new dead_pick();
            CA.cInf.buff = new dbuf();

            t_scan.tab = tab_NPC_Scan;
            CA.cInf.target_pool = 0x0183E3F0;
            //CA.cInf.target_pool = 0x0183EFA8; // 0x0183E3F0;
            CA.char_base = (IntPtr)0;
            CA.cInf.My_Class = 0x0079C461;

            CA.cInf.pot_hp = 0x2F5558;
            CA.cInf.pot_mp = 0x2F5568;
            CA.cInf.c_range = 3;
            CA.cInf.perHP = 50;
            CA.cInf.perMP = 30;
            CA.cInf.perEXP = 50;
            CA.cInf.current_target = 0;

            // Buff

            // Key Watch
            KW.base_dir = AppDomain.CurrentDomain.BaseDirectory + "extn\\";
            KW.bot = false;
            KW.lclick = false;
            KW.rclick = false;
            KW.use_skill = false;
            KW.use_slist = false;
            KW.use_buff = false;
            KW.no_anim = false;
            KW.start_force = false;
            KW.lockon = false;
            KW.LastClick = 0;
            KW.a_pick = false;
            KW.h_hp = true;
            KW.h_mp = true;
            KW.use_gcode = false;
            KW.nc_skill = 0;
            KW.mt.mtarget = false;
            KW.mt.style1 = false;
            KW.mt.style2 = false;
            KW.npc = false;
            KW.items = false;
            KW.run_fast = false;
            KW.fast_dbuff = false;
            KW.dead_pick = false;
            KW.lock_pvp = false;
            KW.no_as = false;
            KW.slow_immunity = false;
            // Auto Click

            /**** Bot Ini *****/
            BT.Mob = new MobList();
            BT.Mob.hit_count = 5;
            BT.bot_on = false;
            BT.start_rec = false;
            BT.new_rec = false;
            BT.rand_walk = false;
            BT.map_x = 0;
            BT.map_y = 0;
            BT.cur_x = 0;
            BT.cur_y = 0;
            BT.me_x = 0;
            BT.me_y = 0;
            BT.tar_x = 0;
            BT.tar_y = 0;
            BT.npick = 5;
            BT.lst_items = new ListView();
            BT.lst_items.Columns.Add("Id");
            BT.lst_items.Columns.Add("Name");
            BT.lst_items.View = View.Details;
            BT.lst_loot_items = new ListView();
            BT.lst_loot_items.Columns.Add("Id");
            BT.lst_loot_items.Columns.Add("Name");
            BT.lst_loot_items.View = View.Details;
            BT.lst_route = new ListView();
            BT.lst_route.Columns.Add("State");
            BT.lst_route.Columns.Add("X");
            BT.lst_route.Columns.Add("Y");
            BT.lst_route.Columns.Add("acc");
            BT.lst_route.Columns.Add("F_X");
            BT.lst_route.Columns.Add("F_Y");
            BT.lst_route.View = View.Details;


            // Controll Access
            //u.an = 20376;
            //u.un = "6uTDyH/V0xKNx+1ldfjunw==";
            // Piccaboo
            u.an = 18552;
            u.un = "U3kejXVbKzAsfmqE+DvhwQ==";
            // Miko Xed
            //u.an = 42124;
            //u.un = "nIKBQMui/qoeBqTaIzIGdg==";
            //u.an = 70308;
            //u.un = "hFd6tsKrcskaDidxGkqmPA==";
            fname = KW.base_dir + "definition.tfc";
            oname = KW.base_dir + "others.tfc";
            if (File.Exists(fname))
            {
                Cryptography.ReadXml(dta_SKills, fname, "dmxtreme", "Z3roCo0!", false);
                if (EN.Me)
                    AdminHK();
                //else
                //if (tab_khan_items.Rows.Count < 1)
                //       Application.Exit();
                //TableToListView_Loot(tab_loot_list, BT.lst_loot_items, lst_loot);
            }
            else
            {
                if (!EN.Me)
                    Application.Exit();
                else
                {
                    // NPC List
                    dta_SKills.Tables["NPC_List"].Rows.Add("<New>", 0);
                    Init_Keys();
                }
                // Item List 
            }
            //TableToListView(tab_khan_items, lst_khan_item);
            tbl_Item.Clear();
            dta_SKills.Tables["Items"].Rows.Add("Rosemay Potion", 4679);
            dta_SKills.Tables["Items"].Rows.Add("Basil Potion", 4683);
            //if (EN.Bot)
            //{
            //  dta_SKills.Tables["Items"].Rows.Add("Reinforced Rose Hip", 12591);
            //  dta_SKills.Tables["Items"].Rows.Add("Reinforced Peppermint", 12593);
            dta_SKills.Tables["Items"].Rows.Add("Red Lolipop", 6639);
            dta_SKills.Tables["Items"].Rows.Add("Blue Lolipop", 6640);
            //}
            dta_SKills.Tables["Items"].Rows.Add("Full Red Potion", 1533);
            dta_SKills.Tables["Items"].Rows.Add("Full Element Potion", 1537);
            //dta_SKills.Tables["Items"].Rows.Add("SB Key", 10665);

            if (File.Exists(oname))
            {
                Cryptography.ReadXml(dta_others, oname, "dmxtreme", "Z3roCo0!", false);
                if (tab_acc_prof.Rows.Count < 1 && !EN.Me)
                    Application.Exit();

                TableToListView_Loot(tab_loot_list, BT.lst_loot_items, lst_loot);

            }
            else
                Application.Exit();

        }
        private void ReloadOptions()
        {
            // Option
            txtRange.Text = CA.cInf.c_range.ToString();
            cmb_rng.SelectedIndex = CA.cInf.c_range;
            chk_MT.Checked = KW.mt.mtarget;
            chk_rand_walk.Checked = BT.rand_walk;
            chk_slow_imn.Checked = KW.slow_immunity;
            // Tab Auto
            perHP.Text = CA.cInf.perHP.ToString();
            perMP.Text = CA.cInf.perMP.ToString();
            perExp.Text = CA.cInf.perEXP.ToString();
            chk_HP.Checked = KW.h_hp;
            chk_MP.Checked = KW.h_hp;
            //chk_EXP.Checked = KW.h_exp;
            chk_aClick.Checked = AC.enabled;
            chk_aClickDC.Checked = AC.doubleclick;
            chk_aClickC.Checked = AC.continues;
            chk_no_as.Checked = KW.no_as;
            // Tab Skill
            chk_uskill.Checked = KW.use_slist;
            chk_gcode.Checked = KW.use_gcode;

            // Delay Tab
            txt_aClick_DC_Delay.Text = AC.dblclick_delay.ToString();
            txt_aClickC_Delay1.Text = AC.click_delay1.ToString();
            txt_aClickC_Delay2.Text = AC.click_delay2.ToString();
            txt_buf_delay.Text = AC.buf_delay.ToString();
            txt_pot_delay.Text = AC.pot_delay.ToString();
            txt_hk.Text = AC.hk_delay.ToString();
            txt_m_timeout.Text = AC.m_timeout.ToString();
            txt_m_w.Text = AC.m_counter.ToString();

        }
        private void FindPLock_DoWork(object sender, DoWorkEventArgs e)
        {
            bool search = true;
            while (search == true)
            {
                int pId = 0;
                if (toolz.GetProcName(CA.game_name, ref pId))
                {
                    if (pId > 0)
                    {
                        ProcessSharp process = new ProcessSharp(System.Diagnostics.Process.GetProcessById(pId), Process.NET.Memory.MemoryType.Local);

                        if (process != null)
                        {
                            k = new ProcessSharp(pId, Process.NET.Memory.MemoryType.Local);
                            k.Memory = new ExternalProcessMemory(k.Handle);


                            CA.MainBase = k.ModuleFactory.MainModule.BaseAddress;
                            CA.char_base = k[CA.MainBase].Read<IntPtr>(0x01450614);
                            CA.cBase = k[CA.MainBase].Read<IntPtr>(0x003198BC);
                            CA.add_pot = k[CA.MainBase].Read<IntPtr>(0x0145D150);
                            CA.MobBase = k[CA.MainBase].Read<IntPtr>(0x00319D48);
                            kPid = k.Native.Id;

                            /** temp /
                            CA.cBase = 0x007198BC;
                            CA.MainBase = CA.cBase - 0x003198BC;
                            /********/
                            ReloadOptions();
                            GetUserDetail();
                            cntr_overide();
                            if (EN.Me)
                            {
                                btn_temp.Enabled = EN.Me;
                                pHack.Enabled = true;
                            }
                            else
                            {
                                if (EN.Expr)
                                    checker.RunWorkerAsync();
                            }
                            frmKW.RunWorkerAsync();
                            Load_Hk();
                            //cntr_overide();
                            GetData.RunWorkerAsync();
                            bg_hk.RunWorkerAsync();

                            // Fill Khan Items

                            if (tab_khan_items.Rows.Count < 1)
                            {
                                Refresh_Items();
                                //TableToListView_Loot(tab_loot_list, BT.lst_loot_items, lst_loot);
                            }
                        }
                        kPid = pId;
                        search = false;
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            app_exit = true;
            FindPLock.CancelAsync();
            FindPLock.Dispose();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void Attack(int dlay = 850)
        {
            if (KW.bot_hit || (CA.cInf.current_target != 0 && CA.cInf.current_target != CA.cInf.MyId && !(KW.lockon && (Keyboard.IsKeyDown(Keys.RButton) || Keyboard.IsKeyDown(Keys.LButton)))))
            {
                k[CA.cBase].Write<int>(CA.c_E8, 1000);
                Thread.Sleep(AC.cut_delay);
                if (KW.use_skill || KW.use_slist)
                    k[CA.cBase].Write<int>(CA.c_skill, CA.u_skill);

                if (KW.no_anim)
                {
                    k[CA.cBase].Write<int>(CA.frz.c_fr1, 0);
                    k[CA.cBase].Write<int>(CA.frz.c_fr2, 1);
                    k[CA.cBase].Write<double>(CA.frz.c_E8d, 5.0);
                    k[CA.cBase].Write<int>(CA.frz.c_mov, 1065680994);
                }
                if (KW.no_as && CA.cInf.MyId != CA.cInf.current_target)
                {
                    k[CA.cBase].Write<short>(CA.cInf.Targ1, (short)CA.cInf.current_target);
                    k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.current_target);
                    k[CA.cBase].Write<short>(CA.cInf.Targ_Act, (short)2);
                    k[CA.cBase].Write<short>(CA.c_pvp, 1200);
                }
                Thread.Sleep(dlay);
                //k[CA.cBase].Write<short>(CA.c_pvp, 1200);
                k[CA.cBase].Write<int>(CA.c_E8, 1000);
            }
        }

        private void Buff()
        {
            k[CA.cBase].Write<int>(CA.c_E8, 1000);
            if (KW.fast_dbuff)
            {
                if (KW.use_skill || KW.use_slist)
                    k[CA.cBase].Write<int>(CA.c_skill, CA.u_skill);

                if (KW.no_anim)
                {
                    k[CA.cBase].Write<int>(CA.frz.c_fr1, 0);
                    k[CA.cBase].Write<int>(CA.frz.c_fr2, 1);
                    k[CA.cBase].Write<double>(CA.frz.c_E8d, 5.0);
                    k[CA.cBase].Write<int>(CA.frz.c_mov, 1065680994);
                }
                if (KW.no_as && CA.cInf.MyId != CA.cInf.current_target)
                {
                    int cur_skill = 0X74A;
                    int new_skill = 0x74C;
                    int c_skill = 0X74C;
                    k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.MyId);
                    k[CA.cBase].Write<short>(CA.cInf.Targ_Act, (short)2);
                    k[CA.cBase].Write<int>(c_skill, CA.u_skill);
                    k[CA.cBase].Write<int>(cur_skill, CA.u_skill);
                    k[CA.cBase].Write<int>(new_skill, CA.u_skill);
                    k[CA.cBase].Write<short>(CA.c_pvp, 1800);
                }
                Thread.Sleep(AC.buf_delay);
                k[CA.cBase].Write<short>(CA.c_pvp, 1100);
                k[CA.cBase].Write<int>(CA.c_E8, 1000);
            }
        }

        private void Run()
        {
            if (KW.run_fast)
            {

                k[CA.cBase].Write<int>(CA.frz.c_fr1, 0);
                k[CA.cBase].Write<int>(CA.frz.c_fr2, 1);
                k[CA.cBase].Write<double>(CA.frz.c_E8d, 5.0);
                k[CA.cBase].Write<int>(CA.frz.c_mov, 1065680994);
                k[CA.cBase].Write<int>(CA.c_E8, 1000);
            }
        }

        private void DoCut_DoWork(object sender, DoWorkEventArgs e)
        {
            int act = 0;
            AC.cut_delay = 0;
            int cur_skill = 0;
            while (app_exit == false && KW.start_force)
            {
                if (!KW.bot)
                {
                    if (KW.lockon)
                    {
                        KW.lclick = Keyboard.IsKeyDown(Keys.LButton);
                        KW.rclick = Keyboard.IsKeyDown(Keys.RButton);
                    }
                    else
                    {
                        int ttype = Convert.ToInt32(k.Memory.Read<byte>(AC.target_type));//khan.ReadByte(AC.target_type, (uint)1)[0];
                        if (KW.use_skill || KW.use_slist)
                            KW.rclick = Keyboard.IsKeyDown(Keys.RButton) && ttype != 255;
                        else
                            KW.lclick = Keyboard.IsKeyDown(Keys.LButton) && ttype != 255;
                    }
                    if (KW.use_slist || KW.use_skill)
                        KW.lclick = false;
                }

                act = k[CA.cBase].Read<short>(CA.c_E8);
                if (act == 1100 && KW.run_fast)
                {
                    k[CA.cBase].Write<int>(CA.frz.c_fr1, 0);
                    k[CA.cBase].Write<int>(CA.frz.c_fr2, 1);
                    k[CA.cBase].Write<double>(CA.frz.c_E8d, 5.0);
                    k[CA.cBase].Write<int>(CA.frz.c_mov, 1065680994);
                }
                if (KW.use_slist && KW.nc_skill > 0)
                {
                    if (cur_skill >= KW.nc_skill)
                        cur_skill = 0;

                    CA.u_skill = CA.cast_skill[cur_skill].code;
                    CA.u_skill_range = CA.cast_skill[cur_skill].range;
                    AC.cut_delay = CA.cast_skill[cur_skill].delay;

                    cur_skill++;
                }
                else
                {
                    CA.u_skill = k[CA.cBase].Read<short>(CA.c_source_skill);
                }

                int targ = k[CA.cBase].Read<short>(CA.cInf.Targ2);
                if (targ != 0)
                    CA.cInf.current_target = targ;

                if (KW.lclick || KW.rclick || KW.lockon)
                {
                    if (!KW.bot && !KW.mt.mtarget)
                    {
                        act = k[CA.cBase].Read<short>(CA.c_E8);
                        switch (act)
                        {
                            case 1100:
                                Run();
                                break;
                            case 1200:
                                Attack(AC.cut_delay);
                                break;
                            case 1800:
                                Buff();
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (!KW.lockon /*&& !KW.mt.mtarget*/ && !KW.bot)
                {
                    KW.lclick = false;
                    KW.rclick = false;
                }

                if (k.Native.HasExited)
                    Application.Exit();

                Thread.Sleep(1);
            }
        }
        private void freez()
        {
            k[CA.cBase].Write<int>(CA.frz.c_fr1, 0);
            k[CA.cBase].Write<int>(CA.frz.c_fr2, 1);
            k[CA.cBase].Write<double>(CA.frz.c_E8d, 5.0);
            k[CA.cBase].Write<int>(CA.frz.c_mov, 1065680994);
        }
        /*
        private void DoCut_DoWork1(object sender, DoWorkEventArgs e)
        {

            int cur_iskill = 0;
            int cut_delay = 1;
            // int cc = 0;
            while (app_exit == false)
            {
                int act = 0;
                if (kPid > 0)
                {

                    int ttype = k[CA.cBase].Read<byte>(AC.target_type); 
                    if (!KW.mt.mtarget)
                    {
                        KW.lclick = false;
                        KW.rclick = false;
                        KW.stop = false;
                        if (ttype == 1)
                        {
                            KW.lclick = Keyboard.IsKeyDown(Keys.LButton);
                            KW.rclick = Keyboard.IsKeyDown(Keys.RButton);
                        }

                    }
                    KW.s_cut = false;
                    cut_delay = 1;
                    if (act == 1100 && KW.run_fast)
                    {
                        freez();
                    }
                    if ((/*!KW.no_as || */
        /*KW.lclick || KW.rclick || KW.lockon || BT.play_temp || (KW.fast_dbuff && act == 1800) || (KW.fast_dbuff && act == 1900)) && KW.start_force && !KW.stop)
                    {
                        KW.s_cut = true;
                        if (KW.s_cut)
                        {
                            if (!KW.use_slist)
                            {
                                if (KW.use_skill)
                                {
                                    int rskill = khan.ReadMemory2Byte(CA.cBase, CA.c_source_skill, 1);
                                    if (rskill > -1)
                                        CA.u_skill = rskill;
                                }
                                else
                                    CA.u_skill = -1;
                            }
                            else
                            {
                                CA.u_skill = CA.cast_skill[cur_iskill].code;
                                cut_delay = CA.cast_skill[cur_iskill].delay;

                                cur_iskill++;
                                if (cur_iskill >= KW.nc_skill)
                                    cur_iskill = 0; // Reset

                                KW.LastClick = 2;
                            }

                            if (act == 1800 || act == 1200 || act == 1900)
                            {
                                short cut_target = khan.ReadMemory2Byte(CA.cBase, CA.cInf.Targ2, 1);
                                short next_target = khan.ReadMemory2Byte(CA.cBase, CA.Next_Target, 1);
                                 
                                k[CA.cBase].Write<int>(CA.c_E8, 1000);
                                // Freez Animation
                                if (KW.no_anim && (act == 1200))
                                {
                                    freez();
                                }
                                k[CA.cBase].Write<int>(CA.c_skill, CA.u_skill); 
                                if (act == 1200 && KW.no_as)
                                {
                                    if (cut_target == CA.cInf.MyId)
                                        cut_target = (short)CA.cInf.current_target;

                                    if (KW.mt.mtarget || BT.play_temp)
                                    {
                                        k[CA.cBase].Write<short>(CA.cInf.Targ1, (short)CA.cInf.current_target);
                                        k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.current_target); 
                                    }
                                    else
                                    {
                                        k[CA.cBase].Write<short>(CA.cInf.Targ2, cut_target); 
                                    }
                                    k[CA.cBase].Write<short>(CA.cInf.Targ_Act, (short)2);
                                    k[CA.cBase].Write<int>(CA.Next_Flag, 2);
                                    k[CA.cBase].Write<short>(CA.Next_Action, 1100);
                                    k[CA.cBase].Write<short>(CA.c_pvp, 1200);
                                     
                                }
                                if (act == 1800 && KW.fast_dbuff)
                                {
                                    k[CA.cBase].Write<short>(CA.c_pvp, 1200);

                                    khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ1, 1, (short)CA.cInf.cut_target);
                                    khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ2, 1, (short)CA.cInf.cut_target);
                                    khan.WriteMemory2Byte(CA.cBase, CA.c_pvp, 1, 1800);
                                }
                                if (act == 1900 && KW.fast_dbuff)
                                    khan.WriteMemory2Byte(CA.cBase, CA.c_pvp, 1, 1900);

                                khan.WriteMemoryInt(CA.cBase, CA.c_skill, 1, CA.u_skill);

                                // Freez Animation 
                                if (KW.no_anim && (act == 1200))
                                {
                                    freez();
                                }
                            }
                            if (act == 1100 && KW.run_fast)
                            {
                                freez();
                            }
                        }
                    }
                }
                switch (act)
                {
                    case 1200:
                        Thread.Sleep(cut_delay);
                        //toolz.Delay(cut_delay);
                        break;
                    case 1800:
                    case 1900:
                        Thread.Sleep(AC.buf_delay);
                        //toolz.Delay(AC.buf_delay);
                        break;
                    default:
                        Thread.Sleep(1);
                        //toolz.Delay(1);
                        break;
                }
            }
        }
    */
        private void btnExit_Click(object sender, EventArgs e)
        {
            KW.start_force = false;
            app_exit = true;
            Application.Exit();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Are you sure you want to reload original setting?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                tab_NPC_Scan.Clear();
                Cryptography.ReadXml(dta_SKills, fname, "dmxtreme", "Z3roCo0!", false);
            }
        }

        private void GetSkill(int srow, out int code, out int delay, out int exec, out int atk)
        {
            code = Convert.ToInt32(dta_SKills.Tables["Skill_List"].Rows[srow]["Code"].ToString());
            delay = Convert.ToInt32(dta_SKills.Tables["Skill_List"].Rows[srow]["Delay"].ToString());
            exec = Convert.ToInt32(dta_SKills.Tables["Skill_List"].Rows[srow]["Exec#"].ToString());
            string st = dta_SKills.Tables["Skill_List"].Rows[srow]["Atk"].ToString();
            if (st == "True")
                atk = 1;
            else
                atk = 0;
        }

        private cskill[] GetSkills(string Type)
        {

            var slist = from m in tab_Skills.AsEnumerable()
                        where m.Field<string>("Type") == Type
                        && m.Field<Boolean>("Atk") == true
                        select m;

            /*
var skl = tab_Skills.AsEnumerable().ToList();
skl.
var slist = from m in skl where m.ty == id select m;
*/
            cskill[] ret = new cskill[slist.Count()];
            int idx = 0;
            foreach (var sk in slist.ToList())
            {

                ret[idx].code = sk.Field<short>("Code");
                ret[idx].delay = sk.Field<short>("Delay");
                ret[idx].exec = sk.Field<byte>("Exec#");
                ret[idx].range = Math.Round(sk.Field<short>("Range") * CA.pace, 10);

                idx++;
            }
            return ret;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Are you sure you want to save current setting?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                int nrec = tab_char_skill.Rows.Count;
                int[] del = new int[nrec];
                int c_rec = 0;

                List<DataRow> rowsToDelete = new List<DataRow>();

                foreach (DataRow row in tab_char_skill.Rows)
                {
                    if (row["char"].ToString() == CA.cInf.name)
                    {
                        rowsToDelete.Add(row);
                    }
                }

                foreach (DataRow row in rowsToDelete)
                {
                    tab_char_skill.Rows.Remove(row);
                }

                foreach (DataRow tb in tab_Skills.Rows)
                {
                    DataRow ndr = tab_char_skill.NewRow();
                    ndr["char"] = CA.cInf.name;
                    ndr["Skill"] = tb.Field<string>("Skill");
                    ndr["Code"] = Convert.ToInt16(tb["Code"].ToString());
                    ndr["Delay"] = Convert.ToInt16(tb["Delay"].ToString());
                    ndr["Exec#"] = Convert.ToByte(tb["Exec#"].ToString());
                    ndr["Atk"] = Convert.ToBoolean(tb["Atk"].ToString());
                    ndr["Range"] = Convert.ToInt16(tb["Range"].ToString());
                    ndr["Type"] = tb.Field<string>("Type");
                    tab_char_skill.Rows.Add(ndr);
                    //tab_char_skill.Rows.Add(CA.cInf.name, tb["Skill"].ToString(), tb["Code"].ToString(), tb["Delay"].ToString(), tb["Exec#"].ToString(), tb["Atk"].ToString(), tb["Range"].ToString(), tb["Type"].ToString());
                }
                tab_NPC_Scan.Clear();
                tab_khan_items.Clear();
                SaveOptions();
                Cryptography.WriteXml(dta_SKills, fname, "dmxtreme", "Z3roCo0!", false);

                tab_loot_list.Clear();
                FromListView(tab_loot_list, BT.lst_loot_items);
                Cryptography.WriteXml(dta_others, oname, "dmxtreme", "Z3roCo0!", false);
                Refresh_Items();
            }
        }

        private void chkDebug()
        {
            if (CA.dbg)
            {
                if (toolz.CheckRemoteDebuggerPresent(k.Native.Handle, ref CA.isDebuggerPresent))
                {
                    if (CA.isDebuggerPresent)
                    {
                        KW.start_force = false;
                        app_exit = true;
                        Application.Exit();
                    }
                }
            }
        }


        private void GetData_DoWork(object sender, DoWorkEventArgs e)
        {
            while (app_exit == false)
            {
                bool isSameName = false;
                chkDebug();
                KW.KL = toolz.GetWindPos(k.Native.Handle);
                CA.cInf.lvl = k[CA.cBase].Read<short>(CA.cInf.LVL); // khan.ReadMemory2Byte(CA.cBase, CA.cInf.LVL, 1);
                label_level.Text = CA.cInf.lvl.ToString();
                label_name.Text = CA.cInf.name;
                string cname = Readstr_off(CA.cBase, CA.cInf.Name, 14); // k[CA.cBase].Read<string>(CA.cInf.Name); // khan.ReadMemoryString(CA.cBase, CA.cInf.Name, 1, 14);
                isSameName = (CA.cInf.name == cname);
                if (k.Native.HasExited)
                    Application.Exit();

                CA.cInf.name = cname;
                if (!isSameName)
                    LoadOptions();

                //txtName.Text = CA.cInf.name;
                CA.cInf.MyId = k[CA.cBase].Read<int>(CA.cInf.MyOff); // khan.ReadMemoryInt(CA.cBase, CA.cInf.MyOff, 1);
                CA.cInf.maxHP = k[CA.cBase].Read<int>(CA.cInf.OHP); //khan.ReadMemoryInt(CA.cBase, CA.cInf.OHP, 1);
                CA.cInf.maxMP = k[CA.cBase].Read<int>(CA.cInf.OMP); //khan.ReadMemoryInt(CA.cBase, CA.cInf.OMP, 1);
                CA.cInf.curHP = k[CA.cBase].Read<int>(CA.cInf.CHP); //khan.ReadMemoryInt(CA.cBase, CA.cInf.CHP, 1);
                CA.cInf.curMP = k[CA.cBase].Read<int>(CA.cInf.CMP); //khan.ReadMemoryInt(CA.cBase, CA.cInf.CMP, 1);
                CA.cInf._m_exp = k[CA.cBase].Read<int>(CA.cInf.MaxEXP); //khan.ReadMemoryInt(CA.cBase, CA.cInf.MaxEXP, 1);
                CA.cInf._c_exp = k[CA.cBase].Read<int>(CA.cInf.CurEXP); //khan.ReadMemoryInt(CA.cBase, CA.cInf.CurEXP, 1);
                toolz.GetInt(txt_mob_type, ref BT.mob_type);
                //CA.cInf.perEXP = Convert.ToInt32(perExp.Text);
                toolz.GetInt(perExp, ref CA.cInf.perEXP);
                //CA.cInf.perHP = Convert.ToInt32(perHP.Text);
                toolz.GetInt(perHP, ref CA.cInf.perHP);
                //CA.cInf.perMP = Convert.ToInt32(perMP.Text);
                toolz.GetInt(perMP, ref CA.cInf.perMP);

                //CA.cInf.ld_bot_max = Convert.ToInt32(txt_exp_max.Text.ToString());
                toolz.GetInt(txt_exp_max, ref CA.cInf.ld_bot_max);
                //CA.cInf.ld_bot_min = Convert.ToInt32(txt_exp_min.Text.ToString());
                toolz.GetInt(txt_exp_min, ref CA.cInf.ld_bot_min);
                int rng = 0; 
                if (EN.Me)
                    toolz.GetInt(txtRange, ref rng);
                else
                    rng = cmb_rng.SelectedIndex + 1;

                int rext = 0;
                toolz.GetInt(txt_boost, ref rext); ;
                CA.hit_range = Math.Round((double)rng * CA.pace, 10);
                toolz.GetInt(txt_num_mobs, ref KW.num_mobs);
                toolz.GetInt(txt_num_hits, ref KW.num_hits);
                BT.Mob.hit_count = KW.num_mobs;
                if (CA.cInf.maxHP > 0 && CA.cInf.perHP > 0)
                    CA.cInf.healHP = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CA.cInf.curHP) * 100) / Convert.ToDecimal(CA.cInf.maxHP));
                if (CA.cInf.maxMP > 0 && CA.cInf.perMP > 0)
                    CA.cInf.healMP = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CA.cInf.curMP) * 100) / Convert.ToDecimal(CA.cInf.maxMP));

                if (CA.cInf._m_exp > 0 && CA.cInf.perEXP > 0)
                    CA.cInf._w_exp = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CA.cInf._c_exp) * 100) / Convert.ToDecimal(CA.cInf._m_exp));

                lb_exp_p.Text = "EXP " + CA.cInf._w_exp.ToString() + "%";

                _hp.Maximum = CA.cInf.maxHP;
                _mp.Maximum = CA.cInf.maxMP;
                _hp.Value = CA.cInf.curHP;
                _mp.Value = CA.cInf.curMP;
                _exp.Maximum = CA.cInf._m_exp;
                _exp.Value = CA.cInf._c_exp;
                toolz.GetInt(txt_hk, ref AC.hk_delay);
                //dta_SKills.Tables["Skill_List"].DefaultView.RowFilter = "Delay = 2";
                //cntr_overide();
                Thread.Sleep(50);
            }
        }

        public void npc_act()
        {
            int bwriten = 0;
            byte[] w_open = BitConverter.GetBytes(9224);
            byte[] w_close = BitConverter.GetBytes(9225);
            string dta = tab_NPC.Rows[cmb_NPC_List.SelectedIndex]["Addr"].ToString();
            IntPtr addr = (IntPtr)Convert.ToInt32(dta);
            byte[] ret = k.Memory.Read<byte>(addr, 2);// khan.ReadByte(addr, (uint)2);
            int iret = (int)BitConverter.ToInt16(ret, 0);
            if (iret == 9225)
            {
                k.Memory.Write((IntPtr)addr, w_open);
                //k[CA.cBase].Write<short>(CA.c_pvp, 1200);
                //khan.WriteMemory((IntPtr)addr, w_open, out bwriten);
            }
            else if (iret == 9224)
            {
                k.Memory.Write((IntPtr)addr, w_close);
                // khan.WriteMemory((IntPtr)addr, w_close, out bwriten);
            }
        }
        private void AddItem()
        {
            short itm = (short)Convert.ToUInt16(tbl_Item.Rows[cmb_Items.SelectedIndex]["Code"].ToString());
            k[CA.add_pot].Write<short>(CA.cInf.Pot, itm);
            //khan.WriteMemory2Byte0(CA.add_pot, CA.cInf.Pot, 1, itm);
        }

        /*
        private void DeadPick()
        {
            uint i_src = CA.MainBase + 0x0039C3E8;
            uint[] i_offset = new uint[2] { 0x10, 0x310 };
            uint[] w_off = new uint[1] { 0x790 };
            uint[] w_act_ext = new uint[1] { 0x794 };
            uint Addr = (uint)khan.FindDmaAddy((int)i_src, i_offset, 2);
            uint w_Addr = (uint)khan.FindDmaAddy((int)CA.cBase, w_off, 1);
            byte[] ret = khan.ReadByte(Addr, 8);
            byte[] t_id = new byte[2] { ret[0], ret[1] };
            byte[] t_par = new byte[2] { ret[4], ret[5] };
            int tg = BitConverter.ToInt16(t_id, 0);
            int par = BitConverter.ToInt16(t_par, 0);
            int bw = 0;
            uint loot_base = CA.MainBase + 0x0143D7DC;
            uint[] loot_offset = new uint[2] { 0xD8, 0x20 };
            int scan_size = 50000;
            uint addr = CA.cInf.loot_address;
            if (addr == 0x0)
                addr = (uint)khan.FindDmaAddy((int)loot_base, loot_offset, 2);
            int npick = 10;
            byte[] type_id;
            int gold = 1581;
            Thread.Sleep(1000);
            AOBScan newscan = new AOBScan((uint)kPid);
            while (npick != 0)
            {
                try
                {
                    byte[] pat = new byte[] { 0x80, 0x0 };
                    int jmp = 14;
                    List<int[]> Loots = newscan.GetGoldId((IntPtr)addr, scan_size, pat, jmp);
                    for (int i = 0; i < Loots.Count; i++)
                    {
                        if (chk_include.Checked)
                        {
                            if ((FindLootItem(BT.lst_loot_items, Loots[i][1].ToString())) || (chk_gold.Checked && Loots[i][1] == gold))
                            {
                                short val = Convert.ToInt16(Loots[i][0].ToString());
                                khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ1, 1, val);
                                khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ2, 1, val);
                                khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ_Act, 1, (short)4);
                                khan.WriteMemory2Byte(CA.cBase, CA.c_pvp, 1, 1300);
                                Thread.Sleep(10);
                            }
                        }
                        if (chk_exclude.Checked)
                        {
                            bool lt = true;
                            if (FindLootItem(BT.lst_loot_items, Loots[i][1].ToString()))
                                lt = false;
                            else
                                if (!chk_gold.Checked && Loots[i][1] == gold)
                                lt = false;
                            if (lt)
                            {
                                short val = Convert.ToInt16(Loots[i][0].ToString());
                                khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ1, 1, val);
                                khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ2, 1, val);
                                khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ_Act, 1, (short)4);
                                khan.WriteMemory2Byte(CA.cBase, CA.c_pvp, 1, 1300);
                                Thread.Sleep(10);
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
                catch
                {
                    break;
                }
                npick--;
            }
        }
        */
        public bool CheckPots()
        {
            bool ret = true;
            int br = 0;
            if (chk_pot_hp.Checked)
            {
                // short pot = BitConverter.ToInt16(khan.ReadMemory((IntPtr)(CA.MainBase + CA.cInf.pot_hp), 2, out br), 0);
                short pot = k[CA.MainBase].Read<short>(CA.cInf.pot_hp);
                if (pot == 0)
                    ret = false;
            }
            if (chk_pot_mp.Checked)
            {
                //short pot = BitConverter.ToInt16(khan.ReadMemory((IntPtr)(CA.MainBase + CA.cInf.pot_mp), 2, out br), 0);
                short pot = k[CA.MainBase].Read<short>(CA.cInf.pot_mp);
                if (pot == 0)
                    ret = false;
            }
            return ret;
        }
        private void frmKW_DoWork(object sender, DoWorkEventArgs e)
        {

            while (app_exit == false)
            {
                chkDebug();
                if (kPid > 0)
                {
                    if (KW.slow_immunity)
                        k[CA.cBase].Write<byte>(CA.cInf.run_off, 1);

                    if (KW.rem_dbuf)
                    {
                        k[CA.cBase].Write<int>(CA.cInf.buff.buf, 0);
                        k[k[CA.MainBase].Read<IntPtr>(CA.cInf.buff.un1)].Write<int>(CA.cInf.buff.off1, 0);
                    }

                    if (KW.use_gcode)
                    {
                        txt_gcode.Text = k[CA.cBase].Read<short>(CA.c_source_skill).ToString();

                    }
                    if (KW.h_hp || KW.h_mp)
                        if (!AutoPot.IsBusy)
                            AutoPot.RunWorkerAsync();

                    if (AC.enabled)
                    {
                        if (!AutoClick.IsBusy)
                            AutoClick.RunWorkerAsync();
                    }

                    if (KW.lock_pvp)
                    {
                        if (toolz.IsSameClient(kPid))
                            WindowsAPI.keybd_event((byte)0x11, 0x9d, 0, 0);
                        else
                            WindowsAPI.keybd_event((byte)0x11, 0x9d, WindowsAPI.KEYEVENTF_KEYUP, 0);

                    }

                    if (KW.use_buff && KW.start_force)
                    {
                        if (!bg_buff.IsBusy)
                            bg_buff.RunWorkerAsync();
                    }
                    KW.h_exp = (chk_stop_heal.Checked || chk_pause_all.Checked);

                    GetUserDetail();
                    if (chk_revive.Checked)
                    {
                        Point P;
                        Color C;
                        if (chk_use_scroll.Checked)
                        {
                            P = BT.scroll.XY;
                            P.X += KW.KL.Left;
                            P.Y += KW.KL.Top;
                            C = BT.scroll.RGB;
                        }
                        else
                        {
                            P = BT.rev.XY;
                            P.X += KW.KL.Left;
                            P.Y += KW.KL.Top;
                            C = BT.rev.RGB;
                        }
                        Color CL = new Color();
                        CL = GetColorAt(P);
                        if (CL == C)
                        {
                            /* -- Not working (fixed)
                            if (chk_dead_pick.Checked)
                            {
                                IntPtr Addr = khan.FindDmaAddy((int)CA.cBase, CA.c_pvp, 1);
                                int br = 0;
                                khan.WriteMemory(Addr, CA.cInf.d_pick.pick_stand, out br);
                                khan.WriteMemoryByte(CA.cBase, CA.cInf.d_pick.pick_flag, 1, (byte)128);
                                khan.WriteMemoryByte(CA.cBase, CA.cInf.d_pick.class_flag, 1, KW.MyClass);
                                Thread.Sleep(300);
                                DeadPick();
                            }
                            */
                            if (CheckPots())
                            {
                                toolz.LeftClick(P.X, P.Y, 20, 100);
                                toolz.LeftClick(P.X, P.Y, 20, 100);
                                int X = KW.KL.Left + (KW.KL.Width / 2);
                                int Y = KW.KL.Top + 50;
                                toolz.SetCursorPos(X, Y);
                            }
                            //if ((BT.state == 1 || BT.state == 2) && chk_ld_path.Checked)
                            BT.ld = false;
                            if (BT.play_temp)
                                CheckLD();
                            /*
                            if (chk_ld_path.Checked)
                            {
                                BT.state = 2;
                                BT.cur_coord = 0;
                            }
                            */
                            //Thread.Sleep(100);

                        }
                    }
                    if (chk_use_knight.Checked)
                    {
                        if (cmb_class_item.SelectedIndex > -1)
                        {
                            byte[] kn = new byte[] { 0x0 };
                            k[CA.cBase].Write<byte>(CA.cInf.d_pick.class_flag, Convert.ToByte(cmb_class_item.SelectedIndex));
                            //khan.WriteMemoryByte(CA.cBase, CA.cInf.d_pick.class_flag, 1, Convert.ToByte(cmb_class_item.SelectedIndex));
                        }
                    }
                    else
                    {
                        //IntPtr ad = khan.FindDmaAddy((int)CA.cBase, CA.cInf.d_pick.class_flag, 1);
                        int br = 0;
                        byte cls = k[CA.cBase].Read<byte>(CA.cInf.d_pick.class_flag);// khan.ReadMemory(ad, 1, out br)[0];
                        if (Convert.ToByte(cls) != KW.MyClass)
                            k[CA.cBase].Write<byte>(CA.cInf.d_pick.class_flag, KW.MyClass);
                        //khan.WriteMemoryByte(CA.cBase, CA.cInf.d_pick.class_flag, 1, KW.MyClass);
                    }
                    if (chk_stats.Checked)
                    {
                        Point P = BT.stat.XY;
                        P.X += KW.KL.Left;
                        P.Y += KW.KL.Top;
                        Color C = BT.stat.RGB;
                        Color CL = new Color();
                        CL = GetColorAt(P);
                        if (CL == C)
                        {
                            toolz.LeftClick(P.X, P.Y, 20, 100);
                            toolz.LeftClick(P.X, P.Y, 20, 100);
                            int X = KW.KL.Left + ((KW.KL.Right - KW.KL.Left) / 2);
                            //int Y = KW.KL.Top + ((KW.KL.Bottom - KW.KL.Top) / 2);
                            int Y = KW.KL.Top + 50;
                            toolz.SetCursorPos(X, Y);
                            //Thread.Sleep(100);
                        }
                    }
                    if (chk_reject_trade.Checked)
                    {
                        Point P = BT.trade.XY;
                        P.X += KW.KL.Left;
                        P.Y += KW.KL.Top;
                        Color C = BT.trade.RGB;
                        Color CL = new Color();
                        CL = GetColorAt(P);
                        if (CL == C)
                        {
                            toolz.LeftClick(P.X, P.Y, 20, 100);
                            toolz.LeftClick(P.X, P.Y, 20, 100);
                            int X = KW.KL.Left + ((KW.KL.Right - KW.KL.Left) / 2);
                            //int Y = KW.KL.Top + ((KW.KL.Bottom - KW.KL.Top) / 2);
                            int Y = KW.KL.Top + 50;
                            toolz.SetCursorPos(X, Y);
                            //Thread.Sleep(100);
                        }
                    }
                    //cntr_overide();
                }
                Thread.Sleep(10);
            }
        }

        private void frmRender_DoWork(object sender, DoWorkEventArgs e)
        {
            while (app_exit == false)
            {
                //chk_NoAnim.Checked = KW.no_anim;
                Thread.Sleep(20);
            }
        }

        private void chk_NoAnim_CheckedChanged(object sender, EventArgs e)
        {
            KW.no_anim = chk_NoAnim.Checked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            KW.start_force = !KW.start_force;
            if (KW.start_force)
            {
                if (!DoCut.IsBusy)
                    DoCut.RunWorkerAsync();
                if (!bg_fetch_mobs.IsBusy)
                    bg_fetch_mobs.RunWorkerAsync();
                if (!RefreshMobs.IsBusy)
                {
                    // Initiate Monster List
                    CA.MonsDead.Clear();
                    CA.MonsHit.Clear();
                    CA.MonsStuck.Clear();
                    CA.MonsList.Clear();
                    CA.MonsScan = new List<MobDetail>();
                    IntPtr madd = CA.MobBase;
                    MobDetail mbn = new MobDetail();
                    mbn.Address = madd;
                    CA.MonsScan.Add(mbn);

                    for (int i = 0; i < 149; i++)
                    {
                        mbn = new MobDetail();
                        madd = (IntPtr)((int)madd + (int)0x3A0);
                        mbn.Address = madd;
                        CA.MonsScan.Add(mbn);
                    }
                    RefreshMobs.RunWorkerAsync();
                }
            }
            tfext.force_ext.SetButton(ref btnStart, "Start", "Pause");
        }

        private void AutoClick_DoWork(object sender, DoWorkEventArgs e)
        {
            //uint c_point = 0;
            while (AC.enabled)
            {
                if (KW.start_force)
                {
                    if (toolz.IsSameClient(kPid))
                    {
                        //int ttype = BitConverter.ToInt32(khan.ReadByte(AC.target_type, (uint)1),0);
                        int ttype = k.Memory.Read<byte>((IntPtr)AC.target_type);// khan.ReadByte(AC.target_type, (uint)1)[0];
                        //uint npoint = (uint)BitConverter.ToDouble(khan.ReadByte(0x0185D0E8, (uint)8),0);
                        /*
                        if (c_point != npoint)
                        {
                            KW.s_cut = false;
                            //Thread.Sleep(50);
                            toolz.Delay(100);
                            KW.s_cut = true;
                            c_point = (uint)npoint;
                        }
                        */

                        //if (KW.use_skill)
                        if (ttype == 1 || ttype == 0)
                        {
                            //TF_Key.KeyDown(0xA3);
                            if (KW.use_skill)
                            {
                                toolz.RightClick(0, 0, AC.click_delay1, AC.click_delay2);
                                KW.rclick = true;
                            }
                            else
                            {
                                toolz.LeftClick(0, 0, AC.click_delay1, AC.click_delay2);
                                KW.lclick = true;
                            }


                            //TF_Key.KeyUp(0xA3);
                            /**************
                            if (KW.LastClick == 1)
                                toolz.LeftClick(0, 0,AC.click_delay1, AC.click_delay2);
                            else if (KW.LastClick == 2)
                                toolz.RightClick(0, 0, AC.click_delay1, AC.click_delay2);
                            ***************/
                        }
                        if (ttype == 3 && KW.a_pick)
                            if (!KW.use_skill)
                                toolz.LeftClick(0, 0, AC.click_delay1, AC.click_delay2);
                    }
                }
                Thread.Sleep(1);
            }
        }
        private void chk_Lock_CheckedChanged(object sender, EventArgs e)
        {
            KW.lockon = chk_Lock.Checked;
        }

        public bool heal_skill()
        {
            bool ret = false;
            if (KW.nc_heal > 0)
            {
                int sperHP = CA.cInf.perHP + ((100 - CA.cInf.perHP) / 2);
                bool sheal_hp = (sperHP > CA.cInf.healHP);
                while (sheal_hp && IsOut() && CharAlive())
                {
                    int cur_skill = 0X74A;
                    int new_skill = 0x74C;
                    int dl = 0;
                    foreach (cskill sk in CA.cast_heal)
                    {
                        k[CA.cBase].Write<int>(CA.cInf.c_E8, 1000);
                        Thread.Sleep(20);
                        k[CA.cBase].Write<short>(CA.cInf.Targ_Act, (short)1);
                        k[CA.cBase].Write<int>(CA.cInf.c_skill, sk.code);
                        k[CA.cBase].Write<int>(cur_skill, sk.code);
                        k[CA.cBase].Write<int>(new_skill, sk.code);
                        k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.MyId);
                        k[CA.cBase].Write<short>(CA.cInf.c_pvp, 1800);
                        dl = sk.delay;
                    }
                    Thread.Sleep(dl);
                    ret = true;
                    sperHP = CA.cInf.perHP + ((100 - CA.cInf.perHP) / 2);
                    sheal_hp = (sperHP > CA.cInf.healHP);
                    Thread.Sleep(1);
                }
            }
            return ret;
        }
        private void AutoPot_DoWork(object sender, DoWorkEventArgs e)
        {

            while (KW.h_hp || KW.h_mp)
            {
                if (KW.start_force)
                    heal_skill();
                if (toolz.IsSameClient(kPid) && KW.start_force)
                {
                    bool heal_hp = false;
                    bool heal_mp = false;
                    if (KW.h_hp)
                    {
                        heal_hp = (CA.cInf.perHP > CA.cInf.healHP);
                        if (heal_hp)
                        {
                            TF_Key.PressKey('q', 50); // HP 
                        }
                    }
                    if (KW.h_mp)
                    {
                        heal_mp = (CA.cInf.perMP > CA.cInf.healMP);
                        if (heal_mp)
                        {
                            TF_Key.PressKey('w', 50); // MP 
                        }
                    }
                    if (heal_hp || heal_mp)
                    {
                        Thread.Sleep(AC.pot_delay);
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void chk_aClick_CheckedChanged(object sender, EventArgs e)
        {
            AC.enabled = chk_aClick.Checked;
        }

        private void chk_Pick_CheckedChanged(object sender, EventArgs e)
        {
            KW.a_pick = chk_Pick.Checked;
            chk_loot_enable.Checked = false;
            if (KW.a_pick)
                if (!AutoPick.IsBusy)
                    AutoPick.RunWorkerAsync();
        }

        private void chkHP_CheckedChanged(object sender, EventArgs e)
        {
            KW.h_hp = chk_HP.Checked;
        }

        private void chk_MP_CheckedChanged(object sender, EventArgs e)
        {
            KW.h_mp = chk_MP.Checked;
        }

        private void chk_aClickC_CheckedChanged(object sender, EventArgs e)
        {
            AC.continues = chk_aClickC.Checked;
        }

        private void chk_aClickDC_CheckedChanged(object sender, EventArgs e)
        {
            AC.doubleclick = chk_aClickDC.Checked;
        }

        // private void chk_EXP_CheckedChanged(object sender, EventArgs e)
        // {
        //     KW.h_exp = chk_EXP.Checked;
        // }

        private byte[] ReadTarget(int addr, out int Targ, out int TargX, out int TargY, out int TargT)
        {
            byte[] value = k.Memory.Read<byte>((IntPtr)addr, 40); //,khan.ReadByte(addr, (uint)40);
            byte[] cnv = new byte[2] { 0x0, 0x0 };
            TargT = Convert.ToInt32(value[1]);
            cnv[0] = value[4]; //Convert.ToInt32(value[5]);
            cnv[1] = value[5]; //= Convert.ToInt32(value[4]);
            Targ = (int)BitConverter.ToInt16(cnv, 0);
            cnv[0] = value[6];
            cnv[1] = value[7];
            TargX = (int)BitConverter.ToInt16(cnv, 0);
            cnv[0] = value[8];
            cnv[1] = value[9];
            TargY = (int)BitConverter.ToInt16(cnv, 0);
            // Temporary 
            return value;
        }

        private byte[] ReadTarget2(int addr, out int Targ, out int TargX, out int TargY, out int TargT, out float tx, out float ty)
        {
            byte[] value = k.Memory.Read<byte>((IntPtr)addr, 40); //khan.ReadByte(addr, (uint)40);
            byte[] cnv = new byte[2] { 0x0, 0x0 };

            //flag = (int)value[0];
            //(int)BitConverter.ToInt16(value, 0);
            TargT = (int)BitConverter.ToInt16(value, 10);
            Targ = (int)BitConverter.ToInt16(value, 3);
            TargX = (int)BitConverter.ToInt16(value, 5);
            TargY = (int)BitConverter.ToInt16(value, 7);
            byte[] fcx = new byte[4];
            byte[] fcy = new byte[4];
            Array.Copy(value, 10, fcx, 0, 4);
            Array.Copy(value, 14, fcy, 0, 4);
            tx = BitConverter.ToSingle(fcx, 0);
            ty = BitConverter.ToSingle(fcy, 0);

            // Temporary 
            return value;
        }

        public IntPtr GetAddress(IntPtr intPtr, int[] off)
        {
            IntPtr ret = ret = k[intPtr].Read<IntPtr>(off[0]);
            int cnt = 0;
            foreach (var o in off)
            {
                if (cnt > 0)
                    ret = IntPtr.Add(ret, o);
                cnt++;
            }
            //ret = k[ret].Read<IntPtr>(o);
            return ret;
        }
        private bool ReadTarget1(uint addr, out int Targ, out float TargX, out float TargY, out int TargT)
        {
            IntPtr sbase = k[CA.MainBase].Read<IntPtr>(0x0039C3E8);
            int[] off = new int[2] { 0x10, 0x2F };
            int[] fetch = new int[4] { (int)0x8, (int)0x0C, (int)0x14, (int)0x18 };
            IntPtr AddrScan = GetAddress(sbase, off);// khan.FindDmaAddy((int)sbase, off, 2);
                                                     // IntPtr AddrScan = k.Memory.Read<byte>
            var value = k.Memory.Read<byte>(AddrScan, 40); // khan.ReadByte((uint)AddrScan, (uint)40);
            byte[] cx = new byte[4];
            byte[] cy = new byte[4];
            byte[] tar = new byte[2];
            byte[] type = new byte[1];
            Array.Copy(value, fetch[0], cx, 0, 4);
            Array.Copy(value, fetch[1], cy, 0, 4);
            Array.Copy(value, fetch[2], tar, 0, 2);
            Array.Copy(value, fetch[3], type, 0, 1);
            TargX = BitConverter.ToSingle(cx, 0);
            TargY = BitConverter.ToSingle(cy, 0);
            Targ = BitConverter.ToInt16(tar, 0);
            TargT = (int)type[0];
            if (Targ > 0 && Targ < 5000)
                return true;
            else
                return false;
        }
        private bool InRangeX(int cx, int cy, int rad, int tx, int ty)
        {
            bool inside = false;
            int[] mx = new int[3] { 0, 0, 0 };
            int[] my = new int[3] { 0, 0, 0 };

            mx[1] = cx - rad;
            mx[2] = cx + rad;

            my[1] = cy - rad;
            my[2] = cy + rad;
            if (tx >= mx[1] && tx <= mx[2])
                if (ty >= my[1] && ty <= my[2])
                    inside = true;
            return inside;
        }

        private bool IsHitable()
        {
            int act = k[CA.cBase].Read<short>(CA.c_E8); // khan.ReadMemory2Byte(CA.cBase, CA.c_E8, 1);
            bool ret = (act == 1200 || act == 1000);
            return ret;
        }
        public void mAdd(int Id, int char_x, int x, int char_y, int y, int rx, int ry, byte[] rd)
        {
            bool fnd = false;
            int ind = -1;
            foreach (DataRow dr in AOE.Rows)
            {
                ind++;
                if (dr["Target"].ToString() == Id.ToString())
                {
                    dr.BeginEdit();
                    dr["Target"] = Id;
                    dr["MeX"] = char_x;
                    dr["X"] = x;
                    dr["RX"] = rx;
                    dr["MeY"] = char_y;
                    dr["Y"] = y;
                    dr["RY"] = ry;
                    dr["RD"] = BitConverter.ToString(rd);
                    dr.EndEdit();
                    fnd = true;
                }
            }
            if (!fnd)
                AOE.Rows.Add(Id, char_x, x, char_y, y, rx, ry, BitConverter.ToString(rd));
        }
        private void chk_uskill_CheckedChanged(object sender, EventArgs e)
        {
            KW.use_slist = chk_uskill.Checked;
            if (KW.use_slist)
            {
                CA.cast_skill = null;
                CA.cast_skill = GetSkills("Attack");
                KW.nc_skill = CA.cast_skill.Count();
            }
        }

        private void chk_uskill_CheckedChanged_old(object sender, EventArgs e)
        {
            KW.use_slist = chk_uskill.Checked;
            int t_exec = 0;
            int b_exec = 0;
            int n_exec = 0;
            int bn_exec = 0;
            if (KW.use_slist)
            {
                int nrows = dta_SKills.Tables["Skill_List"].Rows.Count;
                cskill[] s = new cskill[nrows];
                for (int i = 0; i < nrows; i++)
                {
                    GetSkill(i, out s[i].code, out s[i].delay, out s[i].exec, out s[i].atk);
                    if (s[i].atk == 1)
                        n_exec += s[i].exec;

                }

                CA.cast_skill = null;
                CA.cast_skill = new cskill[n_exec];
                t_exec = 0;
                b_exec = 0;
                KW.nc_skill = n_exec;
                KW.nc_buff = bn_exec;
                KW.nc_buff = 0;

                for (int i = 0; i < nrows; i++)
                {
                    if (s[i].atk == 1)
                    {
                        for (int x = 0; x < s[i].exec; x++)
                        {

                            CA.cast_skill[t_exec].code = s[i].code;
                            CA.cast_skill[t_exec].delay = s[i].delay;
                            CA.cast_skill[t_exec].range = s[i].range;
                            t_exec++;
                        }
                    }
                }

            }
            t_exec++;
        }
        private void chk_gcode_CheckedChanged(object sender, EventArgs e)
        {
            KW.use_gcode = chk_gcode.Checked;
        }

        private void txt_pot_delay_TextChanged(object sender, EventArgs e)
        {
            //AC.pot_delay = Convert.ToInt32(txt_pot_delay.Text);
            toolz.GetInt(txt_pot_delay, ref AC.pot_delay);
        }

        private void txt_aClick_DC_Delay_TextChanged(object sender, EventArgs e)
        {
            //AC.dblclick_delay = Convert.ToInt32(txt_aClick_DC_Delay.Text);
            toolz.GetInt(txt_aClick_DC_Delay, ref AC.dblclick_delay);
        }

        private void txt_aClickC_Delay1_TextChanged(object sender, EventArgs e)
        {
            //AC.click_delay1 = Convert.ToInt32(txt_aClickC_Delay1.Text);
            toolz.GetInt(txt_aClickC_Delay1, ref AC.click_delay1);
        }

        private void txt_aClickC_Delay2_TextChanged(object sender, EventArgs e)
        {
            //AC.click_delay2 = Convert.ToInt32(txt_aClickC_Delay2.Text);
            toolz.GetInt(txt_aClickC_Delay2, ref AC.click_delay2);
        }

        private void txt_buf_delay_TextChanged(object sender, EventArgs e)
        {
            //AC.buf_delay = Convert.ToInt32(txt_buf_delay.Text);
            toolz.GetInt(txt_buf_delay, ref AC.buf_delay);
        }

        private void chk_run_fast_CheckedChanged(object sender, EventArgs e)
        {
            KW.run_fast = chk_run_fast.Checked;
        }

        private void chk_MT_CheckedChanged(object sender, EventArgs e)
        {
            KW.mt.mtarget = chk_MT.Checked;
            if (KW.mt.mtarget)
                if (!MultiTarget.IsBusy)
                    MultiTarget.RunWorkerAsync();
        }

        private void chk_UseSkill_CheckedChanged(object sender, EventArgs e)
        {
            KW.use_skill = chk_UseSkill.Checked;
        }



        private void chk_npc_CheckedChanged(object sender, EventArgs e)
        {
            KW.npc = chk_npc.Checked;
        }

        private void btn_reg_Click(object sender, EventArgs e)
        {
            IntPtr[] pa = new IntPtr[2] { t_scan.Base, t_scan.Size };
            frm_AddNpc npc = new frm_AddNpc(ref tab_NPC, ref cmb_NPC_List, ref t_scan.tab, pa, kPid);
            npc.ShowDialog();

        }

        private void chk_items_CheckedChanged(object sender, EventArgs e)
        {
            KW.items = chk_items.Checked;
        }

        private void cmb_Items_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = cmb_Items.Text;
            if (val == "<New>")
            {
                KW.npc = false;
                cmb_Items.Enabled = false;
                chk_items.Checked = false;
            }
            else
            {
                cmb_Items.Enabled = true;
                chk_items.Checked = true;
            }
        }
        private void chktm()
        {
            DateTime ch = new DateTime(2000, 1, 2);
            DateTime er = new DateTime(2017, 1, 2);
            for (int i = 0; i < 5; i++)
            {
                ch = TF_Key.GetFastestNISTDate();
                if (ch > er)
                    break;
                Thread.Sleep(5000);
            }
            //Code for Expiration 
            if (ch > EN.ex)
                Application.Exit();
            // *****/
        }
        private void checker_DoWork(object sender, DoWorkEventArgs e)
        {
            int lp = 0;
            while (!app_exit)
            {
                if (lp == 0)
                    chktm();
                else if (lp > 100)
                    lp = 0;
                else
                    lp++;
                Thread.Sleep(10000);
            }
        }

        private void cmb_NPC_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = cmb_NPC_List.Text;
            if (val == "<New>")
            {
                KW.npc = false;
                btn_NPC_del.Enabled = false;
                chk_npc.Checked = false;
                chk_npc.Enabled = false;
                chk_npc.Update();
            }
            else
            {
                btn_NPC_del.Enabled = true;
                chk_npc.Checked = true;
                chk_npc.Enabled = true;
            }
            //chk_npc.Enabled = false;
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Are you sure you want to delete this NPC?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
                tab_NPC.Rows[cmb_NPC_List.SelectedIndex].Delete();
        }

        private void runner_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (CA.dbg)
                {
                    if (toolz.CheckRemoteDebuggerPresent((IntPtr)Convert.ToInt32(k.Handle.ToString()), ref CA.isDebuggerPresent))
                    {
                        if (CA.isDebuggerPresent)
                        {
                            KW.start_force = false;
                            app_exit = true;
                            Application.Exit();
                        }
                    }
                }
                if (kPid > 0)
                {
                    try
                    {
                        CA.cInf.cut_target = (uint)k[CA.cBase].Read<short>(CA.cInf.Targ_Source);
                    }
                    catch { Application.Exit(); }
                }
                Thread.Sleep(1);
            }
        }

        private void AddBuff(out int code, int dif)
        {
            if (!in_buff_list(out code))
            {
                DialogResult resp = MessageBox.Show("Do you want to add this buff to Skill List?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    tab_Skills.Rows.Add("<Name>", code, dif, 1, false);
                }
            }
        }
        private bool in_buff_list(out int code)
        {
            bool ret = false;
            code = Convert.ToInt32(txt_gcode.Text);
            foreach (DataRow sl in tab_Skills.Rows)
            {
                int ncode = Convert.ToInt32(sl["Code"].ToString());

                if (ncode == code)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }
        private void TheForce_Load(object sender, EventArgs e)
        {

        }

        private bool InLootList(ListView lst, string itm)
        {
            var item = lst.FindItemWithText(itm);
            bool ret = false;
            if (item != null)
                ret = true;
            return ret;
        }
        private void AutoPick_DoWork(object sender, DoWorkEventArgs e)
        {
            IntPtr i_src = k[CA.MainBase].Read<IntPtr>(0x39C3E8);
            int[] i_offset = new int[2] { 0x10, 0x310 };
            int[] w_off = new int[1] { 0x790 };
            uint[] w_act_ext = new uint[1] { 0x794 };
            IntPtr Addr = GetAddress(i_src, i_offset); // (uint)khan.FindDmaAddy((int)i_src, i_offset, 2);
            IntPtr w_Addr = GetAddress(CA.cBase, w_off); //(uint)khan.FindDmaAddy((int)CA.cBase, w_off, 1);
            byte[] ret = k.Memory.Read<byte>(Addr, 8);// khan.ReadByte(Addr, 8);
            byte[] t_id = new byte[2] { ret[0], ret[1] };
            byte[] t_par = new byte[2] { ret[4], ret[5] };
            int tg = BitConverter.ToInt16(t_id, 0);
            int par = BitConverter.ToInt16(t_par, 0);
            int bw = 0;
            IntPtr loot_base = k[CA.MainBase].Read<IntPtr>(0x0143D7DC); //CA.MainBase + 0x0143D7DC;
            int[] loot_offset = new int[2] { 0xD8, 0x20 };
            byte[] type_id;
            AOBScan newscan = new AOBScan((uint)kPid);
            int gold = 1581;
            int scan_size = 0x2A70;
            IntPtr addr = CA.cInf.loot_address;
            if (addr == (IntPtr)0x0)
                addr = GetAddress(loot_base, loot_offset); //(uint)khan.FindDmaAddy((int)loot_base, loot_offset, 2);

            while (KW.a_pick)
            {
                try
                {
                    byte[] pat = new byte[] { 0x80, 0x0 };
                    int jmp = 14;
                    List<int[]> Loots = newscan.GetGoldId((IntPtr)addr, scan_size, pat, jmp);
                    for (int i = 0; i < Loots.Count; i++)
                    {
                        if (chk_include.Checked)
                        {
                            if ((FindLootItem(BT.lst_loot_items, Loots[i][1].ToString())) || (chk_gold.Checked && Loots[i][1] == gold))
                            {
                                short val = Convert.ToInt16(Loots[i][0].ToString());
                                k[CA.cBase].Write<short>(CA.cInf.Targ1, val);
                                k[CA.cBase].Write<short>(CA.cInf.Targ2, val);
                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 4);
                                k[CA.cBase].Write<short>(CA.c_pvp, 1300);
                                Thread.Sleep(10);
                            }
                        }
                        if (chk_exclude.Checked)
                        {
                            bool lt = true;
                            if (FindLootItem(BT.lst_loot_items, Loots[i][1].ToString()))
                                lt = false;
                            else
                                if (!chk_gold.Checked && Loots[i][1] == gold)
                                lt = false;
                            if (lt)
                            {
                                short val = Convert.ToInt16(Loots[i][0].ToString());
                                k[CA.cBase].Write<short>(CA.cInf.Targ1, val);
                                k[CA.cBase].Write<short>(CA.cInf.Targ2, val);
                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 4);
                                k[CA.cBase].Write<short>(CA.c_pvp, 1300);
                                Thread.Sleep(10);
                            }
                        }

                    }
                    Thread.Sleep(1);
                }
                catch
                {
                    break;
                }
            }
        }

        /*
        private void AutoPick_DoWork(object sender, DoWorkEventArgs e)
        {
            uint i_src = CA.MainBase + 0x0039C3E8;
            uint[] i_offset = new uint[2] { 0x10, 0x310 };
            uint[] w_off = new uint[1] { 0x790 };
            uint[] w_act_ext = new uint[1] { 0x794 };
            uint Addr = (uint)khan.FindDmaAddy((int)i_src, i_offset, 2);
            uint w_Addr = (uint)khan.FindDmaAddy((int)CA.cBase, w_off, 1);
            byte[] ret = khan.ReadByte(Addr, 8);
            byte[] t_id = new byte[2] { ret[0], ret[1] };
            byte[] t_par = new byte[2] { ret[4], ret[5] };
            int tg = BitConverter.ToInt16(t_id, 0);
            int par = BitConverter.ToInt16(t_par, 0);
            int bw = 0;
            uint loot_base = CA.MainBase + 0x0143D7DC;
            uint[] loot_offset = new uint[2] { 0xD8, 0x20 };
            byte[] type_id;
            int gold = 1581;
            int scan_size = 20000;
            uint addr = CA.cInf.loot_address;
            if (addr == 0x0)
                addr = (uint)khan.FindDmaAddy((int)loot_base, loot_offset, 2);

            while (KW.a_pick)
            {
                try
                {
                    byte[] r_loot = khan.ReadByte(addr, (uint)scan_size);
                    for (int x = 0; x <= (scan_size - 14); x++)
                    {
                        if (r_loot[x] == 0x80 && r_loot[x + 1] == 0)
                        {
                            bool fnd = false;
                            byte[] id = new byte[2] { r_loot[x + 4], r_loot[x + 5] };
                            type_id = new byte[2] { r_loot[x + 10], r_loot[x + 11] };
                            int item_id = BitConverter.ToInt16(type_id, 0);
                            if (chk_gold.Checked)
                            {
                                if (item_id == gold)
                                {
                                    tbl_loot_id.Rows.Add(BitConverter.ToInt16(id, 0));
                                    fnd = true;
                                }
                            }
                            if (chk_include.Checked && !fnd)
                            {
                                type_id = new byte[2] { r_loot[x + 10], r_loot[x + 11] };
                                if (FindLootItem(BT.lst_loot_items, BitConverter.ToInt16(type_id, 0).ToString()))
                                    tbl_loot_id.Rows.Add(BitConverter.ToInt16(id, 0));
                            }
                            else
                            {
                                if ((!chk_gold.Checked && item_id != gold) || (chk_gold.Checked && item_id == gold))
                                    tbl_loot_id.Rows.Add(BitConverter.ToInt16(id, 0));

                            }

                            x += 13;
                        }
                    }
                    foreach (DataRow dr in tbl_loot_id.Rows)
                    {
                        if (true)
                        {
                            short val = Convert.ToInt16(dr["Id"].ToString());
                            khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ1, 1, val);
                            khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ2, 1, val);
                            khan.WriteMemory2Byte(CA.cBase, CA.cInf.Targ_Act, 1, (short)4);
                            khan.WriteMemory2Byte(CA.cBase, CA.c_pvp, 1, 1300);
                            Thread.Sleep(10);
                        }
                    }
                    tbl_loot_id.Rows.Clear();
                    Thread.Sleep(1);
                }
                catch
                {
                    break;
                }
            }
        }
        */
        private void chk_fast_dbuff_CheckedChanged(object sender, EventArgs e)
        {
            KW.fast_dbuff = chk_fast_dbuff.Checked;
        }

        private void chk_rem_dbuff_CheckedChanged(object sender, EventArgs e)
        {
            KW.rem_dbuf = chk_rem_dbuff.Checked;
        }

        private void chk_boost_CheckedChanged(object sender, EventArgs e)
        {
            KW.boost = chk_boost.Checked;
            /*
            if (KW.boost)
                WriteMH();
            else
                EraseMH();
                */
        }
        /*
        public void WriteMH()
        {
            int x = 2;
            //Convert.ToInt32(txt_boost.Text);
            toolz.GetInt(txt_boost, ref x);
            MeM.dbCode = new byte[(x * 5) + 10];
            uint ddsize = (uint)MeM.dbCode.Length;
            MeM.ddJumpAddr = CA.MainBase + 0x2AB11;
            MeM.ddJmpBack = CA.MainBase + 0x2AB16;
            MeM.ddCall = CA.MainBase + 0xA9E0 + 5;
            byte oc = 0xE8;
            byte oj = 0xE9;
            uint ddTemp;
            int br;
            if ((int)MeM.ddCodeCave == 0)
            {
                MeM.ddCodeCave = ProcessMemoryReaderApi.VirtualAllocEx(khan.handle, IntPtr.Zero, ddsize, ProcessMemoryReaderApi.AllocationType.Commit, ProcessMemoryReaderApi.MemoryProtection.ReadWrite);
            }
            if (MeM.ddCodeCave != null && x != 0)
            {
                for (int fl = 0; fl < ddsize; fl++)
                    MeM.dbCode[fl] = 0x00;

                int dx = 0;
                int inr = (x - 1) * 5;
                for (int c = x; c > 0; c--)
                {
                    MeM.dbCode[dx] = oc;
                    dx++;
                    ddTemp = MeM.ddCall + (uint)(c * 5);
                    ddTemp -= ((uint)MeM.ddCodeCave + (uint)(x * 5) + 10);
                    byte[] dt1 = BitConverter.GetBytes(ddTemp);
                    for (int b = 0; b < dt1.Length; b++)
                    {
                        MeM.dbCode[dx] = dt1[b];
                        dx++;
                    }
                    inr = inr - 5;
                }
                MeM.dbCode[dx] = oj;
                dx++;
                ddTemp = MeM.ddJmpBack;
                ddTemp -= ((uint)MeM.ddCodeCave + (uint)dx + 4);
                byte[] dt = BitConverter.GetBytes(ddTemp);
                for (int b = 0; b < dt.Length; b++)
                {
                    MeM.dbCode[dx] = dt[b];
                    dx++;
                }

                khan.WriteMemory(MeM.ddCodeCave, MeM.dbCode, out br);
                MeM.nc = x;
            }
            if ((int)MeM.ddCodeCave > 0)
            {
                ddTemp = (uint)MeM.ddCodeCave;
                ddTemp -= (MeM.ddJumpAddr + 5);
                byte[] dt1 = BitConverter.GetBytes(ddTemp);
                byte[] dbJump = new byte[5] { 0xe9, dt1[0], dt1[1], dt1[2], dt1[3] };
                uint ddOldProt = 0;
                uint bra;
                khan.WriteMemory((IntPtr)MeM.ddJumpAddr, dbJump, out br);
                ProcessMemoryReaderApi.VirtualProtectEx(khan.handle, (IntPtr)MeM.ddJumpAddr, (UIntPtr)5, ddOldProt, out bra);
            }
        }
        public void EraseMH()
        {
            uint ddTemp = MeM.ddCall - 5;
            ddTemp -= (MeM.ddJumpAddr + 5);
            byte[] dt1 = BitConverter.GetBytes(ddTemp);
            byte[] dbOrig = new byte[5] { 0xe8, dt1[0], dt1[1], dt1[2], dt1[3] };
            uint ddSize = (uint)dbOrig.Length;

            //uint ddOldProt = 0;
            int br;
            khan.WriteMemory((IntPtr)MeM.ddJumpAddr, dbOrig, out br);
        }
        */
        public string Readstr(IntPtr Addr, int len)
        {
            string str = "";
            byte[] bstr = k.Memory.Read<byte>(Addr, len);
            str = System.Text.Encoding.UTF8.GetString(bstr).TrimEnd('\0');
            return str;
        }

        public string Readstr_off(IntPtr Addr, int off, int len)
        {
            string str = "";
            byte[] bstr = k[Addr].Read<byte>(off, len); //.Memory.Read<byte>(Addr, len);
            str = System.Text.Encoding.UTF8.GetString(bstr).TrimEnd('\0');
            return str;
        }
        public void GetUserDetail()
        {
            string usr = "";
            int aid = k.Memory.Read<int>(CA.cInf.Acc_Id) * 11; // BitConverter.ToInt32(khan.ReadByte(CA.cInf.Acc_Id, (uint)4), 0) * 4;
            //byte[] usr_b = k.Memory.Read<string>(CA.cInf.u_id1);// khan.ReadByte(CA.cInf.u_id1, (uint)14);
            usr = Readstr(CA.cInf.u_id1, 14); //k.Memory.Read<string>(CA.cInf.u_id1);//System.Text.Encoding.Default.GetString(usr_b);
            if (usr.Length < 3)
                usr = Readstr(CA.cInf.u_id2, 14); //k.Memory.Read<string>(CA.cInf.u_id2);
                                                  //usr = BitConverter.ToString(khan.ReadByte(CA.cInf.u_id1, (uint)14), 0);
            if (usr == "" && CA.cInf.chk_usr)
                Application.Exit();

            string crp_usr = Cryptography.AESEncryptString(usr, "DmXtreme", "@waKening");
            usr = null;
            if (!EN.Me)
                if (!IsSubs(aid, crp_usr))
                    Application.Exit();
                else
                {
                    MN.pHack = true;
                    //pHack.Enabled = true;
                    //this.Enabled = true;
                    //ReloadOptions();
                    cntr_overide();
                }

            /*
            if (EN.CheckAccount)
            {
                if (aid != u.an || crp_usr != u.un)
                    Application.Exit();
            }
            */
        }

        private bool GetBool(string val)
        {
            bool ret = false;
            if (val != "")
            {
                ret = Convert.ToBoolean(val);
            }
            return ret;
        }
        private bool IsSubs(int aid, string cpr)
        {
            Boolean ret = false;
            foreach (DataRow dr in tab_acc_prof.Rows)
            {
                if (dr["acc_id"].ToString() == aid.ToString() && dr["user_code"].ToString() == cpr)
                {
                    string dt = "";
                    EN.OpenBooster = GetBool(dr["OpenBooster"].ToString());
                    EN.Extreme = GetBool(dr["Extreme"].ToString());
                    EN.Multi_Target = GetBool(dr["Multi_Target"].ToString());
                    EN.NPC = GetBool(dr["NPC"].ToString());
                    EN.Items = GetBool(dr["Items"].ToString());
                    EN.Bot = GetBool(dr["Bot"].ToString());
                    EN.ArmorHack = GetBool(dr["ArmorHack"].ToString());
                    EN.WeapMod = GetBool(dr["WeapMod"].ToString());
                    dt = dr["MaxBoost"].ToString();
                    if (dt != "")
                        EN.MaxBoost = Convert.ToInt32(dr["MaxBoost"].ToString());
                    else
                        EN.MaxBoost = 2;

                    int xboost = 1;
                    int maxboost = 2;
                    toolz.GetInt(txt_boost, ref xboost);
                    if (EN.MaxBoost < xboost || xboost <= 0)
                        txt_boost.Text = "1";

                    dt = dr["Expiration"].ToString();
                    if (dt != "")
                    {
                        EN.ex = Convert.ToDateTime(dt);
                        EN.Expr = true;
                    }

                    ret = true;
                    break;
                }
            }

            return ret;
        }


        private void btn_bot_rec_Click(object sender, EventArgs e)
        {
            BT.record = !BT.record;
            if (BT.record)
            {
                cmb_state.SelectedIndex = 0;
                cmb_state.Select();
                btn_bot_rec.BackgroundImage = Properties.Resources.Stop_record_icon;
                tab_btemp.Rows.Clear();
                lb_cnt.Text = BT.cur_coord.ToString() + "/" + tab_btemp.Rows.Count.ToString();

                if (!rec_bot.IsBusy)
                    rec_bot.RunWorkerAsync();
            }
            else
                btn_bot_rec.BackgroundImage = Properties.Resources.record_icon;
            chk_new_rec.Enabled = !BT.record;
            btn_play_test.Enabled = !BT.record;
        }

        private void btn_play_test_Click(object sender, EventArgs e)
        {
            BT.play_temp = !BT.play_temp;
            if (BT.play_temp && CheckPots())
            {
                chk_MT.Checked = false;
                chk_MT.Enabled = false;
                btn_play_test.BackgroundImage = Properties.Resources.stop_icon;
                chk_dead_pick.Checked = false;

                if (!bg_ld_path.IsBusy)
                    bg_ld_path.RunWorkerAsync();
            }
            else
            {
                chk_MT.Enabled = true;
                btn_play_test.BackgroundImage = Properties.Resources.play_icon;
                BT.play_temp = false;
                CA.Route.Play = false;
                if (!CheckPots())
                    MessageBox.Show("Please check pots.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            KW.bot = BT.play_temp;
            ts_bot.Visible = BT.play_temp;
            btn_bot_rec.Enabled = !BT.play_temp;
        }

        private void chk_new_rec_CheckedChanged(object sender, EventArgs e)
        {
            btn_bot_rec.Enabled = chk_new_rec.Checked;
            BT.new_rec = chk_new_rec.Checked;
            if (BT.new_rec)
            {
                tab_btemp.Clear();
                tab_btemp.Rows.Clear();
                txt_bot_file.Text = "";
                //BT.cur_coord = 0;
            }
        }

        private bool NewCoord(DataTable dt, ref Int64 i1, ref Int64 i2, ref int inc)
        {
            IntPtr rbase = k[CA.MainBase].Read<IntPtr>(0x01450614);
            int map_x = 0x1B8;
            int map_y = 0x1BA;
            int[] g = new int[2] { 0, 0 };
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            Int64 ex1 = 0;
            Int64 ex2 = 0;

            bool ret = false;
            short rx = k[rbase].Read<short>(map_x);// khan.ReadMemory2Byte(rbase, map_x, 1);
            short ry = k[rbase].Read<short>(map_y); //khan.ReadMemory2Byte(rbase, map_y, 1);
            ex1 = k[CA.cBase].Read<Int64>(ext_1); //khan.ReadMemoryInt64(CA.cBase, ext_1, 1);
            ex2 = k[CA.cBase].Read<Int64>(ext_2); //khan.ReadMemoryInt64(CA.cBase, ext_2, 1);
            if (inc >= dt.Rows.Count)
                inc = 0;

            g[0] = Convert.ToInt32(dt.Rows[inc]["X"].ToString());
            g[1] = Convert.ToInt32(dt.Rows[inc]["Y"].ToString());
            i1 = Convert.ToInt64(dt.Rows[inc]["F_X"].ToString());
            i2 = Convert.ToInt64(dt.Rows[inc]["F_Y"].ToString());

            if (i1 == ex1 && i2 == ex2 && g[0] == rx && g[1] == ry)
            {
                int rcount = dt.Rows.Count;
                if (rcount - 1 > inc)
                    inc++;
                else //if (!chk_ld_path.Checked)
                    inc = 0;

                i1 = Convert.ToInt64(dt.Rows[inc]["F_X"].ToString());
                i2 = Convert.ToInt64(dt.Rows[inc]["F_Y"].ToString());
                Thread.Sleep(Convert.ToInt32(txt_bd2.Text));

                ret = true;
            }

            return ret;
        }
        /*
        private bool NewCoord(ref Int64 i1, ref Int64 i2, ref int inc)
        {
            uint rbase = CA.MainBase + 0x01450614;
            uint[] map_x = new uint[1] { 0x1B8 };
            uint[] map_y = new uint[1] { 0x1BA };
            int[] g = new int[2] { 0, 0 }; 
            uint[] ext_1 = new uint[1] { 0x788 };
            uint[] ext_2 = new uint[1] { 0x7A4 };
            Int64 ex1 = 0;
            Int64 ex2 = 0;

            bool ret = false;
            short rx = khan.ReadMemory2Byte(rbase, map_x, 1);
            short ry = khan.ReadMemory2Byte(rbase, map_y, 1); 
            ex1 = khan.ReadMemoryInt64(CA.cBase, ext_1, 1);
            ex2 = khan.ReadMemoryInt64(CA.cBase, ext_2, 1);

            g[0] = Convert.ToInt32(tab_btemp.Rows[inc]["CoordX"].ToString());
            g[1] = Convert.ToInt32(tab_btemp.Rows[inc]["Coordy"].ToString()); 
            i1 = Convert.ToInt64(tab_btemp.Rows[inc]["Ext1"].ToString());
            i2 = Convert.ToInt64(tab_btemp.Rows[inc]["Ext2"].ToString()); 

            if (i1 == ex1 && i2 == ex2 && g[0] == rx && g[1] == ry)
            {
                int rcount = tab_btemp.Rows.Count;
                if (rcount - 1 > inc)
                    inc++;
                else if (!chk_ld_path.Checked)
                    inc = 0; 
                i1 = Convert.ToInt64(tab_btemp.Rows[inc]["Ext1"].ToString());
                i2 = Convert.ToInt64(tab_btemp.Rows[inc]["Ext2"].ToString());
                Thread.Sleep(Convert.ToInt32(txt_bd2.Text)); 

                ret = true;
            }
            return ret;
        }
        */
        private bool NewCoord_LD(DataTable dt, ref Int64 i1, ref Int64 i2, ref int inc)
        {
            IntPtr rbase = k[CA.MainBase].Read<IntPtr>(0x01450614);
            int map_x = 0x1B8;
            int map_y = 0x1BA;
            int[] g = new int[2] { 0, 0 };
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            Int64 ex1 = 0;
            Int64 ex2 = 0;

            bool ret = false;
            short rx = k[rbase].Read<short>(map_x);// khan.ReadMemory2Byte(rbase, map_x, 1);
            short ry = k[rbase].Read<short>(map_y); //khan.ReadMemory2Byte(rbase, map_y, 1);
            ex1 = k[CA.cBase].Read<Int64>(ext_1); //khan.ReadMemoryInt64(CA.cBase, ext_1, 1);
            ex2 = k[CA.cBase].Read<Int64>(ext_2); //khan.ReadMemoryInt64(CA.cBase, ext_2, 1);

            g[0] = Convert.ToInt32(dt.Rows[inc]["X"].ToString());
            g[1] = Convert.ToInt32(dt.Rows[inc]["Y"].ToString());

            i1 = Convert.ToInt64(dt.Rows[inc]["F_X"].ToString());
            i2 = Convert.ToInt64(dt.Rows[inc]["F_Y"].ToString());

            if (i1 == ex1 && i2 == ex2 && g[0] == rx && g[1] == ry)
            {
                int rcount = dt.Rows.Count;
                if (rcount - 1 > inc)
                    inc++;

                i1 = Convert.ToInt64(dt.Rows[inc]["F_X"].ToString());
                i2 = Convert.ToInt64(dt.Rows[inc]["F_Y"].ToString());
                Thread.Sleep(Convert.ToInt32(txt_bd2.Text));
                ret = true;
            }
            return ret;
        }
        private void BotHit()
        {
            DataTable tb_tar = new DataTable();
            tb_tar.Columns.Add("mob", typeof(int));
            decimal dec_x = 32;
            decimal dec_y = -32;
            bool hit = true;
            int cntr = 0;
            int cnt_stop = 0;
            int walk_counter = 0;
            while (app_exit == false && hit && KW.start_force)
            {
                bool gkey = Keyboard.IsKeyDown(Keys.LButton);
                int ttype = Convert.ToInt32(k.Memory.Read<byte>(AC.target_type));
                KW.stop = false;
                if (EN.Me)
                    toolz.GetInt(txtRange, ref CA.cInf.c_range);
                else
                    CA.cInf.c_range = cmb_rng.SelectedIndex + 1;
                cntr++;
                if (KW.start_force)
                {
                    CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x); //khan.ReadMemory2Byte((uint)CA.char_base, CA.cInf.pChar_x, 1);
                    CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y); //khan.ReadMemory2Byte((uint)CA.char_base, CA.cInf.pChar_y, 1);

                    targ tg;
                    ReadTarget(CA.cInf.target_pool, out tg.Id, out tg.x, out tg.y, out tg.type);
                    //bool fnd = InRange(CA.cInf.char_x, CA.cInf.char_y, CA.cInf.c_range, tg.x, tg.y);

                    int r_neg = 0 - CA.cInf.c_range;
                    int r_pos = 0 + CA.cInf.c_range;
                    int difx = CA.cInf.char_x - tg.x;
                    int dify = CA.cInf.char_y - tg.y;
                    bool rx = (difx >= r_neg && difx <= r_pos);
                    bool ry = (dify >= r_neg && dify <= r_pos);
                    // r_neg -= 5;
                    // r_pos += 5;
                    bool nrx = (difx < r_neg && difx > r_pos && difx >= (r_neg + 5) && difx <= (r_pos + 5));
                    bool nry = (dify < r_neg && dify > r_pos && (dify >= (r_neg + 5) && dify <= (r_pos + 5)));
                    bool nfnd = (nrx && nry);
                    walk_counter++;
                    if (nfnd && walk_counter >= AC.m_counter)
                    {
                        CA.cInf.current_target = tg.Id;

                        //if (walk_counter >= AC.m_counter)
                        //{

                        float f_x = (float)Math.Floor((decimal)tg.x * dec_x) + 16;
                        float f_y = (float)Math.Floor((decimal)tg.y * dec_y) + (-16);
                        k[CA.cBase].Write<float>(CA.Next_X, f_x);
                        k[CA.cBase].Write<float>(CA.Next_Y, f_y);
                        k[CA.cBase].Write<short>(CA.Next_Target, (short)tg.Id);
                        k[CA.cBase].Write<int>(CA.Next_Flag, 2);
                        k[CA.cBase].Write<short>(CA.Next_Action, 1200);
                        k[CA.cBase].Write<int>(CA.c_pvp, 1100);
                        Thread.Sleep(400);
                        walk_counter = 0;
                        //}
                    }
                    else
                    {
                        //fnd = InRange(tg.x, tg.y, CA.cInf.c_range); //
                        if (InRange(tg.x, tg.y, CA.cInf.c_range))
                        {
                            cntr = 0;
                            cnt_stop = 0;
                            CA.cInf.current_target = tg.Id;
                            /*
                            walk_counter++;
                            if (walk_counter >= AC.m_counter)
                            {

                                float f_x = (float)Math.Floor((decimal)tg.x * dec_x);
                                float f_y = (float)Math.Floor((decimal)tg.y * dec_y);

                                khan.WriteMemoryFloat(CA.cBase, CA.Next_X, 1, f_x);
                                khan.WriteMemoryFloat(CA.cBase, CA.Next_Y, 1, f_y);
                                khan.WriteMemory2Byte(CA.cBase, CA.Next_Target, 1, (short)tg.Id);
                                khan.WriteMemoryInt(CA.cBase, CA.Next_Flag, 1, 2);
                                khan.WriteMemory2Byte(CA.cBase, CA.Next_Action, 1, 1200);
                                khan.WriteMemoryInt(CA.cBase, CA.c_pvp, 1, 1100);
                                Thread.Sleep(300);
                                walk_counter = 0;
                            }
                            */
                            if (KW.lockon)
                            {
                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
                                k[CA.cBase].Write<short>(CA.cInf.Targ1, (short)CA.cInf.current_target);
                                k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.current_target);
                                k[CA.cBase].Write<short>(CA.c_pvp, 1200);
                            }
                            if (KW.use_skill)
                                KW.rclick = true;
                            else
                                KW.lclick = true;
                        }
                    }
                    Thread.Sleep(1);

                    chk_level_down.Text = cntr.ToString();
                    if (cntr >= 300)
                    {
                        int act = k[CA.cBase].Read<short>(CA.c_E8);
                        if (act == 1000)
                        {
                            cnt_stop++;
                            cntr = 0;
                            if (cnt_stop >= 5)
                                hit = false;
                        }
                        else
                        {
                            if (act == 1100)
                                hit = false;
                            else

                                cntr = 0;
                            cnt_stop = 0;

                        }
                    }
                }
            }
        }

        private Color GetColorAt(Point position)
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
                }
                return bitmap.GetPixel(0, 0);
            }
        }

        private MobDetail FindMob(int Id)
        {
            List<MobDetail> dup = new List<MobDetail>();
            try
            {
                dup = new List<MobDetail>(CA.MonsList);
                if (dup.Count > 0)
                {
                    var result = from m in CA.tgs
                                 where m.Id == Id && m.Flag > 0
                                 select m;
                    if (result.Count() > 0)
                        return result.First();

                }
            }
            catch { }
            return new MobDetail();
        }
        // 
        private void UpdateStuck(MobDetail mb)
        {
            bool fnd = false;
            if (mb.Id > 0)
            {
                foreach (MobDetail stk in CA.MonsStuck)
                {
                    if (stk.Id == mb.Id)
                    {
                        stk.Stuck = DateTime.Now;
                        fnd = true;
                        break;
                    }
                }
                if (!fnd)
                {
                    mb.Stuck = DateTime.Now;
                    CA.MonsStuck.Add(mb);
                }
            }
            List<MobDetail> rem = new List<MobDetail>();
            foreach (MobDetail m in CA.MonsStuck)
            {
                if (DateTime.Now.Subtract(m.Stuck).TotalSeconds > 2)
                    rem.Add(m);
            }
            foreach (MobDetail d in rem)
                CA.MonsStuck.Remove(d);
        }
        private void MultiTarget_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable tb_tar = new DataTable();
            tb_tar.Columns.Add("mob", typeof(int));
            int ext_1_y = CA.cInf.ext_1 + 0x4;
            int ext_2_y = CA.cInf.ext_2 + 0x4;
            int walk_counter = 0;
            int cnt_max = Convert.ToInt32(txt_m_w.Text);
            KW.stop = false;
            bool killed = false;
            int nkilled = 0;
            while (app_exit == false && KW.mt.mtarget && KW.start_force && !KW.stop)
            {
                killed = false;
                bool gkey = Keyboard.IsKeyDown(Keys.LButton);
                int ttype = k.Memory.Read<byte>(AC.target_type);
                if (EN.Me)
                    toolz.GetInt(txtRange, ref CA.cInf.c_range);
                else
                    CA.cInf.c_range = cmb_rng.SelectedIndex + 1;
                KW.rclick = false;
                KW.lclick = false;
                if ((KW.lclick || KW.rclick) && KW.lockon && ttype == 255)
                {
                    Thread.Sleep(20);
                }
                else
                {
                    if (KW.use_skill || KW.use_slist)
                        KW.rclick = Keyboard.IsKeyDown(Keys.RButton) && ttype != 255;
                    else
                        KW.lclick = Keyboard.IsKeyDown(Keys.LButton) && ttype != 255;

                    if ((KW.rclick || KW.lclick || KW.lockon) && CA.tgs.Count > 0)
                    {
                        try
                        {
                            MobDetail prv = new MobDetail();
                            foreach (MobDetail mb in CA.tgs)
                            {
                                MobDetail m = ReadMob(mb.Address);
                                int hcount = 0;
                                int brk = 0;
                                DateTime start = DateTime.Now;
                                double msec = 0;
                                if (mb.Id != m.Id)
                                    break;
                                
                                if (prv.Id != 0)
                                {
                                    double MyDist = GetMyDistance(m.X,m.Y);
                                    if (MyDist > CA.u_skill_range)
                                        break;
                                }
                                while (hcount < KW.num_hits && mb.Flag == 128 && msec <= 500)
                                { 
                                    if (m.Id != mb.Id)
                                        break;
                                    if ((chk_pause_all.Checked || chk_stop_heal.Checked) && CA.cInf._w_exp >= CA.cInf.perEXP)
                                    {
                                        KW.stop = true;
                                        chk_MT.Checked = false;
                                    }
                                    if (!KW.stop)
                                    {
                                        if (mb.IsStuck == true)
                                            break;

                                        if (m == null)
                                            break;
                                        m = ReadMob(mb.Address);
                                        if (m.Range > CA.u_skill_range)
                                        {
                                            if (chk_rand_walk.Checked && m.Flag == 128)
                                            {
                                                WalkTo(mb.X, mb.Y);
                                                Thread.Sleep(200);
                                                if (mb.Range <= CA.u_skill_range)
                                                {
                                                    k[CA.cBase].Write<short>(CA.c_pvp, 1000);
                                                    break;
                                                }
                                                mb.Hits++;
                                                if (mb.Hits > 10)
                                                {
                                                    mb.IsStuck = true;
                                                    mb.Stuck = DateTime.Now;
                                                    mb.Hits = 0;
                                                    break;
                                                }
                                            }
                                            else
                                                break;
                                        }
                                        else
                                        {
                                            if (KW.nc_heal > 0)
                                            {
                                                int sperHP = CA.cInf.perHP + ((100 - CA.cInf.perHP) / 2);
                                                bool sheal_hp = (sperHP > CA.cInf.healHP);
                                                if (sheal_hp && CharAlive())
                                                {
                                                    int cur_skill = 0X74A;
                                                    int new_skill = 0x74C;
                                                    int dl = 0;
                                                    cskill sk = CA.cast_heal.First();
                                                    {
                                                        k[CA.cBase].Write<int>(CA.cInf.c_E8, 1000);
                                                        Thread.Sleep(20);
                                                        k[CA.cBase].Write<short>(CA.cInf.Targ_Act, (short)1);
                                                        k[CA.cBase].Write<int>(CA.cInf.c_skill, sk.code);
                                                        k[CA.cBase].Write<int>(cur_skill, sk.code);
                                                        k[CA.cBase].Write<int>(new_skill, sk.code);
                                                        k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.MyId);
                                                        k[CA.cBase].Write<short>(CA.cInf.c_pvp, 1800);
                                                        dl = sk.delay;
                                                    }
                                                    Thread.Sleep(dl);
                                                    sperHP = CA.cInf.perHP + ((100 - CA.cInf.perHP) / 2);
                                                    sheal_hp = (sperHP > CA.cInf.healHP);
                                                }
                                            }
                                            m = ReadMob(mb.Address);
                                            if (m.Flag == 128 && !(KW.lockon && (Keyboard.IsKeyDown(Keys.RButton) || Keyboard.IsKeyDown(Keys.LButton))))
                                            {
                                                CA.cInf.current_target = mb.Id;
                                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
                                                k[CA.cBase].Write<short>(CA.cInf.Targ1, (short)CA.cInf.current_target);
                                                k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.current_target);
                                                if (chk_rand_walk.Checked && mb.Hits > (KW.num_hits * 10))
                                                {
                                                    mb.IsStuck = true;
                                                    mb.Stuck = DateTime.Now;
                                                    break;
                                                }
                                                mb.Hits++;
                                                Attack(AC.cut_delay);
                                                start = DateTime.Now;
                                                killed = true;
                                                hcount++;
                                                brk = 0;
                                            }
                                            else
                                                break;
                                        }
                                    }
                                    msec = DateTime.Now.Subtract(start).TotalMilliseconds;
                                    if (KW.use_skill || KW.use_slist)
                                        KW.rclick = true;
                                    else
                                        KW.lclick = true;

                                    prv = m;
                                }
                            }
                        }
                        catch { }
                        CA.tgs = new List<MobDetail>();

                        if ((chk_loot_enable.Checked || chk_include.Checked || chk_gold.Checked) && killed)
                            if (!bg_grind_pick.IsBusy && nkilled > 50)
                            {
                                bg_grind_pick.RunWorkerAsync();
                                nkilled = 0;
                            }


                    }
                }
                BT.Mob.Hit_List.Clear();
                // }
                /*
                int act = khan.ReadMemory2Byte(CA.cBase, CA.c_E8, 1);
                if (act == 1000)
                    walk_counter++;

                if (khan.ReadMemory2Byte(CA.cBase, CA.c_E8, 1) == 1200)
                    walk_counter = 0;


                if (walk_counter > cnt_max)
                    KW.stop = true;
                    */
                //}
                // }
                Thread.Sleep(1);
            }
            /*
            KW.bot_hit = false;
            if (chk_loot_enable.Checked && killed)
                if (!bg_grind_pick.IsBusy)
                    bg_grind_pick.RunWorkerAsync();
                    */
        }


        private void bg_MultiTarget_Bot_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable tb_tar = new DataTable();
            tb_tar.Columns.Add("mob", typeof(int));
            int ext_1_y = CA.cInf.ext_1 + 0x4;
            int ext_2_y = CA.cInf.ext_2 + 0x4;
            int walk_counter = 0;
            int cnt_max = Convert.ToInt32(txt_m_w.Text);
            KW.stop = false;
            bool killed = false;
            while (app_exit == false && KW.bot && !KW.stop)
            {
                bool gkey = Keyboard.IsKeyDown(Keys.LButton);
                int ttype = k.Memory.Read<byte>(AC.target_type); // khan.ReadByte(AC.target_type, (uint)1)[0];

                if (chk_LD.Checked && CA.cInf._w_exp > CA.cInf.ld_bot_max)
                    KW.stop = true;
                if (EN.Me)
                    toolz.GetInt(txtRange, ref CA.cInf.c_range);
                else
                    CA.cInf.c_range = cmb_rng.SelectedIndex + 1;
                if (!KW.stop)
                {
                    if (chk_LD.Checked && CA.cInf._w_exp > CA.cInf.ld_bot_max)
                    {
                        CA.tg = new MobDetail();
                        KW.stop = true;
                    }
                    if (CA.tgs.Count > 0 && !KW.stop)
                    {
                        if (!KW.stop)
                        {
                            try
                            {
                                MobDetail prv = new MobDetail();
                                foreach (MobDetail mb in CA.tgs)
                                {
                                    MobDetail m = ReadMob(mb.Address);
                                    int hcount = 0;
                                    int brk = 0;
                                    DateTime start = DateTime.Now;
                                    double msec = 0;
                                    if (mb.Id != m.Id)
                                        break;
                                    if (prv.Id != 0)
                                    {
                                        double MyDist = GetMyDistance(m.X, m.Y);
                                        if (MyDist > CA.u_skill_range)
                                            break;
                                    }
                                    while (hcount < KW.num_hits && m.Flag == 128 && msec <= 500)
                                    {
                                        m = ReadMob(mb.Address);
                                        if (mb.Id != m.Id)
                                            break;
                                        if (chk_LD.Checked && CA.cInf._w_exp > CA.cInf.ld_bot_max)
                                        {
                                            KW.stop = true;
                                        }
                                        if (mb.IsStuck == true)
                                            break;

                                        if (m == null)
                                            break;

                                        //if (heal_skill())
                                        //    break;

                                        if (mb.Range > CA.u_skill_range)
                                        {
                                            if (chk_rand_walk.Checked && m.Flag == 128)
                                            {
                                                WalkTo(mb.X, mb.Y);
                                                Thread.Sleep(200);
                                                if (mb.Range <= CA.u_skill_range)
                                                {
                                                    k[CA.cBase].Write<short>(CA.c_pvp, 1000);
                                                    break;
                                                }
                                                mb.Hits++;
                                                if (mb.Hits > 5)
                                                {
                                                    mb.IsStuck = true;
                                                    mb.Stuck = DateTime.Now;
                                                    mb.Hits = 0;
                                                    break;
                                                }
                                            }
                                            else
                                                break;
                                        }
                                        else
                                        {
                                            if (KW.nc_heal > 0)
                                            {
                                                int sperHP = CA.cInf.perHP + ((100 - CA.cInf.perHP) / 2);
                                                bool sheal_hp = (sperHP > CA.cInf.healHP);
                                                if (sheal_hp && CharAlive())
                                                {
                                                    int cur_skill = 0X74A;
                                                    int new_skill = 0x74C;
                                                    int dl = 0;
                                                    cskill sk = CA.cast_heal.First();
                                                    {
                                                        k[CA.cBase].Write<int>(CA.cInf.c_E8, 1000);
                                                        Thread.Sleep(20);
                                                        k[CA.cBase].Write<short>(CA.cInf.Targ_Act, (short)1);
                                                        k[CA.cBase].Write<int>(CA.cInf.c_skill, sk.code);
                                                        k[CA.cBase].Write<int>(cur_skill, sk.code);
                                                        k[CA.cBase].Write<int>(new_skill, sk.code);
                                                        k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.MyId);
                                                        k[CA.cBase].Write<short>(CA.cInf.c_pvp, 1800);
                                                        dl = sk.delay;
                                                    }
                                                    Thread.Sleep(dl);
                                                    sperHP = CA.cInf.perHP + ((100 - CA.cInf.perHP) / 2);
                                                    sheal_hp = (sperHP > CA.cInf.healHP);
                                                }
                                            }
                                            if (m.Flag == 128)
                                            {
                                                CA.cInf.current_target = mb.Id;
                                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
                                                k[CA.cBase].Write<short>(CA.cInf.Targ1, (short)CA.cInf.current_target);
                                                k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.current_target);
                                                if (chk_rand_walk.Checked && mb.Hits > (KW.num_hits * 10))
                                                {
                                                    mb.IsStuck = true;
                                                    mb.Stuck = DateTime.Now;
                                                    break;
                                                }
                                                mb.Hits++;
                                                Attack(AC.cut_delay);
                                                start = DateTime.Now;
                                                killed = true;
                                                hcount++;
                                                brk = 0;
                                            }
                                            else
                                                break;
                                        }
                                        msec = DateTime.Now.Subtract(start).TotalMilliseconds;
                                        /*
                                        if (KW.use_skill || KW.use_slist)
                                            KW.rclick = true;
                                        else
                                            KW.lclick = true;
                                            */
                                        //Thread.Sleep(AC.cut_delay);
                                    }
                                    prv = m;
                                }
                            }
                            catch { }
                            CA.tgs = new List<MobDetail>();
                        }
                    }
                }
                int act = k[CA.cBase].Read<short>(CA.c_E8);
                if (act == 1000)
                    walk_counter++;

                if (k[CA.cBase].Read<short>(CA.c_E8) == 1200)
                    walk_counter = 0;


                if (walk_counter > cnt_max)
                    KW.stop = true;
                //}
                Thread.Sleep(1);
            }

            KW.bot_hit = false;
            if ((chk_loot_enable.Checked || chk_include.Checked || chk_gold.Checked) && killed)
                if (!bg_grind_pick.IsBusy)
                    bg_grind_pick.RunWorkerAsync();
        }
        private void LD_bot(ref DataTable dtp)
        {
            DataTable tb_tar = new DataTable();
            tb_tar.Columns.Add("mob", typeof(int));
            int ext_1_y = CA.cInf.ext_1 + 0x4;
            int ext_2_y = CA.cInf.ext_2 + 0x4;
            decimal dec_x = 32;
            decimal dec_y = -32;
            int exit_counter = 0;
            int cnt_max = Convert.ToInt32(txt_m_w.Text);
            KW.stop = false;
            bool ld = chk_LD.Checked;
            BT.ld = true;
            while (app_exit == false && KW.start_force && CA.cInf._w_exp >= CA.cInf.ld_bot_min && BT.ld && ld)
            {
                bool gkey = Keyboard.IsKeyDown(Keys.LButton);
                int ttype = k.Memory.Read<byte>(AC.target_type); // khan.ReadByte(AC.target_type, (uint)1)[0];

                if (KW.bot)
                {
                    int cnt = BT.Mob.Near_Mob.Count();
                    //if (cnt <= 1 && IsAlive())
                    if (CA.tg.Flag != 128 && IsAlive())
                    {
                        Int64 ex1 = 0;
                        Int64 ex2 = 0;
                        NewCoord_LD(dtp, ref ex1, ref ex2, ref BT.cur_coord);
                        walk(ex1, ex2);
                        lb_cnt.Text = BT.cur_coord.ToString() + "/" + dtp.Rows.Count.ToString();

                        exit_counter = 0;
                        //walk_float(ex1, ex1);
                        Thread.Sleep(500);

                    }
                    else
                    {
                        k[CA.cBase].Write<short>(CA.c_pvp, 1000);
                        k[CA.cBase].Write<short>(CA.c_E8, 1000);
                    }

                    ld = CheckLD();
                    if (exit_counter > 30)
                        ld = false;
                    Thread.Sleep(200);
                }
                ld = CheckLD();
            }

        }
        private bool CheckLD()
        {
            bool ld = chk_LD.Checked;
            if (ld)
            {
                if (CA.cInf.ld_bot_min >= CA.cInf._w_exp)
                {
                    switch (BT.state)
                    {
                        case 0:
                        case 1:
                            if (chk_ld_path.Checked)
                            {
                                BT.state = 2;
                                BT.cur_coord = 0;
                            }
                            break;
                        default:
                            break;
                    }
                    ld = false;
                }
                else
                {
                    if (CA.cInf._w_exp >= CA.cInf.ld_bot_max && chk_ld_path.Checked)
                    {
                        BT.state = 2;
                        //BT.cur_coord = 0;
                    }
                    else
                        BT.state = 1;
                }
            }
            return ld;
        }
        private void rec_bot_DoWork(object sender, DoWorkEventArgs e)
        {
            IntPtr rbase = k[CA.MainBase].Read<IntPtr>(0x01450614);
            int map_x = 0x1B8;
            int map_y = 0x1BA;
            IntPtr tbase = k[CA.MainBase].Read<IntPtr>(0x00001788);
            uint[] tar_x = new uint[1] { 0x4A8 };
            uint[] tar_y = new uint[1] { 0x4AC };
            uint[] act_ext = new uint[1] { 0x77E };
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            Int64 ex1 = 0;
            Int64 ex2 = 0;
            int ox = 0;
            int oy = 0;
            int trace_count = 0;
            KW.bot_hit = false;

            while (BT.record)
            {

                bool key = Keyboard.IsKeyDown(Keys.End);
                if (key)
                {
                    int aid = k.Memory.Read<byte>(CA.cInf.Acc_Id) * 11; // khan.ReadByte(CA.cInf.Acc_Id, (uint)4), 0) * 4;
                    short rx = k[rbase].Read<short>(map_x); // khan.ReadMemory2Byte(rbase, map_x, 1);
                    short ry = k[rbase].Read<short>(map_y); //khan.ReadMemory2Byte(rbase, map_y, 1);
                    ex1 = k[CA.cBase].Read<Int64>(ext_1); //khan.ReadMemoryInt64(CA.cBase, ext_1, 1);
                    ex2 = k[CA.cBase].Read<Int64>(ext_2); //khan.ReadMemoryInt64(CA.cBase, ext_2, 1);
                    trace_count++;

                    ListViewItem lvi = new ListViewItem(cmb_state.SelectedIndex.ToString());
                    lvi.SubItems.Add(rx.ToString());
                    lvi.SubItems.Add(ry.ToString());
                    lvi.SubItems.Add(aid.ToString());
                    lvi.SubItems.Add(ex1.ToString());
                    lvi.SubItems.Add(ex2.ToString());

                    ListViewItem t_lvi = new ListViewItem(cmb_state.SelectedIndex.ToString());
                    t_lvi.SubItems.Add(rx.ToString());
                    t_lvi.SubItems.Add(ry.ToString());
                    if (BT.lst_route.Items.Count == 0)
                    {
                        BT.lst_route.Items.Insert(0, lvi);
                        //BT.lst_route.Items[0].Selected = true;
                        //BT.lst_route.Select();
                        lst_route.Items.Add(t_lvi);
                        lst_route.Items[0].Selected = true;
                        lst_route.Select();
                    }
                    else
                    {
                        int sel = lst_route.SelectedItems[0].Index + 1;
                        BT.lst_route.SelectedItems.Clear();
                        BT.lst_route.Items.Insert(sel, lvi);
                        //BT.lst_route.Items[sel].Selected = true;
                        //BT.lst_route.Select();
                        //for (int i = 0; i < lst_route.SelectedItems.Count; i++)
                        // {
                        //     lst_route.Items[i].Selected = false;
                        //    lst_route.Select();
                        // }
                        lst_route.SelectedItems.Clear();
                        lst_route.Items.Insert(sel, t_lvi);
                        lst_route.Items[sel].Selected = true;
                        lst_route.Select();
                    }
                    Thread.Sleep(100);
                }
                Thread.Sleep(50);
            }

        }

        public static void FromListView(DataTable table, ListView lvw)
        {
            table.Clear();
            var columns = lvw.Columns.Count;

            //foreach (ColumnHeader column in lvw.Columns)
            //    table.Columns.Add(column.Text);

            foreach (ListViewItem item in lvw.Items)
            {
                var cells = new object[columns];
                for (var i = 0; i < columns; i++)
                    cells[i] = item.SubItems[i].Text;
                table.Rows.Add(cells);
            }
        }

        private string get_map_name()
        {
            IntPtr map_base = k[CA.MainBase].Read<IntPtr>(0x0030EA88);
            int map_offset = 0x558;
            //IntPtr map_addr = khan.FindDmaAddy((int)map_base, map_offset, 1);
            string map_name = Readstr_off(map_base, map_offset, 14); //k[map_base].Read<string>(map_offset); // khan.ReadCString((int)map_addr, 22);
            return map_name;
        }
        private void reg_bot_acc()
        {
            int r_cnt = tab_bot_acc.Rows.Count;
            int aid = k.Memory.Read<byte>(CA.cInf.Acc_Id) * 11; //BitConverter.ToInt32(khan.ReadByte(CA.cInf.Acc_Id, (uint)4), 0) * 4;
            string map = get_map_name();
            tab_bot_acc.Clear();
            tab_bot_acc.Rows.Add(aid, map);
        }
        private void btn_save_bot_Click(object sender, EventArgs e)
        {

            fileSave.CreatePrompt = true;
            fileSave.OverwritePrompt = true;
            fileSave.Filter = "Bot files (*.bot)|*.bot";
            fileSave.InitialDirectory = KW.base_dir;
            DialogResult result = fileSave.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (fileSave.FileName != null)
                {
                    DialogResult resp = MessageBox.Show("Are you sure you want to save bot route?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        tab_new_route.Clear();
                        FromListView(tab_new_route, BT.lst_route);
                        //foreach(ListViewItem lvi in lst_route.Items) 
                        //    tab_new_route.Rows.Add(lvi); 
                        reg_bot_acc();
                        Cryptography.WriteXml(dta_bot, fileSave.FileName, "dmxtreme", "th3f0rc3", false);
                        //txt_bot_file.Text = fileSave.FileName;
                    }
                }
            }
        }

        public bool CheckMap()
        {
            bool ret = false;
            int aid = k.Memory.Read<byte>(CA.cInf.Acc_Id) * 11; //BitConverter.ToInt32(khan.ReadByte(CA.cInf.Acc_Id, (uint)4), 0) * 4;
            string map = get_map_name();
            foreach (DataRow dr in tab_bot_acc.Rows)
            {
                if (dr["account"].ToString() == aid.ToString() && dr["map"].ToString() == map)
                    ret = true;
            }
            return ret;
        }
        private void btn_open_map_Click(object sender, EventArgs e)
        {
            fileOpen.Filter = "Bot files (*.bot)|*.bot";
            fileOpen.InitialDirectory = KW.base_dir;
            DialogResult result = fileOpen.ShowDialog();
            if (result == DialogResult.OK)
            {
                //if (!chk_ld_path.Checked)
                // {
                DialogResult resp = MessageBox.Show("Are you sure you want to load this bot route?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {

                    tab_btemp.Rows.Clear();
                    tab_new_route.Clear();
                    string f_name = fileOpen.SafeFileName;
                    Cryptography.ReadXml(dta_bot, KW.base_dir + f_name, "dmxtreme", "th3f0rc3", false);
                    if (CheckMap())
                    {
                        txt_bot_file.Text = f_name;
                        btn_play_test.Enabled = true;
                    }
                    else
                    {
                        txt_bot_file.Text = "";
                        btn_play_test.Enabled = false;
                        MessageBox.Show("This route is not for this map or not for your account?", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
                // }/*
                // else
                // {
                //     DialogResult resp = MessageBox.Show("Are you sure you want to load this bot route?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //     if (resp == DialogResult.Yes)
                //     {
                //         txt_ld_path.Text = fileOpen.FileName;
                //     }
                // }
                // */
            }
        }

        private void perHP_KeyDown(object sender, KeyEventArgs e)
        {
            char c = Convert.ToChar(e.KeyValue);
            if (!char.IsDigit(c))
            {
                e.Handled = true;
            }
        }


        public void Load_Hk()
        {
            hk.AutoClick = GetHKSet("Auto Click");
            hk.ExpWatch = GetHKSet("Exp Watch");
            hk.ForceRemove = GetHKSet("Force Remove");
            hk.FastRun = GetHKSet("Fast Run");
            hk.Heal = GetHKSet("Heal");
            hk.MultiTarget = GetHKSet("Multi Target");
            hk.PickItem = GetHKSet("Pick Item");
            hk.UseSkill = GetHKSet("Use Skill");
            hk.LockOn = GetHKSet("Lock On");
            hk.UseSkillList = GetHKSet("Use Skill List");
            hk.SetItem = GetHKSet("Set Item");
            hk.Npc = GetHKSet("Npc");
            hk.Bot = GetHKSet("Start Bot");
            hk.No_Anim = GetHKSet("No Animation");
            hk.Start = GetHKSet("Start");
        }
        private void dg_hot_keys_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Load_Hk();
        }

        private void bg_hk_DoWork(object sender, DoWorkEventArgs e)
        {
            while (app_exit == false)
            {
                if (kPid > 0)
                {
                    if (KW.items)
                    {
                        if (Keyboard.IsKeyDown(hk.SetItem) && hk.SetItem != Keys.None)
                            AddItem();
                    }
                    if (KW.npc)
                    {
                        if (Keyboard.IsKeyDown(hk.Npc) && hk.Npc != Keys.None)
                            npc_act();
                    }
                    if (Keyboard.IsKeyDown(hk.Start) && hk.Start != Keys.None)
                    {
                        //KW.start_force = !KW.start_force;
                        //tfext.force_ext.SetButton(ref btnStart, "Start", "Pause");
                        btnStart_Click(this, e);
                    }
                    if (Keyboard.IsKeyDown(hk.No_Anim) && hk.No_Anim != Keys.None)
                    {
                        KW.no_anim = !KW.no_anim;
                        chk_NoAnim.Checked = KW.no_anim;
                    }
                    if (Keyboard.IsKeyDown(hk.LockOn) && hk.LockOn != Keys.None)
                    {
                        KW.lockon = !KW.lockon;
                        chk_Lock.Checked = KW.lockon;
                    }
                    if (Keyboard.IsKeyDown(hk.FastRun) && hk.FastRun != Keys.None)
                    {
                        KW.run_fast = !KW.run_fast;
                        chk_run_fast.Checked = KW.run_fast;
                    }
                    if (Keyboard.IsKeyDown(hk.MultiTarget) && hk.MultiTarget != Keys.None)
                    {
                        KW.mt.mtarget = !KW.mt.mtarget;
                        chk_MT.Checked = KW.mt.mtarget;
                    }
                    if (Keyboard.IsKeyDown(hk.Heal) && hk.Heal != Keys.None)
                    {
                        chk_HP.Checked = !chk_HP.Checked;
                        chk_MP.Checked = !chk_MP.Checked;
                    }
                    if (Keyboard.IsKeyDown(hk.ForceRemove) && hk.ForceRemove != Keys.None)
                    {
                        chk_rem_dbuff.Checked = !chk_rem_dbuff.Checked;
                    }
                    if (Keyboard.IsKeyDown(hk.ExpWatch) && hk.ExpWatch != Keys.None)
                    {
                        chk_stop_heal.Checked = !chk_stop_heal.Checked;
                    }
                    if (Keyboard.IsKeyDown(hk.AutoClick) && hk.AutoClick != Keys.None)
                    {
                        chk_aClick.Checked = !chk_aClick.Checked;
                    }
                    if (Keyboard.IsKeyDown(hk.PickItem) && hk.PickItem != Keys.None)
                    {
                        KW.a_pick = !KW.a_pick;
                        chk_Pick.Checked = KW.a_pick;
                    }
                    //// LDQ Bug
                    if (Keyboard.IsKeyDown(Keys.PageDown) && g_ldq_bug.Visible)
                    {
                        BT.ldq = !BT.ldq;
                        if (BT.ldq)
                        {
                            if (chk_ldq_clist.Checked && chk_ldq_cview.Checked && chk_ldq_recon.Checked)
                            {
                                toolz.GetInt(txt_esc_delay, ref BT.ldq_esc_delay);
                                if (!ldq_bug.IsBusy)
                                    ldq_bug.RunWorkerAsync();
                            }
                        }
                    }
                    if (Keyboard.IsKeyDown(hk.Bot) && hk.Bot != Keys.None)
                    {
                        if (Convert.ToInt32(tab_btemp.Rows.Count.ToString()) > 0)
                            btn_play_test_Click(sender, e);
                    }
                    if (KW.h_exp)
                    {
                        if (CA.cInf.perEXP <= CA.cInf._w_exp)
                        {
                            if (chk_stop_heal.Checked)
                            {
                                chk_HP.Checked = false;
                                chk_MP.Checked = false;
                                if (KW.mt.mtarget)
                                    chk_MT.Checked = false;

                                if (BT.play_temp)
                                    btn_play_test_Click(sender, e);
                            }
                            if (chk_pause_all.Checked)
                            {
                                if (BT.play_temp)
                                    btn_play_test_Click(sender, e);

                                if (KW.mt.mtarget)
                                    chk_MT.Checked = false;

                                //if (KW.start_force)
                                //    btnStart_Click(sender, e);
                            }
                        }
                    }
                    if (KW.bot_ld && BT.play_temp)
                    {
                        if (CA.cInf.ld_bot_max <= CA.cInf._w_exp)
                        {
                            chk_HP.Checked = false;
                            chk_MP.Checked = false;

                            if (KW.mt.mtarget)
                                chk_MT.Checked = false;

                            //if (BT.play_temp)
                            //    btn_play_test_Click(sender, e);

                            //if (chk_ld_path.Checked)
                            //{
                            if (!bg_ld_path.IsBusy)
                                bg_ld_path.RunWorkerAsync();
                            //}
                        }

                        if (CA.cInf.ld_bot_min >= CA.cInf._w_exp)
                        {
                            chk_HP.Checked = true;
                            chk_MP.Checked = true;

                            //if (Convert.ToInt32(lb_cnt.Text) > 0)
                            // if (!BT.play_temp)
                            //    btn_play_test_Click(sender, e);
                        }

                    }
                }
                Thread.Sleep(AC.hk_delay);
            }
        }
        public void LootItems()
        {
            IntPtr i_src = k[CA.MainBase].Read<IntPtr>(0x0039C3E8); //CA.MainBase + 0x0039C3E8;
            int[] i_offset = new int[2] { 0x10, 0x310 };
            int[] w_off = new int[1] { 0x790 };
            uint[] w_act_ext = new uint[1] { 0x794 };
            IntPtr Addr = GetAddress(i_src, i_offset); // (uint)khan.FindDmaAddy((int)i_src, i_offset, 2);
            IntPtr w_Addr = GetAddress(CA.cBase, w_off); //(uint)khan.FindDmaAddy((int)CA.cBase, w_off, 1);
            byte[] ret = k.Memory.Read<byte>(Addr, 8); // khan.ReadByte(Addr, 8);
            byte[] t_id = new byte[2] { ret[0], ret[1] };
            byte[] t_par = new byte[2] { ret[4], ret[5] };
            int tg = BitConverter.ToInt16(t_id, 0);
            int par = BitConverter.ToInt16(t_par, 0);
            int bw = 0;
            DataTable loot = tbl_loot_id as DataTable;
            IntPtr loot_base = k[CA.MainBase].Read<IntPtr>(0x0143D7DC); //CA.MainBase + 0x0143D7DC;
            int[] loot_offset = new int[2] { 0xD8, 0x20 };
            int scan_size = 10000;
            IntPtr addr = GetAddress(loot_base, loot_offset); //(uint)khan.FindDmaAddy((int)loot_base, loot_offset, 2);
            byte[] r_loot = k.Memory.Read<byte>(addr, scan_size); // khan.ReadByte(addr, (uint)scan_size);
            loot.Clear();
            for (int x = 0; x <= (scan_size - 8); x++)
            {
                if (r_loot[x] == 0x80 && r_loot[x + 1] == 0)
                {
                    byte[] id = new byte[2] { r_loot[x + 4], r_loot[x + 5] };
                    loot.Rows.Add(BitConverter.ToInt16(id, 0));
                    x += 7;
                }
            }
            foreach (DataRow dr in loot.Rows)
            {
                if (true)
                {
                    short val = Convert.ToInt16(dr["Id"].ToString());
                    k[CA.cBase].Write<short>(CA.cInf.Targ1, val);
                    k[CA.cBase].Write<short>(CA.cInf.Targ2, val);
                    k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 4);
                    k[CA.cBase].Write<short>(CA.c_pvp, 1300);
                    Thread.Sleep(10);
                }
            }
            loot.Rows.Clear();
        }

        private void chk_dead_pick_CheckedChanged(object sender, EventArgs e)
        {
            KW.dead_pick = chk_dead_pick.Checked;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        private void chk_LockPvp_CheckedChanged(object sender, EventArgs e)
        {
            KW.lock_pvp = chk_LockPvp.Checked;
            if (KW.lock_pvp)
                TF_Key.KeyDown(TF_Key.VK_CONTROL);
            else
                TF_Key.KeyUp(TF_Key.VK_CONTROL);
            // SendMessage((IntPtr)0x00030128, 0x51, 0, 0);
        }

        private void txt_m_timeout_TextChanged(object sender, EventArgs e)
        {
            //AC.m_timeout = Convert.ToInt16(txt_m_timeout.Text);
            toolz.GetInt(txt_m_timeout, ref AC.m_timeout);
        }

        private void txt_m_w_TextChanged(object sender, EventArgs e)
        {
            //AC.m_counter = Convert.ToInt32(txt_m_w.Text);
            toolz.GetInt(txt_m_w, ref AC.m_counter);
        }

        private void btn_rem_coord_Click(object sender, EventArgs e)
        {

        }

        private void chk_no_as_CheckedChanged(object sender, EventArgs e)
        {
            KW.no_as = chk_no_as.Checked;
        }



        private void chk_rand_walk_CheckedChanged(object sender, EventArgs e)
        {


            BT.rand_walk = chk_rand_walk.Checked;
        }

        private void chk_slow_imn_CheckedChanged(object sender, EventArgs e)
        {
            KW.slow_immunity = chk_slow_imn.Checked;
        }

        private void btn_reset_bot_Click(object sender, EventArgs e)
        {
            BT.cur_coord = 0;
            lb_cnt.Text = "0/0";
            //lb_txt.Text = "0;
        }

        private void chk_level_down_CheckedChanged(object sender, EventArgs e)
        {
            BT.ld_check = chk_level_down.Checked;
            //chk_EXP.Checked = BT.ld_check;
        }

        private List<cskill> CheckBuffs(cskill[] skl, Int16 buff = 0)
        {    
            int br; 
            List<cskill> ret = new List<cskill>();
            int nb = k[k[CA.MainBase].Read<IntPtr>(CA.cInf.buff.buff_mod)].Read<int>(CA.cInf.buff.n_buff);
            //if (nb > 0)
            //{
                nb = nb * 2;
                byte[] blist = k[k[CA.MainBase].Read<IntPtr>(CA.cInf.buff.buff_mod)].Read<byte>(CA.cInf.buff.l_buff, nb);
                foreach (cskill s in skl)
                {
                    bool fnd = false;
                    for (int i = 0; i < nb; i++)
                    {
                        int cbuff = BitConverter.ToInt16(blist, i);
                        if (cbuff == s.code)
                            fnd = true;
                            
                        i++;
                    }
                    if (!fnd)
                        ret.Add(s);
                }
            //} 
            return ret;
        }

        public bool IsOut()
        { 
            short loc = k[CA.cBase].Read<short>(CA.cInf.loc);// khan.ReadMemory2Byte(CA.cBase, CA.cInf.loc, 1); 
            foreach (int lc in CA.Safe)
            {
                if (lc == loc)
                    return false;
            }  
            return true;
        }

        public bool sIsOut()
        {
            short loc = k[CA.cBase].Read<short>(CA.cInf.loc);// khan.ReadMemory2Byte(CA.cBase, CA.cInf.loc, 1); 
            foreach (int lc in CA.Safe)
            {
                if (lc == loc)
                    return false;
            }
            return true;
        }
        private void bg_buff_DoWork(object sender, DoWorkEventArgs e)
        {
            int new_buff = 0;
            int cur_buff = k[CA.cBase].Read<int>(CA.cInf.buff.buf);

            while (KW.use_buff && KW.start_force)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (IsOut())
                    {
                        CA.cInf.buff.buff_count = 0;
                        int nb = k[k[CA.MainBase].Read<IntPtr>(CA.cInf.buff.buff_mod)].Read<int>(CA.cInf.buff.n_buff);
                        List<cskill> gskill = CheckBuffs(CA.cast_buff); 
                        foreach (cskill bf in gskill)
                        {  
                                int cur_skill = 0X74A;
                                int new_skill = 0x74C;
                                int c_skill = 0X74C;
                                k[CA.cBase].Write<short>(CA.c_E8, 1000);
                                Thread.Sleep(bf.delay);
                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 1);
                                k[CA.cBase].Write<int>(CA.c_skill, bf.code);
                                k[CA.cBase].Write<int>(cur_skill, bf.code);
                                k[CA.cBase].Write<int>(new_skill, bf.code);
                                k[CA.cBase].Write<short>(CA.cInf.Targ2, (short)CA.cInf.MyId);
                                k[CA.cBase].Write<short>(CA.c_pvp, 1800); 
                        } 
                    }
                    Thread.Sleep(20);
                }
                Thread.Sleep(1);
            }
        }

        private void chk_ubuf_CheckedChanged(object sender, EventArgs e)
        {
            KW.use_buff = chk_ubuf.Checked;
            if (KW.use_buff)
            {
                CA.cast_buff = null;
                CA.cast_buff = GetSkills("Buff");
                KW.nc_buff = CA.cast_buff.Count();
                CA.cast_buff_supp = null;
                CA.cast_buff_supp = GetSkills("Buff Support");
                KW.nc_buff_supp = CA.cast_buff_supp.Count();
                CA.cast_heal = null;
                CA.cast_heal = GetSkills("Heal");
                KW.nc_heal = CA.cast_heal.Count();
            }
        }

        private void btn_temp_Click(object sender, EventArgs e)
        {
            temp adm = new temp(ref tab_acc_prof, kPid);
            adm.ShowDialog();
        }

        private void loadskilllist()
        {
            bool s_found = false;
            foreach (DataRow dr in tab_char_skill.Rows)
            {
                if (dr["char"].ToString() == CA.cInf.name)
                {
                    s_found = true;
                    break;
                }
            }
            if (s_found)
            {
                tab_Skills.Clear();
                foreach (DataRow dr in tab_char_skill.Rows)
                {
                    if (dr["char"].ToString() == CA.cInf.name)
                    {
                        string atk = dr["Atk"].ToString();
                        if (atk == "")
                            atk = "false";
                        DataRow ndr = tab_Skills.NewRow();
                        ndr["Skill"] = dr.Field<string>("Skill");
                        ndr["Code"] = Convert.ToInt16(dr["Code"].ToString());
                        ndr["Delay"] = Convert.ToInt16(dr["Delay"].ToString());
                        ndr["Exec#"] = Convert.ToByte(dr["Exec#"].ToString());
                        ndr["Atk"] = Convert.ToBoolean(dr["Atk"].ToString());
                        ndr["Range"] = Convert.ToInt16(dr["Range"].ToString());
                        ndr["Type"] = dr.Field<string>("Type");

                        //ndr["Delay"] = DataRowExtensions.Field<short>(dr, "Delay");// dr.Field<short>("Delay");
                        //ndr["Exec#"] = dr.Field<byte>("Exec#");
                        //ndr["Atk"] = dr.Field<bool>("Atk");
                        //ndr["Range"] = dr.Field<short>("Range");
                        //ndr["Type"] = dr.Field<string>("Type");
                        tab_Skills.Rows.Add(ndr);
                        // tab_Skills.Rows.Add(dr.Field<string>("Skill"),dr.Field<short>("Code"),dr.Field<short>("Delay"),dr.Field<short>("Exec#"),dr.Field<bool>("Atk"),dr.Field<short>("Range"), dr.Field<string>("Type"));
                        //tab_Skills.Rows.Add(dr["Skill"].ToString(), dr["Code"].ToString(), dr["Delay"].ToString(), dr["Exec#"].ToString(), atk, dr.Field<short>("Range"), dr.Field<string>("Type"));
                        //tab_Skills.Rows.Add(dr["Skill"].ToString(), dr["Code"].ToString(), dr["Delay"].ToString(), dr["Exec#"].ToString(), atk,dr["Range"].ToString(), dr["Type"].ToString()); //, dr.Field<int>("Range"), dr.Field<string>("Type"));
                    }
                }
            }
            //else
            //    MessageBox.Show("No Skill List is found for this char.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void btn_open_cskill_Click(object sender, EventArgs e)
        {
            loadskilllist();
        }

        private void TheForce_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label13_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pHack_DrawItem(object sender, DrawItemEventArgs e)
        {
            // using (Brush br = new SolidBrush(tabColorDictionary[pHack.TabPages[e.Index]]))
            // {
            // Color the Tab Header
            //Pen red = new Pen(Color.Red);
            //e.Graphics.FillRectangle(red, e.Bounds);
            // swap our height and width dimensions
            var rotatedRectangle = new Rectangle(0, 0, e.Bounds.Height, e.Bounds.Width);

            // Rotate
            e.Graphics.ResetTransform();
            e.Graphics.RotateTransform(-90);

            // Translate to move the rectangle to the correct position.
            e.Graphics.TranslateTransform(e.Bounds.Left, e.Bounds.Bottom, System.Drawing.Drawing2D.MatrixOrder.Append);

            // Format String
            var drawFormat = new System.Drawing.StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            // Draw Header Text
            e.Graphics.DrawString(pHack.TabPages[e.Index].Text, e.Font, Brushes.Black, rotatedRectangle, drawFormat);
        }


        private void bg_get_coord_DoWork(object sender, DoWorkEventArgs e)
        {
            bool GetInfo = true;
            Point P_Mouse = new Point { X = 0, Y = 0 };
            Color P_Color = new Color();

            while (GetInfo)
            {
                if (Keyboard.IsKeyDown(Keys.PrintScreen))
                {
                    P_Mouse = toolz.GetCursorPosition();
                    P_Color = GetColorAt(P_Mouse);
                    GetInfo = false;
                }
                if (GetInfo == false)
                {
                    P_Mouse.X -= KW.KL.Left;
                    P_Mouse.Y -= KW.KL.Top;
                    switch (BT.prec)
                    {
                        case 0:
                            BT.scroll.Captured = true;
                            BT.scroll.XY = P_Mouse;
                            BT.scroll.RGB = P_Color;
                            chk_revive.Checked = true;
                            break;
                        case 1:
                            BT.rev.Captured = true;
                            BT.rev.XY = P_Mouse;
                            BT.rev.RGB = P_Color;
                            chk_revive.Checked = true;
                            break;
                        case 2:
                            BT.stat.Captured = true;
                            BT.stat.XY = P_Mouse;
                            BT.stat.RGB = P_Color;
                            chk_stats.Checked = true;
                            break;
                        case 3:
                            BT.trade.Captured = true;
                            BT.trade.XY = P_Mouse;
                            BT.trade.RGB = P_Color;
                            chk_reject_trade.Checked = true;
                            break;
                        case 4:
                            BT.ldq_clist.Captured = true;
                            BT.ldq_clist.XY = P_Mouse;
                            BT.ldq_clist.RGB = P_Color;
                            chk_ldq_clist.Checked = true;
                            break;
                        case 5:
                            BT.ldq_cview.Captured = true;
                            BT.ldq_cview.XY = P_Mouse;
                            BT.ldq_cview.RGB = P_Color;
                            chk_ldq_cview.Checked = true;
                            break;
                        case 6:
                            BT.ldq_recon.Captured = true;
                            BT.ldq_recon.XY = P_Mouse;
                            BT.ldq_recon.RGB = P_Color;
                            chk_ldq_recon.Checked = true;
                            break;
                        case 7:
                            BT.ldq_recon1.Captured = true;
                            BT.ldq_recon1.XY = P_Mouse;
                            BT.ldq_recon1.RGB = P_Color;
                            chk_ldq_recon.Checked = true;
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        private void btn_rec_revive_Click(object sender, EventArgs e)
        {
            if (chk_use_scroll.Checked)
                BT.prec = 0;
            else
                BT.prec = 1;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void btn_rec_stat_Click(object sender, EventArgs e)
        {
            BT.prec = 2;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void chk_LD_CheckedChanged(object sender, EventArgs e)
        {
            KW.bot_ld = chk_LD.Checked;
            txt_exp_min.Enabled = KW.bot_ld;
            txt_exp_max.Enabled = KW.bot_ld;
        }

        private void walk(Int64 ex1, Int64 ex2)
        {
            int act = k[CA.cBase].Read<short>(CA.c_E8);
            int act2 = k[CA.cBase].Read<short>(CA.c_pvp);
            bool pvp1 = act2 != 1200 && act2 != 1800;
            bool pvp2 = act != 1200 && act != 1800;
            if (pvp1 && pvp2 && CA.tgs.Count <= 1)
            {
                int ext_1 = 0x788;
                int ext_2 = 0x7A4;
                k.Memory.Write<byte>(CA.MainBase + 0x143D2AC, 2);
                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 0);
                k[CA.cBase].Write<int>(CA.Next_Flag, 0);
                k[CA.cBase].Write<short>(CA.Next_Action, 0);
                k[CA.cBase].Write<Int64>(ext_1, ex1);
                k[CA.cBase].Write<Int64>(ext_2, ex2);
                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
                k[CA.cBase].Write<short>(CA.c_pvp, 1100);
            }
        }

        public void WalkTo(int x, int y)
        {
            CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x);
            CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y);
            int rx = CA.cInf.char_x - x;
            int ry = CA.cInf.char_y - y;
            /*
            if (rx < 0)
                x = CA.cInf.char_x + 1;
            if (rx > 0)
                x = CA.cInf.char_x - 1;
            if (ry < 0)
                y = CA.cInf.char_y + 1;
            if (ry > 0)
                y = CA.cInf.char_y - 1;
                */
            if (rx < 1)
                x = x - 1;
            if (rx > 1)
                x = x + 1;
            if (ry < 1)
                y = y - 1;
            if (ry > 1)
                y = y + 1;
            float sx = (float)((x * dec_x) + 16);
            float sy = (float)((y * dec_y) + (-16));
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            int act = k[CA.cBase].Read<short>(CA.c_E8);
            int act2 = k[CA.cBase].Read<short>(CA.c_pvp);
            bool pvp1 = act2 != 1200 && act2 != 1800;
            bool pvp2 = act != 1200 && act != 1800;
            if (pvp1 && pvp2)
            {
                WalkToXX(x, y);
            }
        }

        public void WalkToXX<T>(T x, T y)
        {
            decimal dec_x = 32;
            decimal dec_y = -32;
            float fx = 0;
            float fy = 0;
            int ix = 0;
            int iy = 0;
            if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16))
            {
                ix = Convert.ToInt32(x.ToString());
                iy = Convert.ToInt32(y.ToString());
                fx = (float)((ix * dec_x));// + 16);
                fy = (float)((iy * dec_y));// + (-16));
            }
            else
            {
                fx = (float)Convert.ToDecimal(x.ToString());
                fy = (float)Convert.ToDecimal(y.ToString());
            }
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            int act = k[CA.cBase].Read<short>(CA.c_E8);
            int act2 = k[CA.cBase].Read<short>(CA.c_pvp);
            bool pvp1 = act2 != 1200 && act2 != 1800;
            bool pvp2 = act != 1200 && act != 1800;
            if (pvp1 && pvp2)
            {
                byte[] sfx = new byte[8];
                byte[] bfx = BitConverter.GetBytes(fx);
                Buffer.BlockCopy(bfx, 0, sfx, 0, 4);
                Buffer.BlockCopy(bfx, 0, sfx, 4, 4);
                byte[] sfy = new byte[8];
                byte[] bfy = BitConverter.GetBytes(fy);
                Buffer.BlockCopy(bfy, 0, sfy, 0, 4);
                Buffer.BlockCopy(bfy, 0, sfy, 4, 4);
                k.Memory.Write<byte>(CA.MainBase + 0x143D2AC, 2);
                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 0);
                k[CA.cBase].Write<int>(CA.Next_Flag, 0);
                k[CA.cBase].Write<short>(CA.Next_Action, 0);
                if (typeof(T) == typeof(int) || typeof(T) == typeof(Int16))
                {
                    k[CA.cBase].Write<float>(ext_1, fx);
                    k[CA.cBase].Write<float>(ext_1 + 4, fy);
                    k[CA.cBase].Write<float>(ext_2, fx);
                    k[CA.cBase].Write<float>(ext_2 + 4, fy);
                }
                else
                {
                    k[CA.cBase].Write<byte>(ext_1, sfx);
                    k[CA.cBase].Write<byte>(ext_2, sfy);
                }
                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
                k[CA.cBase].Write<short>(CA.c_pvp, 1100);
            }
        }

        public void WalkToFX(float x, float y)
        {
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            int act = k[CA.cBase].Read<short>(CA.c_E8);
            int act2 = k[CA.cBase].Read<short>(CA.c_pvp);
            bool pvp1 = act2 != 1200 && act2 != 1800;
            bool pvp2 = act != 1200 && act != 1800;
            if (pvp1 && pvp2)
            {
                byte[] sfx = new byte[8];
                byte[] bfx = BitConverter.GetBytes(x);
                Buffer.BlockCopy(bfx, 0, sfx, 0, 4);
                Buffer.BlockCopy(bfx, 0, sfx, 4, 4);
                byte[] sfy = new byte[8];
                byte[] bfy = BitConverter.GetBytes(y);
                Buffer.BlockCopy(bfy, 0, sfy, 0, 4);
                Buffer.BlockCopy(bfy, 0, sfy, 4, 4);
                Int64 ix = BitConverter.ToInt64(sfx, 0);
                Int64 iy = BitConverter.ToInt64(sfy, 0);
                k.Memory.Write<byte>(CA.MainBase + 0x143D2AC, 2);
                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 0);
                k[CA.cBase].Write<int>(CA.Next_Flag, 0);
                k[CA.cBase].Write<short>(CA.Next_Action, 0);
                k[CA.cBase].Write<Int64>(ext_1, ix);
                k[CA.cBase].Write<Int64>(ext_2, iy);
                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
                k[CA.cBase].Write<short>(CA.c_pvp, 1100);
            }
        }
        private void walk_float(float ex1, float ex2)
        {
            int ext_1 = 0x788;
            int ext_2 = 0x7A4;
            k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 0);
            k[CA.cBase].Write<float>(ext_1, ex1);
            k[CA.cBase].Write<float>(ext_2, ex2);
            k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 2);
            k[CA.cBase].Write<short>(CA.c_pvp, 1100);
        }
        public int LoadPath(string fname)
        {
            if (File.Exists(fname))
            {
                tab_btemp.Rows.Clear();
                Cryptography.ReadXml(dta_bot, fname, "dmxtreme", "th3f0rc3", false);
                BT.cur_coord = 0;
                return tab_btemp.Rows.Count;
            }
            return 0;
        }

        public static DataTable path_finderX(DataTable dt_s, string state)
        {
            DataTable ret = dt_s.Copy();
            tab_filter(ret, state);
            DataTable dret = dt_s.Clone();
            string filt = "State = " + state.ToString();
            List<DataRow> rfound = new List<DataRow>();
            rfound = dt_s.Select(filt).ToList();
            foreach (DataRow dr in rfound.ToList())
                dret.Rows.Add(dr.ItemArray);
            return ret;
        }

        public static DataTable path_finder(DataTable dt_s, string state)
        {
            DataTable dret = dt_s.Clone();
            string filt = "State = " + state.ToString();
            List<DataRow> rfound = new List<DataRow>();
            rfound = dt_s.Select(filt).ToList();

            foreach (DataRow dr in rfound.ToList())
                dret.Rows.Add(dr.ItemArray);
            return dret;
        }

        public static void tab_filter(DataTable dt, string filt)
        {
            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow row in dt.Rows)
            {
                if (row["State"].ToString() != filt)
                {
                    rowsToDelete.Add(row);
                }
            }

            foreach (DataRow row in rowsToDelete)
            {
                dt.Rows.Remove(row);
            }
        }

        private bool IsAlive()
        {
            bool ret = true;
            Point P;
            Color C;
            if (chk_use_scroll.Checked)
            {
                P = BT.scroll.XY;
                P.X += KW.KL.Left;
                P.Y += KW.KL.Top;
                C = BT.scroll.RGB;
            }
            else
            {
                P = BT.rev.XY;
                P.X += KW.KL.Left;
                P.Y += KW.KL.Top;
                C = BT.rev.RGB;
            }
            Color CL = new Color();
            try
            {
                CL = GetColorAt(P);
            }
            catch
            {
            }
            if (CL == C)
                ret = false;

            return ret;
        }

        private List<PathDetail> LoadRoute(DataTable rt)
        {
            List<PathDetail> ret = new List<PathDetail>();
            foreach (DataRow dr in rt.Rows)
            {
                PathDetail pd = new PathDetail();
                pd.X = Convert.ToInt16(dr["X"].ToString());
                pd.Y = Convert.ToInt16(dr["Y"].ToString());
                pd.F_X = Convert.ToInt64(dr["F_X"].ToString());
                pd.F_Y = Convert.ToInt64(dr["F_Y"].ToString());
                ret.Add(pd);
            }
            return ret;
        }
        private RouteDetail LoadBotRoute(DataTable dtb)
        {
            RouteDetail ret = new RouteDetail();
            ret.MoveTo.Clear();
            ret.Grind.Clear();
            ret.LD.Clear();
            //if (chk_move_path.Checked)
            ret.MoveTo = LoadRoute(path_finder(tab_new_route, "0"));
            //if (chk_grind_path.Checked)
            ret.Grind = LoadRoute(path_finder(tab_new_route, "1"));
            //if (chk_ld_path.Checked)
            ret.LD = LoadRoute(path_finder(tab_new_route, "2"));
            return ret;
        }
        private int ToCoord(List<PathDetail> pd, int idx)
        {
            bool IsWalk = false;

            IsWalk = GetDistance(CA.cInf.char_x, CA.cInf.char_y, pd[idx].X, pd[idx].Y) > 1;
            if (IsWalk)
            {
                //walk(pd[idx].F_X, pd[idx].F_Y);
                WalkToXX(pd[idx].X, pd[idx].Y);
                Thread.Sleep(100);
            }
            else
                idx++;

            if (pd.Count > 0)
            {
                if (idx > pd.Count - 1)
                    return 0;
            }
            return idx;
        }

        private void DoStop(int w = 0)
        {
            k[CA.cBase].Write<short>(CA.c_pvp, 1000);
            Thread.Sleep(w);
            k[CA.cBase].Write<int>(CA.c_E8, 1000);
        }

        private void lb_state_disp(List<PathDetail> rt, int idx)
        {
            string[] states = new string[] { "MoveTo: ", "Grind: ", "LD: " };
            lb_cnt.Text = states[CA.Route.State] + idx.ToString() + "/" + rt.Count.ToString();
        }

        private bool FindLDMobs(int range = 5, int tk = 4)
        {
            CA.Route.LDIdx = CA.Route.LD.Count();
            // Find 4 mobs
            CA.MonsLD.Clear();
            bool fnd = CA.cInf._w_exp < CA.cInf.ld_bot_min;
            while (/*CA.MonsLD.Count < tk*/ !fnd && CA.Route.State == 1 && CA.Route.Play && CA.Route.ld)
            {
                fnd = CA.cInf._w_exp < CA.cInf.ld_bot_min;
                try
                {
                    CA.MonsLD = CA.MonsScan
                                    .Where((MonsList, Range) => MonsList.Range <= range && MonsList.Id > 0 && MonsList.Flag >= 128)// && !MonsList.IsStuck)// && MonsList.Flag2 < 5) // && MonsList.Stuck == nul)
                                    .Take(tk).ToList();
                }
                catch
                {
                }
                if (CA.MonsLD.Count < tk - 1)
                    CA.Route.GrindIdx = ToCoord(CA.Route.Grind, CA.Route.GrindIdx);
                Thread.Sleep(100);

                return true;
            }
            return false;
        }

        private bool CharAlive()
        {
            return CA.cInf.curHP > 0;
        }

        private near_coord NearPath()
        {
            near_coord ret = new near_coord();
            foreach (PathDetail p in CA.Route.MoveTo)
            {
                double rng = GetDistance(CA.cInf.char_x, CA.cInf.char_y, p.X, p.Y);
                if (ret.Range >= rng)
                {
                    ret.State = 0;
                    ret.idx = CA.Route.MoveTo.IndexOf(p);
                    ret.Range = rng;
                }
            }
            foreach (PathDetail p in CA.Route.Grind)
            {
                double rng = GetDistance(CA.cInf.char_x, CA.cInf.char_y, p.X, p.Y);
                if (ret.Range >= rng)
                {
                    ret.State = 1;
                    ret.idx = CA.Route.Grind.IndexOf(p);
                    ret.Range = rng;
                }
            }
            return ret;
        }
        private bool IsArrived(List<PathDetail> pd, int idx)
        {
            return (GetDistance(CA.cInf.char_x, CA.cInf.char_y, pd[idx].X, pd[idx].Y) <= 1);
        }
        private void bg_ld_path_DoWork(object sender, DoWorkEventArgs e)
        {
            CA.Route = LoadBotRoute(tab_new_route);
            near_coord nCoord = NearPath();
            CA.Route.MoveToIdx = 0;
            CA.Route.GrindIdx = 0;
            CA.Route.LDIdx = 0;
            CA.Route.Play = true;
            CA.Route.hp = chk_HP.Checked;
            CA.Route.mp = chk_MP.Checked;
            bool Playing = BT.play_temp && KW.start_force;

            CA.Route.State = nCoord.State;
            switch (nCoord.State)
            {
                case 0:
                    CA.Route.MoveToIdx = nCoord.idx;
                    break;
                case 1:
                    CA.Route.GrindIdx = nCoord.idx;
                    break;
                default:
                    break;
            }
            lb_bot_state.Text = "Botting..";
            while (Playing)
            {
                Playing = BT.play_temp && KW.start_force;
                int nb = k[k[CA.MainBase].Read<IntPtr>(CA.cInf.buff.buff_mod)].Read<int>(CA.cInf.buff.n_buff);
                switch (CA.Route.State)
                {
                    case 0:
                        {
                            while (CA.Route.MoveToIdx < CA.Route.MoveTo.Count && Playing && CA.Route.Play && CharAlive())
                            {
                                Playing = BT.play_temp && KW.start_force;
                                if (!CharAlive())
                                    break;
                                else
                                {
                                    chk_HP.Checked = CA.Route.hp;
                                    chk_MP.Checked = CA.Route.mp;
                                }
                                if (!CheckMap())
                                    break;
                                if (CA.cInf.ld_bot_max <= CA.cInf._w_exp && chk_LD.Checked)
                                {
                                    if (chk_ld_path.Checked)
                                    {
                                        CA.Route.LDIdx = 0;
                                        CA.Route.State = 2;
                                        break;
                                    }
                                    else
                                    {
                                        if (chk_use_scroll.Checked)
                                        {
                                            while (CA.cInf._w_exp >= CA.cInf.ld_bot_min && Playing && CA.Route.Play)
                                            {
                                                Thread.Sleep(100);
                                            }
                                            CA.Route.ld = false;
                                        }
                                    }
                                }
                                CA.Route.MoveToIdx = ToCoord(CA.Route.MoveTo, CA.Route.MoveToIdx);
                                lb_state_disp(CA.Route.MoveTo, CA.Route.MoveToIdx);

                                if (CA.Route.MoveTo.Count <= CA.Route.MoveToIdx + 1)
                                {
                                    CA.Route.State = 1;
                                    CA.Route.GrindIdx = 0;
                                    break;
                                }
                            }
                            //CA.Route.MoveToIdx = 0;
                            if (CA.Route.MoveToIdx >= CA.Route.MoveTo.Count)
                                CA.Route.MoveToIdx = 0;
                            //DoStop(50);
                            break;
                        }
                    case 1:
                        {
                            while (CA.Route.GrindIdx < CA.Route.Grind.Count && Playing && CA.Route.Play)
                            {
                                Playing = BT.play_temp && KW.start_force;
                                if (!CharAlive())
                                    break;
                                if (!CheckMap())
                                    break;

                                KW.bot_hit = false;
                                if (CA.tgs.Count == 0 && !bg_MultiTarget_Bot.IsBusy)
                                {
                                    CA.Route.GrindIdx = ToCoord(CA.Route.Grind, CA.Route.GrindIdx);
                                }
                                else
                                    KW.bot_hit = true;

                                lb_state_disp(CA.Route.Grind, CA.Route.GrindIdx);

                                if (CA.cInf.ld_bot_max <= CA.cInf._w_exp && chk_LD.Checked)
                                {
                                    KW.bot_hit = false;
                                    CA.Route.ld = true;
                                    if (KW.slow_immunity)
                                        k[CA.cBase].Write<byte>(CA.cInf.run_off, 0);
                                    FindLDMobs(5,3);
                                }
                                else
                                {
                                    if (!CA.Route.ld)
                                    {
                                        if (KW.slow_immunity)
                                            k[CA.cBase].Write<byte>(CA.cInf.run_off, 1);

                                        chk_HP.Checked = CA.Route.hp;
                                        chk_MP.Checked = CA.Route.mp;
                                    }
                                    else
                                        break;
                                }
                                if (KW.bot_hit)
                                {
                                    if (!bg_MultiTarget_Bot.IsBusy)// && !heal_skill())
                                        bg_MultiTarget_Bot.RunWorkerAsync();
                                }
                                if (CA.Route.GrindIdx >= CA.Route.Grind.Count)
                                    CA.Route.GrindIdx = 0;
                            }

                            if (CA.Route.ld)
                            {
                                while (CharAlive())
                                {
                                    FindLDMobs(5,3);
                                    Thread.Sleep(100);
                                    if (!bg_grind_pick.IsBusy)
                                        bg_grind_pick.RunWorkerAsync();
                                }
                                if (chk_ld_path.Checked)
                                {
                                    CA.Route.LDIdx = 0;
                                    CA.Route.State = 2;
                                    break;
                                }
                                else
                                {
                                    if (chk_use_scroll.Checked)
                                    {
                                        while (CA.cInf._w_exp >= CA.cInf.ld_bot_min && Playing && CA.Route.Play)
                                        {
                                            FindLDMobs(5,3);
                                            Thread.Sleep(100);
                                            if (CharAlive())
                                            {
                                                if (!bg_grind_pick.IsBusy)
                                                    bg_grind_pick.RunWorkerAsync();
                                            }
                                        }
                                        CA.Route.ld = false;
                                    }
                                }
                            }
                            if (CA.Route.GrindIdx > CA.Route.Grind.Count)
                                CA.Route.GrindIdx = 0;
                            break;
                        }
                    case 2:
                        {
                            while (CA.Route.LDIdx < CA.Route.LD.Count && Playing && CA.Route.Play && CharAlive())
                            {
                                Playing = BT.play_temp && KW.start_force;
                                if (!CharAlive())
                                {
                                    CA.Route.LDIdx = 0;
                                    break;
                                }
                                if (!CheckMap())
                                    break;
                                CA.Route.LDIdx = ToCoord(CA.Route.LD, CA.Route.LDIdx);
                                lb_state_disp(CA.Route.LD, CA.Route.LDIdx);
                                if (CA.Route.LDIdx >= CA.Route.LD.Count)
                                {
                                    Thread.Sleep(100);
                                    if (!CharAlive())
                                    {
                                        CA.Route.State = 0;
                                        CA.Route.LDIdx = 0;
                                        break;
                                    }
                                }
                                if (CA.cInf._w_exp <= CA.cInf.ld_bot_min)
                                {
                                    CA.Route.LDIdx = 0;
                                    CA.Route.GrindIdx = 0;
                                    CA.Route.MoveToIdx = 0;
                                    CA.Route.State = 0;
                                    CA.Route.ld = false;
                                    break;
                                }
                                while (CA.Route.LDIdx + 1 == CA.Route.LD.Count && IsArrived(CA.Route.LD, CA.Route.LDIdx))
                                {
                                    Thread.Sleep(100);
                                    if (!CharAlive())
                                        break;
                                }

                            }
                            if (CA.cInf._w_exp <= CA.cInf.ld_bot_min)
                            {
                                CA.Route.LDIdx = 0;
                                CA.Route.GrindIdx = 0;
                                CA.Route.MoveToIdx = 0;
                                CA.Route.State = 0;
                                CA.Route.ld = false;
                                break;
                            }
                            if (CA.Route.LDIdx >= CA.Route.LD.Count)
                                CA.Route.LDIdx = 0;
                            //DoStop();
                            break;
                        }
                    case 10:
                        {
                            ts_bot.ToolTipText = "Bot is Paused (Press to Resume)";
                            lb_bot_state.Text = "Paused..";
                            DoStop(50);
                            while (CA.Route.State == 10 && Playing && !CA.Route.Play)
                            {
                                Thread.Sleep(100);
                            }
                            lb_bot_state.Text = "Botting..";
                            ts_bot.ToolTipText = "Bot is on (Press to Pause)";
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            CA.Route.Play = false;
            lb_bot_state.Text = "Stopped..";
            lb_bot_state.Text = "Coord State";
        }
        private void SaveOptions()
        {
            IntPtr Acc_Id = (IntPtr)0x00713164;
            string ai = (BitConverter.ToInt32(k.Memory.Read<byte>(Acc_Id, 4), 0) * 4).ToString();// khan.ReadByte(Acc_Id, (uint)4), 0) * 4).ToString();
            // check box // CA.cInf.name
            SetPrf_B(ai, CA.cInf.name, "chk_UseSkill", chk_UseSkill.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_NoAnim", chk_NoAnim.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_no_as", chk_no_as.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_HP", chk_HP.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_MP", chk_MP.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_stop_heal", chk_stop_heal.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_pause_all", chk_pause_all.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_ld_path", chk_ld_path.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_uskill", chk_uskill.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_move_path", chk_move_path.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_grind_path", chk_grind_path.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_use_scroll", chk_use_scroll.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_include", chk_include.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_ubuf", chk_ubuf.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_loot_enable", chk_loot_enable.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_gold", chk_gold.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_boost", chk_boost.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_slow_imn", chk_slow_imn.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_hit_on", chk_hit_on.Checked);
            //SetPrf_B(ai, CA.cInf.name, "chk_dead_pick", chk_dead_pick.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_LD", chk_LD.Checked);

            SetPrf_B(ai, CA.cInf.name, "chk_pot_hp", chk_pot_hp.Checked);
            SetPrf_B(ai, CA.cInf.name, "chk_pot_mp", chk_pot_mp.Checked);

            // Text Box 
            SetPrf_I(ai, CA.cInf.name, "txt_ngold", txt_ngold.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_grange", txt_grange.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_boost", txt_boost.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txtRange", txtRange.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_pot_delay", txt_pot_delay.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_buf_delay", txt_buf_delay.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_aClick_DC_Delay", txt_aClick_DC_Delay.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_aClickC_Delay1", txt_aClickC_Delay1.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_aClickC_Delay2", txt_aClickC_Delay2.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_hk", txt_hk.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "perHP", perHP.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "perMP", perMP.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "perExp", perExp.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_m_w", txt_m_w.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_exp_max", txt_exp_max.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_exp_min", txt_exp_min.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_loot_pass", txt_loot_pass.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_num_mobs", txt_num_mobs.Text.ToString());
            SetPrf_I(ai, CA.cInf.name, "txt_num_hits", txt_num_hits.Text.ToString());

            // File paths
            SetPrf_S(ai, CA.cInf.name, "txt_bot_file", txt_bot_file.Text.ToString());
            //SetPrf_S(ai, CA.cInf.name, "txt_ld_path", txt_ld_path.Text.ToString());
            /// BT
            SetBT(ai, CA.cInf.name, "scroll", ref BT.scroll);
            SetBT(ai, CA.cInf.name, "rev", ref BT.rev);
            SetBT(ai, CA.cInf.name, "stat", ref BT.stat);
            SetBT(ai, CA.cInf.name, "trade", ref BT.trade);

            SetBT(ai, CA.cInf.name, "ldq_clist", ref BT.ldq_clist);
            SetBT(ai, CA.cInf.name, "ldq_cview", ref BT.ldq_cview);
            SetBT(ai, CA.cInf.name, "ldq_recon", ref BT.ldq_recon);
            SetBT(ai, CA.cInf.name, "ldq_recon1", ref BT.ldq_recon1);
        }
        private void LoadOptions()
        {
            IntPtr Acc_Id = (IntPtr)0x00713164;
            string ai = (BitConverter.ToInt32(k.Memory.Read<byte>(Acc_Id, 4), 0) * 4).ToString();//(BitConverter.ToInt32(khan.ReadByte(Acc_Id, (uint)4), 0) * 4).ToString();
            bool found = false;
            int[] pflg = new int[] { 0x6D5 };
            IntPtr c_add = GetAddress(CA.cBase, pflg);// khan.FindDmaAddy((int)CA.cBase, CA.cInf.d_pick.class_flag, 1);
            int br = 0;
            KW.MyClass = k[CA.cBase].Read<byte>(CA.cInf.d_pick.class_flag); // khan.ReadMemory(c_add, 1, out br)[0];
            cmb_class_item.SelectedIndex = KW.MyClass;
            cmb_class_item.Select();
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == ai && dr["char"].ToString() == CA.cInf.name)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                // check box
                chk_UseSkill.Checked = GetPrf_B(ai, CA.cInf.name, "chk_UseSkill");
                chk_NoAnim.Checked = GetPrf_B(ai, CA.cInf.name, "chk_NoAnim");
                chk_no_as.Checked = GetPrf_B(ai, CA.cInf.name, "chk_no_as");
                chk_HP.Checked = GetPrf_B(ai, CA.cInf.name, "chk_HP");
                chk_MP.Checked = GetPrf_B(ai, CA.cInf.name, "chk_MP");
                chk_stop_heal.Checked = GetPrf_B(ai, CA.cInf.name, "chk_stop_heal");
                chk_pause_all.Checked = GetPrf_B(ai, CA.cInf.name, "chk_pause_all");
                chk_ld_path.Checked = GetPrf_B(ai, CA.cInf.name, "chk_ld_path");
                chk_uskill.Checked = GetPrf_B(ai, CA.cInf.name, "chk_uskill");
                chk_move_path.Checked = GetPrf_B(ai, CA.cInf.name, "chk_move_path");
                chk_grind_path.Checked = GetPrf_B(ai, CA.cInf.name, "chk_grind_path");
                chk_use_scroll.Checked = GetPrf_B(ai, CA.cInf.name, "chk_use_scroll");
                chk_include.Checked = GetPrf_B(ai, CA.cInf.name, "chk_include");
                chk_ubuf.Checked = GetPrf_B(ai, CA.cInf.name, "chk_ubuf");
                chk_loot_enable.Checked = GetPrf_B(ai, CA.cInf.name, "chk_loot_enable");
                chk_gold.Checked = GetPrf_B(ai, CA.cInf.name, "chk_gold");
                chk_boost.Checked = GetPrf_B(ai, CA.cInf.name, "chk_boost");
                chk_slow_imn.Checked = GetPrf_B(ai, CA.cInf.name, "chk_slow_imn");
                chk_hit_on.Checked = GetPrf_B(ai, CA.cInf.name, "chk_hit_on");
                //chk_dead_pick.Checked = GetPrf_B(ai, CA.cInf.name, "chk_dead_pick");
                chk_LD.Checked = GetPrf_B(ai, CA.cInf.name, "chk_LD");
                chk_pot_hp.Checked = GetPrf_B(ai, CA.cInf.name, "chk_pot_hp");
                chk_pot_mp.Checked = GetPrf_B(ai, CA.cInf.name, "chk_pot_mp");

                // text box  
                txt_ngold.Text = GetPrf_S(ai, CA.cInf.name, "txt_ngold");
                txt_grange.Text = GetPrf_S(ai, CA.cInf.name, "txt_grange");
                txt_boost.Text = GetPrf_S(ai, CA.cInf.name, "txt_boost");
                txtRange.Text = GetPrf_S(ai, CA.cInf.name, "txtRange");
                txt_pot_delay.Text = GetPrf_S(ai, CA.cInf.name, "txt_pot_delay");
                txt_buf_delay.Text = GetPrf_S(ai, CA.cInf.name, "txt_buf_delay");
                txt_aClick_DC_Delay.Text = GetPrf_S(ai, CA.cInf.name, "txt_aClick_DC_Delay");
                txt_aClickC_Delay1.Text = GetPrf_S(ai, CA.cInf.name, "txt_aClickC_Delay1");
                txt_aClickC_Delay2.Text = GetPrf_S(ai, CA.cInf.name, "txt_aClickC_Delay2");
                txt_hk.Text = GetPrf_S(ai, CA.cInf.name, "txt_hk");
                perHP.Text = GetPrf_S(ai, CA.cInf.name, "perHP");
                perMP.Text = GetPrf_S(ai, CA.cInf.name, "perMP");
                perExp.Text = GetPrf_S(ai, CA.cInf.name, "perExp");
                txt_m_w.Text = GetPrf_S(ai, CA.cInf.name, "txt_m_w");
                txt_exp_max.Text = GetPrf_S(ai, CA.cInf.name, "txt_exp_max");
                txt_exp_min.Text = GetPrf_S(ai, CA.cInf.name, "txt_exp_min");
                txt_bot_file.Text = GetPrf_S(ai, CA.cInf.name, "txt_bot_file");
                txt_loot_pass.Text = GetPrf_S(ai, CA.cInf.name, "txt_loot_pass");
                txt_num_hits.Text = GetPrf_S(ai, CA.cInf.name, "txt_num_hits");
                txt_num_mobs.Text = GetPrf_S(ai, CA.cInf.name, "txt_num_mobs");
                // txt_ld_path.Text = GetPrf_S(ai, CA.cInf.name, "txt_ld_path");

                GetBT(ai, CA.cInf.name, "scroll", ref BT.scroll);
                GetBT(ai, CA.cInf.name, "rev", ref BT.rev);
                GetBT(ai, CA.cInf.name, "stat", ref BT.stat);
                GetBT(ai, CA.cInf.name, "trade", ref BT.trade);

                GetBT(ai, CA.cInf.name, "ldq_clist", ref BT.ldq_clist);
                GetBT(ai, CA.cInf.name, "ldq_cview", ref BT.ldq_cview);
                GetBT(ai, CA.cInf.name, "ldq_recon", ref BT.ldq_recon);
                GetBT(ai, CA.cInf.name, "ldq_recon1", ref BT.ldq_recon1);

                if (txt_bot_file.Text != "")
                {
                    if (File.Exists(KW.base_dir + txt_bot_file.Text))
                    {
                        Cryptography.ReadXml(dta_bot, KW.base_dir + txt_bot_file.Text, "dmxtreme", "th3f0rc3", false);
                        lb_cnt.Text = "0/" + tab_btemp.Rows.Count.ToString();
                        if (lb_cnt.Text != "" || lb_cnt.Text != "0/0")
                        {
                            btn_play_test.Enabled = true;
                            //BT.cur_coord = 0;
                        }
                        else
                        {
                            btn_play_test.Enabled = false;
                            //BT.cur_coord = 0;
                        }
                    }
                }
            }
            loadskilllist();
        }
        private void SetPrf_B(string acc, string chr, string opt, bool value)
        {
            bool found = false;
            string val = "0";
            if (value == true)
                val = "1";
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == acc && dr["char"].ToString() == chr && dr["Option"].ToString() == opt)
                {
                    dr.BeginEdit();
                    dr["Type"] = "bool";
                    dr["Value"] = val;
                    dr.EndEdit();
                    found = true;
                    break;
                }
            }
            if (!found)
                tab_char_options.Rows.Add(chr, opt, "bool", val, acc);
        }

        private void SetPrf_I(string acc, string chr, string opt, string val)
        {
            bool found = false;
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == acc && dr["char"].ToString() == chr && dr["Option"].ToString() == opt)
                {
                    dr.BeginEdit();
                    dr["Type"] = "int";
                    dr["Value"] = val;
                    dr.EndEdit();
                    found = true;
                    break;
                }
            }
            if (!found)
                tab_char_options.Rows.Add(chr, opt, "int", val, acc);
        }
        private void SetPrf_S(string acc, string chr, string opt, string val)
        {
            bool found = false;
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == acc && dr["char"].ToString() == chr && dr["Option"].ToString() == opt)
                {
                    dr.BeginEdit();
                    dr["Type"] = "string";
                    dr["Value"] = val;
                    dr.EndEdit();
                    found = true;
                    break;
                }
            }
            if (!found)
                tab_char_options.Rows.Add(chr, opt, "string", val, acc);
        }
        private bool GetPrf_B(string acc, string chr, string opt)
        {
            string type = "";
            string val = "0";
            bool ret = false;
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == acc && dr["char"].ToString() == chr && dr["Option"].ToString() == opt)
                {
                    type = dr["Type"].ToString();
                    val = dr["Value"].ToString();
                    break;
                }
            }
            if (type == "bool")
                if (val == "1")
                    ret = true;
            return ret;
        }
        private string GetPrf_S(string acc, string chr, string opt)
        {
            string type = "";
            string val = "";
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == acc && dr["char"].ToString() == chr && dr["Option"].ToString() == opt)
                {
                    type = dr["Type"].ToString();
                    val = dr["Value"].ToString();
                    break;
                }
            }
            if (type == "")
                val = "0";

            return val;
        }
        private int GetPrf_I(string acc, string chr, string opt)
        {
            string type = "";
            int val = 0;
            foreach (DataRow dr in tab_char_options.Rows)
            {
                if (dr["acc"].ToString() == acc && dr["char"].ToString() == chr && dr["Option"].ToString() == opt)
                {
                    type = dr["Type"].ToString();
                    val = Convert.ToInt32(dr["Value"].ToString());
                    break;
                }
            }
            if (type == "")
                val = 0;

            return val;
        }
        private void SetBT(string acc, string chr, string vr, ref CScreen cbt)
        {
            IntPtr Acc_Id = (IntPtr)0x00713164;
            vr = vr + "-";
            string ai = (BitConverter.ToInt32(k.Memory.Read<byte>(Acc_Id, 4), 0) * 4).ToString();//(BitConverter.ToInt32(khan.ReadByte(Acc_Id, (uint)4), 0) * 4).ToString();
            SetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.A", cbt.RGB.A.ToString());
            SetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.R", cbt.RGB.R.ToString());
            SetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.G", cbt.RGB.G.ToString());
            SetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.B", cbt.RGB.B.ToString());
            SetPrf_I(ai, CA.cInf.name, vr + "cbt.XY.X", cbt.XY.X.ToString());
            SetPrf_I(ai, CA.cInf.name, vr + "cbt.XY.Y", cbt.XY.Y.ToString());
            SetPrf_B(ai, CA.cInf.name, vr + "cbt.Captured", cbt.Captured);
        }
        private void GetBT(string acc, string chr, string vr, ref CScreen cbt)
        {
            IntPtr Acc_Id = (IntPtr)0x00713164;

            int a, r, g, b, x, y;

            vr = vr + "-";
            string ai = (BitConverter.ToInt32(k.Memory.Read<byte>(Acc_Id, 4), 0) * 4).ToString();//(BitConverter.ToInt32(khan.ReadByte(Acc_Id, (uint)4), 0) * 4).ToString();
            a = GetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.A");
            r = GetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.R");
            g = GetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.G");
            b = GetPrf_I(ai, CA.cInf.name, vr + "cbt.RGB.B");
            x = GetPrf_I(ai, CA.cInf.name, vr + "cbt.XY.X");
            y = GetPrf_I(ai, CA.cInf.name, vr + "cbt.XY.Y");
            cbt.Captured = GetPrf_B(ai, CA.cInf.name, vr + "cbt.Captured");
            cbt.XY = new Point { X = x, Y = y };
            cbt.RGB = Color.FromArgb(a, r, g, b);
        }

        private void txtRange_TextChanged(object sender, EventArgs e)
        {
            toolz.GetInt(txtRange, ref CA.cInf.c_range);
        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            Int64 x, y;
            int idx = lst_route.SelectedItems[0].Index;
            x = Convert.ToInt64(BT.lst_route.Items[idx].SubItems[4].Text);
            y = Convert.ToInt64(BT.lst_route.Items[idx].SubItems[5].Text);
            walk(x, y);
            //tele(x, y);
        }

        private void lst_route_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lst_route.SelectedItems.Count > 0)
                {
                    int idx = lst_route.SelectedItems[0].Index;
                    BT.lst_route.Items[idx].Remove();
                    lst_route.Items[idx].Remove();
                }
            }
        }

        private static void TableToListView(DataTable table, ListView lst)
        {
            foreach (DataRow row in table.Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                for (int i = 1; i < table.Columns.Count; i++)
                {
                    item.SubItems.Add(row[i].ToString());
                }
                lst.Items.Add(item);
            }
        }



        private void btn_open_route_Click(object sender, EventArgs e)
        {
            fileOpen.Filter = "Bot files (*.bot)|*.bot";
            fileOpen.InitialDirectory = KW.base_dir;
            DialogResult result = fileOpen.ShowDialog();
            if (result == DialogResult.OK)
            {
                //if (!chk_ld_path.Checked)
                //{
                DialogResult resp = MessageBox.Show("Are you sure you want to load this bot route?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {

                    tab_new_route.Clear();
                    BT.lst_route.Items.Clear();
                    lst_route.Items.Clear();
                    Cryptography.ReadXml(dta_bot, fileOpen.FileName, "dmxtreme", "th3f0rc3", false);
                    foreach (DataRow row in tab_new_route.Rows)
                    {
                        ListViewItem item = new ListViewItem(row[0].ToString());
                        ListViewItem t_item = new ListViewItem(row[0].ToString());
                        for (int i = 1; i < tab_new_route.Columns.Count; i++)
                        {
                            item.SubItems.Add(row[i].ToString());
                            if (i < 3)
                                t_item.SubItems.Add(row[i].ToString());
                        }
                        BT.lst_route.Items.Add(item);
                        lst_route.Items.Add(t_item);
                    }
                    /*
                    foreach (DataRow dt in tab_new_route.Rows)
                    {
                        string[] ar = Array.ConvertAll<object, string>(dt.ItemArray, p => p.ToString());
                        lst_route.Items.Add(new ListViewItem(ar));
                    }
                    */
                    // }
                }/*
                else
                {
                    DialogResult resp = MessageBox.Show("Are you sure you want to load this bot route?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resp == DialogResult.Yes)
                    {
                        txt_ld_path.Text = fileOpen.FileName;
                    }
                }*/
            }
        }

        private void chk_ld_path_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ld_path.Checked)
            {
                chk_use_scroll.Checked = !chk_ld_path.Checked;
                chk_move_path.Checked = chk_ld_path.Checked;
            }
        }

        private void chk_use_scroll_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_use_scroll.Checked)
                chk_ld_path.Checked = !chk_use_scroll.Checked;
        }

        private string FindItemInTable(string itm)
        {
            string ret = "";
            foreach (DataRow dr in tab_khan_items.Rows)
            {
                if (dr["Id"].ToString() == itm)
                {
                    ret = dr["Name"].ToString();
                    break;
                }
            }
            return ret;
        }

        public bool FindLootItem(ListView lv, string id)
        {
            bool ret = false;
            foreach (ListViewItem itm in lv.Items)
            {
                if (itm.SubItems[0].Text == id)
                {
                    //ret = true;
                    //break;
                    return true;
                }
                /*
                if (itm.SubItems[0].Text == "0")
                {
                    return false; 
                }
                */
            }
            return ret;
        }

        public static string DTFindLootItem(DataTable dt, string id)
        {
            string ret = null;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Id"].ToString() == id)
                {
                    ret = dr["Name"].ToString();
                    break;
                }
            }
            return ret;
        }
        private void bg_capture_DoWork(object sender, DoWorkEventArgs e)
        {
            while (chk_capture.Checked)
            {
                int itm = k[CA.add_pot].Read<short>(CA.cInf.Pot);// khan.ReadMemory2Byte(CA.add_pot, CA.cInf.Pot, 1); 

                if (Keyboard.IsKeyDown(Keys.Home))
                {
                    //ListViewItem item = new ListViewItem(row[0].ToString())
                    bool item = FindLootItem(BT.lst_loot_items, itm.ToString());
                    if (!item)
                    {
                        string name = DTFindLootItem(tab_khan_items, itm.ToString());
                        ListViewItem add_item = new ListViewItem(itm.ToString());
                        ListViewItem t_add_item = new ListViewItem(name);
                        add_item.SubItems.Add(name);
                        BT.lst_loot_items.Items.Add(add_item);
                        lst_loot.Items.Add(t_add_item);
                    }
                    Thread.Sleep(100);
                }
            }
        }

        private void chk_capture_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_capture.Checked)
                if (!bg_capture.IsBusy)
                    bg_capture.RunWorkerAsync();
        }

        private void lst_loot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_loot.SelectedItems.Count > 0)
            {
                int idx = lst_loot.SelectedItems[0].Index;
                //txt_l_name.ReadOnly = false;
                //txt_l_id.Text = lst_loot.Items[idx].SubItems[0].Text.ToString();
                //txt_l_name.Text = lst_loot.Items[idx].SubItems[1].Text.ToString();
            }
            else
            {
                //txt_l_id.Text = "";
                //txt_l_name.Text = "";
                //txt_l_name.ReadOnly = true;
            }
        }

        private void txt_l_name_TextChanged(object sender, EventArgs e)
        {

            /*
            if (lst_loot.SelectedItems.Count > 0)
            {
                int idx = lst_loot.SelectedItems[0].Index;
                lst_loot.Items[idx].SubItems[1].Text = txt_l_name.Text;
            }
            */
        }

        private void lst_loot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (lst_loot.SelectedItems.Count > 0)
                {
                    int idx = lst_loot.SelectedItems[0].Index;
                    BT.lst_loot_items.Items[idx].Remove();
                    lst_loot.Items[idx].Remove();
                }
            }
        }

        private void btn_my_account_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Are you sure you want to set current route this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                int aid = BitConverter.ToInt32(k.Memory.Read<byte>(CA.cInf.Acc_Id, 4), 0) * 4; // khan.ReadByte(CA.cInf.Acc_Id, (uint)4), 0) * 4;
                for (int idx = 0; idx < BT.lst_route.Items.Count; idx++)
                    BT.lst_route.Items[idx].SubItems[3].Text = aid.ToString();
            }

        }

        private void btn_reject_trade_Click(object sender, EventArgs e)
        {

            BT.prec = 3;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();

        }

        private void bg_grind_pick_DoWork_New(object sender, DoWorkEventArgs e)
        {

            int rng = 0;
            toolz.GetInt(txt_grange, ref rng);
            int ng = 0;
            toolz.GetInt(txt_ngold, ref ng);
            int bw = 0;
            IntPtr loot_base = k[CA.MainBase].Read<IntPtr>(0x0143D7DC); //CA.MainBase + 0x0143D7DC;
            //int[] loot_offset = new int[2] { 0xD8, 0x20 };
            //loot_base = IntPtr.Add(loot_base, loot_offset[0]);
            //loot_base = IntPtr.Add(loot_base, loot_offset[1]);
            IntPtr addr = k[loot_base].Read<IntPtr>(0xD8);
            addr = IntPtr.Add(addr, 0x20);
            int pick_pass = 0;
            toolz.GetInt(txt_loot_pass, ref pick_pass);
            int npick = pick_pass;
            int gold = 1581;
            int len = 0x67;
            Thread.Sleep(1000);
            bool ii = chk_include.Checked;
            bool ei = chk_exclude.Checked;
            bool gi = chk_gold.Checked;
            while ((chk_loot_enable.Checked && npick != 0) || KW.a_pick)
            {
                //try
                //{
                if ((chk_loot_enable.Checked && npick != 0) || KW.a_pick)
                {
                    List<l_items> lst = new List<l_items>();
                    for (int i = 0; i < 105; i++)
                    {
                        try
                        {
                            int ad = i * 104;
                            int flag1 = k.Memory.Read<byte>(addr + ad);
                            int flag2 = k.Memory.Read<byte>(addr + ad + 96);
                            if (flag1 == 128)
                            {
                                l_items add_i = new l_items();
                                add_i.Address = addr + ad;
                                add_i.Loot_Id = k.Memory.Read<short>(addr + ad + 4);
                                add_i.Id = k.Memory.Read<short>(addr + ad + 10);
                                add_i.X = k.Memory.Read<short>(addr + ad + 6);
                                add_i.Y = k.Memory.Read<short>(addr + ad + 8);
                                add_i.Range = GetDistance(CA.cInf.char_x, CA.cInf.char_y, add_i.X, add_i.Y);
                                lst.Add(add_i);
                            }
                        }
                        catch { }
                    }
                    foreach (l_items add_i in lst)
                    {
                        bool get = false;
                        if (add_i.Range <= rng)
                        {
                            if (add_i.Id == gold)
                            {
                                get = gi;
                            }
                            else
                            {
                                if (ei || ii)
                                {
                                    get = FindLootItem(BT.lst_loot_items, add_i.Id.ToString());
                                    if (ei)
                                        get = false;
                                }
                            }
                            if (get)
                            {
                                k[CA.cBase].Write<short>(CA.cInf.Targ1, add_i.Loot_Id);
                                k[CA.cBase].Write<short>(CA.cInf.Targ2, add_i.Loot_Id);
                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 4);
                                k[CA.cBase].Write<short>(CA.c_pvp, 1300);
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }


        private void bg_grind_pick_DoWork(object sender, DoWorkEventArgs e)
        {
            IntPtr i_src = k[CA.MainBase].Read<IntPtr>(0x0039C3E8); //CA.MainBase + 0x0039C3E8;
            int[] i_offset = new int[2] { 0x10, 0x310 };
            int[] w_off = new int[1] { 0x790 };
            uint[] w_act_ext = new uint[1] { 0x794 };
            IntPtr Addr = GetAddress(i_src, i_offset); // (uint)khan.FindDmaAddy((int)i_src, i_offset, 2);
            IntPtr w_Addr = GetAddress(CA.cBase, w_off); //(uint)khan.FindDmaAddy((int)CA.cBase, w_off, 1);
            byte[] ret = k.Memory.Read<byte>(Addr, 8); // khan.ReadByte(Addr, 8);
            byte[] t_id = new byte[2] { ret[0], ret[1] };
            byte[] t_par = new byte[2] { ret[4], ret[5] };
            int tg = BitConverter.ToInt16(t_id, 0);
            int par = BitConverter.ToInt16(t_par, 0);
            int rng = 0;
            toolz.GetInt(txt_grange, ref rng);
            int ng = 0;
            toolz.GetInt(txt_ngold, ref ng);
            int bw = 0;
            IntPtr loot_base = k[CA.MainBase].Read<IntPtr>(0x0143D7DC); //CA.MainBase + 0x0143D7DC;
            int[] loot_offset = new int[2] { 0xD8, 0x20 };
            int scan_size = 0x2A70;
            AOBScan newscan = new AOBScan((uint)kPid);
            IntPtr addr = CA.cInf.loot_address;
            if (addr == (IntPtr)0x0)
                addr = GetAddress(loot_base, loot_offset); // (uint)khan.FindDmaAddy((int)loot_base, loot_offset, 2);
            toolz.GetInt(txt_loot_pass, ref BT.npick);
            int npick = BT.npick;
            byte[] type_id;
            int gold = 1581;
            int len = 0x67;
            Thread.Sleep(1000);
            while ((chk_loot_enable.Checked && npick != 0) || KW.a_pick)
            {
                try
                {
                    if ((chk_loot_enable.Checked && npick != 0) || KW.a_pick)
                    {
                        byte[] pat = new byte[] { 0x80, 0x0 };
                        int jmp = 14;
                        List<int[]> Loots = newscan.GetGoldId((IntPtr)addr, scan_size, pat, jmp);
                        List<int[]> dl = new List<int[]>();
                        // Remove not in range 
                        foreach (int[] itm in Loots)
                        {
                            if (GetDistance(CA.cInf.char_x, CA.cInf.char_y, itm[2], itm[3]) > rng)
                                dl.Add(itm);
                        }
                        // Remove Items
                        foreach (int[] itm in dl)
                        {
                            Loots.Remove(itm);
                        }
                        dl.Clear();
                        // Exclude items
                        if (chk_exclude.Checked)
                        {
                            foreach (int[] itm in Loots)
                            {
                                if (FindLootItem(BT.lst_loot_items, itm[1].ToString()))
                                    dl.Add(itm);
                            }
                        }

                        /*******************/
                        // Include items
                        if (chk_include.Checked)
                        {
                            foreach (int[] itm in Loots)
                            {
                                if (FindLootItem(BT.lst_loot_items, itm[1].ToString()))
                                {
                                    Thread.Sleep(100);
                                    short val = Convert.ToInt16(itm[0].ToString());
                                    k[CA.cBase].Write<short>(CA.cInf.Targ1, val);
                                    k[CA.cBase].Write<short>(CA.cInf.Targ2, val);
                                    k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 4);
                                    k[CA.cBase].Write<short>(CA.c_pvp, 1300);
                                    //dl.Add(itm);
                                    Thread.Sleep(100);
                                }
                                else if ((!chk_gold.Checked && itm[1] == gold) || (itm[1] != gold))
                                    dl.Add(itm);
                            }
                        }
                        // Golds
                        if (!chk_gold.Checked)
                        {
                            foreach (int[] itm in Loots)
                            {
                                if (itm[1] == gold)
                                    dl.Add(itm);
                            }
                        }
                        // Remove Items
                        foreach (int[] itm in dl)
                        {
                            Loots.Remove(itm);
                        }
                        /*******************/
                        bool gloot = false;
                        if (Loots.Count() > ng)
                            gloot = true;

                        foreach (int[] itm in Loots)
                        {
                            if ((gloot && itm[1] == gold) || (itm[1] != gold))
                            {
                                short val = Convert.ToInt16(itm[0].ToString());
                                k[CA.cBase].Write<short>(CA.cInf.Targ1, val);
                                k[CA.cBase].Write<short>(CA.cInf.Targ2, val);
                                k[CA.cBase].Write<short>(CA.cInf.Targ_Act, 4);
                                k[CA.cBase].Write<short>(CA.c_pvp, 1300);
                                Thread.Sleep(10);
                            }
                        }
                    }
                    else
                        Thread.Sleep(10);
                }
                catch
                {
                    break;
                }
                npick--;
            }
        }
        private void btn_load_items_Click(object sender, EventArgs e)
        {

            fileOpen.Filter = "txt files (*.txt)|*.txt";
            fileOpen.InitialDirectory = KW.base_dir;
            DialogResult result = fileOpen.ShowDialog();
            if (result == DialogResult.OK)
            {
                lst_khan_item.Clear();
                var lines = File.ReadAllLines(fileOpen.FileName);
                tab_khan_items.Clear();
                foreach (var line in lines)
                {
                    string ln = line.ToString();
                    string[] txt = ln.Split('|');
                    string iid = txt[0];
                    tab_khan_items.Rows.Add(iid, txt[1]);
                }
                TableToListView_Loot(tab_khan_items, BT.lst_loot_items, lst_khan_item);
            }

        }

        private static void FindTableToListView(DataTable table, ListView lst, string txt)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row[1].ToString().IndexOf(txt) > -1)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());
                    for (int i = 1; i < table.Columns.Count; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }
                    lst.Items.Add(item);
                }
            }
        }

        private static void TableToListView_Loot(DataTable table, ListView lst, ListView tmp)
        {
            lst.BeginUpdate();
            DataTable tb = table.Copy();
            foreach (DataRow row in table.Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                ListViewItem t_item = new ListViewItem(row[1].ToString());
                for (int i = 1; i < table.Columns.Count; i++)
                {
                    tmp.Items.Add(t_item);
                    item.SubItems.Add(row[i].ToString());
                }
                lst.Items.Add(item);
            }
            lst.EndUpdate();
        }
        private static void FindTableToListView_Loot(DataTable table, ListView lst, ListView lst_tmp, string txt)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row[1].ToString().ToUpper().IndexOf(txt.ToUpper()) > -1)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());
                    ListViewItem item_temp = new ListViewItem(row[1].ToString());
                    for (int i = 1; i < table.Columns.Count; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }
                    lst.Items.Add(item);
                    lst_tmp.Items.Add(item_temp);
                }
            }
        }
        private void txt_l_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string txtfind = txt_l_name.Text;
                lst_khan_item.Items.Clear();
                BT.lst_items.Items.Clear();
                if (tab_khan_items.Rows.Count > 1)
                {
                    FindTableToListView_Loot(tab_khan_items, BT.lst_items, lst_khan_item, txtfind);
                }
            }
        }

        private static int FindInList(ListView ls, int col, string txt)
        {
            int ret = -1;
            foreach (ListViewItem lv in ls.Items)
            {
                if (lv.SubItems[col].Text == txt)
                {
                    ret = lv.Index;
                    break;
                }
            }
            return ret;
        }
        private void lst_khan_item_DoubleClick(object sender, EventArgs e)
        {
            if (lst_khan_item.SelectedItems.Count > 0)
            {
                ListViewItem lvi = BT.lst_items.Items[lst_khan_item.SelectedItems[0].Index];
                if (FindInList(BT.lst_loot_items, 0, lvi.SubItems[0].Text) == -1)
                {
                    ListViewItem l_new = new ListViewItem(lvi.SubItems[0].Text);
                    ListViewItem t_new = new ListViewItem(lvi.SubItems[1].Text);
                    l_new.SubItems.Add(lvi.SubItems[1].Text);
                    BT.lst_loot_items.Items.Add(l_new);
                    lst_loot.Items.Add(t_new);
                }
            }
        }

        private void lst_khan_item_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            //lst_khan_item.Columns[0].Width = -1;
        }

        private void lst_loot_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            //lst_loot.Columns[0].Width = 0;
        }

        private static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        private double GetMyDistance(double x2, double y2)
        {
            CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x);
            CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y);
            return Math.Sqrt(Math.Pow((x2 - CA.cInf.char_x), 2) + Math.Pow((y2 - CA.cInf.char_y), 2));
        }
        public bool InRange(int x, int y, double r)
        {
            CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x);
            CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y);
            double dist = GetDistance(CA.cInf.char_x, CA.cInf.char_y, x, y);
            return (dist <= r);
        }

        private byte[] ReadTargets(int addr, out int Targ, out int TargX, out int TargY, out int TargT, out float tx, out float ty, out int flag)
        {
            byte[] value = k.Memory.Read<byte>((IntPtr)addr, 40);
            byte[] cnv = new byte[2] { 0x0, 0x0 };

            //flag = (int)value[0]; 
            flag = (int)BitConverter.ToInt16(value, 0);
            TargT = (int)BitConverter.ToInt16(value, 10);
            Targ = (int)BitConverter.ToInt16(value, 4);
            TargX = (int)BitConverter.ToInt16(value, 6);
            TargY = (int)BitConverter.ToInt16(value, 8);
            byte[] fcx = new byte[4];
            byte[] fcy = new byte[4];
            Array.Copy(value, 10, fcx, 0, 4);
            Array.Copy(value, 14, fcy, 0, 4);
            tx = BitConverter.ToSingle(fcx, 0);
            ty = BitConverter.ToSingle(fcy, 0);

            // Temporary 
            return value;
        }
        private void bg_fetch_mobs_DoWork(object sender, DoWorkEventArgs e)
        {
            decimal dec_x = 32;
            decimal dec_y = -32;
            while (app_exit == false && KW.start_force)
            {
                Get_Targets();
                Thread.Sleep(1);
            }
        }

        private void Refresh_Items()
        {
            AOBScan newscan = new AOBScan((uint)kPid);
            IntPtr item_add = (IntPtr)newscan.AobScan(new byte[] { 0x42, 0x72, 0x6F, 0x61, 0x64, 0x20, 0x53, 0x77, 0x6F, 0x72, 0x64, 0x00, 0x4C, 0x6F, 0x6E, 0x67, 0x20, 0x53, 0x77, 0x6F, 0x72, 0x64, 0x00, 0x00, 0x4B, 0x61, 0x72, 0x6E, 0x79, 0x75 });
            byte[] bItem = k.Memory.Read<byte>(item_add, 0xE168);
            string itext = System.Text.Encoding.UTF8.GetString(bItem);
            string[] separator = { "\0" };
            var ls = itext.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
            IntPtr sAdd = k[CA.MainBase].Read<IntPtr>(0x0031997C);
            sAdd = IntPtr.Add(sAdd, 0x8);
            tab_khan_items.Clear();
            for (int i = 0; i <= 21906; i++)
            {
                kitem itm = new kitem();
                itm.Addr = sAdd;
                itm.Code = i;
                itm.nCode = k.Memory.Read<short>(sAdd + 0x34);
                itm.Plus = k.Memory.Read<short>(sAdd + 0x36);
                if (itm.nCode > 0)
                    itm.Name = ls[itm.nCode - 1];
                if (itm.Plus > 0)
                    itm.Name = itm.Name + " +" + itm.Plus.ToString();

                sAdd += 112;
                tab_khan_items.Rows.Add(itm.Code.ToString(), itm.Name);
            }
        }
        private void btn_find_loot_Click(object sender, EventArgs e)
        {
            // AOBScan newscan = new AOBScan((uint)kPid);
            //for Win 8 Eikuy - 
            /*
            CA.cInf.loot_address = (IntPtr)newscan.AobScan(new byte[] { 0x94, 0x0C, 0x08, 0x0D, 0x92, 0x15, 0x08, 0x8D });
            if (CA.cInf.loot_address == (IntPtr)0x0)
                CA.cInf.loot_address = (IntPtr)newscan.AobScan(new byte[] { 0x8F, 0x0C, 0x08, 0x8D, 0xBC, 0x12, 0xB8, 0x17 });

            if (CA.cInf.loot_address == (IntPtr)0x0)
                MessageBox.Show("Scan failed.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
                MessageBox.Show("Scan successful.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Question); 
        */
        }

        class spattern
        {
            public readonly IMemoryPattern dPat = new DwordPattern("42 ?? 6F 61 64 20 53 77 6F 72 64 00 4C 6F 6E 67 20 53 77");
        }
        private void btn_item_stat_Click(object sender, EventArgs e)
        {
            frmItemStat npc = new frmItemStat(kPid);
            npc.ShowDialog();
        }

        private void chk_loot_enable_CheckedChanged(object sender, EventArgs e)
        {
            chk_Pick.Checked = false;
        }

        private void btn_rem_loot_Click(object sender, EventArgs e)
        {
            if (BT.lst_loot_items.Items.Count > 0)
            {
                DialogResult resp = MessageBox.Show("Are you sure you want delete loot list items?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resp == DialogResult.Yes)
                {
                    while (BT.lst_loot_items.Items.Count > 0)
                    {
                        BT.lst_loot_items.Items[0].Remove();
                        lst_loot.Items[0].Remove();
                    }
                }
            }
        }

        private void chk_include_CheckedChanged(object sender, EventArgs e)
        {
            chk_exclude.Checked = false;
        }

        private void chk_exclude_CheckedChanged(object sender, EventArgs e)
        {
            chk_include.Checked = false;
        }


        private void npc_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chk_use_scroll.Checked)
                BT.prec = 4;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btn_ldq_clist_Click(object sender, EventArgs e)
        {

            BT.prec = 4;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void btn_ldq_cview_Click(object sender, EventArgs e)
        {

            BT.prec = 5;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void btn_ldq_recon_Click(object sender, EventArgs e)
        {

            BT.prec = 6;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void chk_ldq_recon_CheckedChanged(object sender, EventArgs e)
        {

        }

        public bool CheckLdq(int what)
        {
            Point P;
            Color C;
            switch (what)
            {
                case 4:
                    P = BT.ldq_clist.XY;
                    P.X += KW.KL.Left;
                    P.Y += KW.KL.Top;
                    C = BT.ldq_clist.RGB;
                    break;
                case 5:
                    P = BT.ldq_cview.XY;
                    P.X += KW.KL.Left;
                    P.Y += KW.KL.Top;
                    C = BT.ldq_cview.RGB;
                    break;
                case 6:
                default:
                    P = BT.ldq_recon.XY;
                    P.X += KW.KL.Left;
                    P.Y += KW.KL.Top;
                    C = BT.ldq_recon.RGB;
                    break;
            }
            Color CL = new Color();
            CL = GetColorAt(P);
            if (CL == C)
                return true;
            return false;
        }
        private void ldq_bug_DoWork(object sender, DoWorkEventArgs e)
        {
            bool enab = false;
            btn_bug_on.Enabled = enab;
            while (BT.ldq)
            {
                if (!bg_ldq_lclick.IsBusy)
                    bg_ldq_lclick.RunWorkerAsync();
                if (!CheckLdq(4) && !CheckLdq(5))
                {
                    TF_Key.ESC();
                    Thread.Sleep(BT.ldq_esc_delay);
                }
                else
                {
                    if (!bg_ldq_lclick.IsBusy)
                        bg_ldq_lclick.RunWorkerAsync();
                }
                if (CheckLdq(5))
                {
                    if (!CheckLdq(7))
                    {
                        TF_Key.ESC();
                        Thread.Sleep(10);
                    }
                }
                Thread.Sleep(1);
            }
            enab = false;
            btn_bug_on.Enabled = true;
        }

        private void btn_ldq_recon1_Click(object sender, EventArgs e)
        {
            BT.prec = 7;
            if (!bg_get_coord.IsBusy)
                bg_get_coord.RunWorkerAsync();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void btn_bug_on_Click(object sender, EventArgs e)
        {

        }

        private void bg_ldq_lclick_DoWork(object sender, DoWorkEventArgs e)
        {
            while (BT.ldq && !CheckLdq(4))
            {
                toolz.LeftClick(BT.ldq_recon.XY.X, BT.ldq_recon.XY.Y, 20, 100);
                Thread.Sleep(1);
            }
        }

        private void TheForce_Load_1(object sender, EventArgs e)
        {

        }

        private pListChar GetSCIndex()
        {
            int idx = cb_support.SelectedIndex;
            int i = 0;
            pListChar ret = new pListChar();
            foreach (var cb in pList)
            {
                if (i == idx)
                    return cb;
                i++;
            }
            return ret;
        }
        private void cb_support_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!k.Handle.IsClosed)
            {
                pListChar pl = GetSCIndex();
                pSupport cpr = new pSupport();
                cpr.GetDetail(pl.pid);
                if (cpr.char_detail.char_class == "Cleric" || cpr.char_detail.char_class == "Miko")
                {
                    sup.GetDetail(pl.pid);
                    chk_sup_enable.Enabled = true;
                    chk_sup_follow.Enabled = true;
                    chk_weap_dura.Enabled = true;
                }
            }
        }

        private void cb_support_DropDown(object sender, EventArgs e)
        {
            pList = new List<pListChar>();
            System.Diagnostics.Process[] procs;
            procs = System.Diagnostics.Process.GetProcessesByName("KhanClient");
            cb_support.Items.Clear();
            foreach (var pr in procs)
            {
                if (pr.Id != 0)
                {
                    pSupport lpr = new pSupport();
                    lpr.GetDetail(pr.Id);
                    if ((lpr.char_detail.char_class == "Cleric" || lpr.char_detail.char_class == "Miko") && k.Native.Id != lpr.Pid)
                    {
                        pListChar li = new pListChar();
                        li.pid = lpr.Pid;
                        li.char_name = lpr.char_detail.char_name;
                        pList.Add(li);
                        cb_support.Items.Add(lpr.char_detail.char_name);
                    }
                }
            }
        }

        private void chk_sup_enable_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_sup_enable.Checked)
            {
                Thread.Sleep(100);
                // ClearList 
                var blist = from m in tab_support.AsEnumerable()
                            where m.Field<bool>("Use") == true
                            && m.Field<string>("Type") == "Buff"
                            select m;
                sup.Buff = new cskill[blist.Count()];
                int counter = 0;
                foreach (var bl in blist)
                {
                    sup.Buff[counter].code = bl.Field<Int16>("Code");
                    sup.Buff[counter].range = bl.Field<Int16>("Range");
                    sup.Buff[counter].delay = bl.Field<Int16>("Delay");
                    counter++;
                }
                var hlist = from m in tab_support.AsEnumerable()
                            where m.Field<bool>("Use") == true
                            && m.Field<string>("Type") == "Heal"
                            select m; 
                sup.Heal = new cskill[hlist.Count()];
                counter = 0;
                foreach (var bl in hlist)
                {
                    sup.Heal[counter].code = bl.Field<Int16>("Code");
                    sup.Heal[counter].range = bl.Field<Int16>("Range");
                    sup.Heal[counter].delay = bl.Field<Int16>("Delay");
                    counter++;
                } 
                if (!bg_support.IsBusy)
                    bg_support.RunWorkerAsync();
            }
        }

        public void sWalk(int x, int y)
        {
            float sx = (float)(x * dec_x) + 16;
            float sy = (float)(y * dec_y) + (-16);
            sup.pr[sup.cBase].Write<short>(sup.Addr.Targ_Act, (short)0);
            sup.pr[sup.cBase].Write<short>(sup.Addr.Next_Flag, 0);
            sup.pr[sup.cBase].Write<short>(sup.Addr.Next_Action, 0);
            sup.pr[sup.cBase].Write<float>(sup.Addr.move_x, sx);
            sup.pr[sup.cBase].Write<float>(sup.Addr.move_y, sy);
            sup.pr[sup.cBase].Write<short>(sup.Addr.Targ_Act, (short)2);
            sup.pr[sup.cBase].Write<short>(sup.Addr.c_pvp, 1100);
        }
        public bool Support(int char_id, int char_x, int char_y, cskill sk)
        {
            bool t = InRange(char_x, char_y, sk.range);
            if (t)
            {
                try
                {
                    int cur_skill = 0X74A;
                    int new_skill = 0x74C;
                    sup.pr[sup.cBase].Write<int>(sup.Addr.c_E8, 1000);
                    Thread.Sleep(sk.delay);
                    sup.pr[sup.cBase].Write<short>(sup.Addr.Targ_Act, (short)1);
                    sup.pr[sup.cBase].Write<int>(sup.Addr.c_skill, sk.code);
                    sup.pr[sup.cBase].Write<int>(cur_skill, sk.code);
                    sup.pr[sup.cBase].Write<int>(new_skill, sk.code);
                    sup.pr[sup.cBase].Write<short>(sup.Addr.Targ2, (short)char_id);
                    sup.pr[sup.cBase].Write<short>(sup.Addr.c_pvp, 1800);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                sWalk(char_x, char_y);
                Thread.Sleep(200);
            }
            return false;
        }
        private bool IsBuff()
        { 
            List<cskill> mBuff = CheckBuffs(sup.Buff);
            foreach (cskill bf in mBuff)
            {
                double dchar = GetDistance(CA.cInf.char_x, CA.cInf.char_y, sup.char_x, sup.char_y);
                if (dchar <= bf.range)
                { 
                        return true; 
                }
            }
            return false;
        }
        private void bg_support_DoWork(object sender, DoWorkEventArgs e)
        {
            int hs_idx = 0;
            while (chk_sup_enable.Checked)
            {
                int frange;
                sup.char_x = sup.pr[sup.char_base].Read<short>(sup.Addr.pChar_x);
                sup.char_y = sup.pr[sup.char_base].Read<short>(sup.Addr.pChar_y);
                //txt_ld_path.Text = GetDistance(CA.cInf.char_x, CA.cInf.char_y, sup.char_x, sup.char_y).ToString(); //("F10");
                if (chk_sup_follow.Checked)
                {
                    int rg = 3;
                    toolz.GetInt(txt_follow_range, ref rg);
                    if (GetMyDistance(sup.char_x, sup.char_y) > rg)
                    { 
                        sWalk(CA.cInf.char_x, CA.cInf.char_y);
                        Thread.Sleep(10);
                    }
                    else
                    {
                        if (sup.pr[sup.cBase].Read<int>(sup.Addr.c_E8) == 1100)
                        {
                            sup.pr[sup.cBase].Write<int>(sup.Addr.c_E8, 1000);
                        }
                    }
                }
                // Weap Durability
                int wdura = Convert.ToInt32(k[CA.cBase].Read<byte>(CA.cInf.Weap));
                int wper = 28;
                toolz.GetInt(txt_weap_dura, ref wper);
                // Weap Dura End
                // Buff 
                if (sIsOut())
                {
                    if (IsBuff() && KW.start_force)
                    {
                        List<cskill> buffs = CheckBuffs(sup.Buff);
                        foreach (cskill bf in buffs)
                        {
                            double dchar = GetDistance(CA.cInf.char_x, CA.cInf.char_y, sup.char_x, sup.char_y);
                            if (dchar <= bf.range)
                            { 
                               Support((short)CA.cInf.MyId, CA.cInf.char_x, CA.cInf.char_y, bf); 
                            }
                        }
                    }
                    if (chk_weap_dura.Checked)
                    {
                        if (wdura <= wper)
                        {
                            if (!bg_beep.IsBusy)
                                bg_beep.RunWorkerAsync();
                        }
                    }
                }
                // Supporter HP
                int curHP = sup.pr[sup.cBase].Read<int>(sup.Addr.CHP);
                int maxHP = sup.pr[sup.cBase].Read<int>(sup.Addr.OHP);
                int sup_healHP = Convert.ToInt32(Math.Floor(Convert.ToDecimal(curHP) * 100) / Convert.ToDecimal(maxHP));

                if (toolz.IsSameClient(sup.Pid))
                {
                    int maxMP = sup.pr[sup.cBase].Read<int>(sup.Addr.OMP);
                    int curMP = sup.pr[sup.cBase].Read<int>(sup.Addr.CMP);
                    int healMP = Convert.ToInt32(Math.Floor(Convert.ToDecimal(curMP) * 100) / Convert.ToDecimal(maxMP));
                    if (healMP < 60)
                    {
                        TF_Key.PressKey('w', 50); // MP
                        Thread.Sleep(200);
                    }
                }
                int pHP = 0;
                toolz.GetInt(txt_sup_per, ref pHP);
                int healHP = Convert.ToInt32(Math.Floor(Convert.ToDecimal(CA.cInf.curHP) * 100) / Convert.ToDecimal(CA.cInf.maxHP));
                if (!IsBuff())
                {
                    if (pHP > healHP && sIsOut() && healHP > 0)
                    {
                        // Heal Main
                        cskill bf = new cskill();
                        if (sup.Heal.Count() > 0)
                        {
                            bf = sup.Heal[hs_idx];
                            double dchar = GetDistance(CA.cInf.char_x, CA.cInf.char_y, sup.char_x, sup.char_y);
                            if (dchar <= bf.range)
                                Support((short)CA.cInf.MyId, CA.cInf.char_x, CA.cInf.char_y, bf);
                        }
                        if (sup.Heal.Count() < hs_idx)
                            hs_idx++;
                        else
                            hs_idx = 0;
                    }

                    if (pHP > sup_healHP && sIsOut() && sup_healHP > 0)
                    {
                        int MyId = sup.pr[sup.cBase].Read<int>(sup.Addr.MyOff);
                        cskill bf = new cskill();
                        if (sup.Heal.Count() > 0)
                        {
                            bf = sup.Heal[hs_idx];
                            double dchar = GetDistance(CA.cInf.char_x, CA.cInf.char_y, sup.char_x, sup.char_y);
                            if (bf.range <= dchar)
                                Support((short)MyId, CA.cInf.char_x, CA.cInf.char_y, bf);
                        }
                        if (sup.Heal.Count() < hs_idx)
                            hs_idx++;
                        else
                            hs_idx = 0;
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void txt_grange_TextChanged(object sender, EventArgs e)
        {

        }

        private void bg_beep_DoWork(object sender, DoWorkEventArgs e)
        {
            toolz.Beep(2000, 400);
        }

        static string ByteToStr(byte[] bytes)
        {
            return string.Join(string.Empty, Array.ConvertAll(bytes, b => b.ToString("X2")));
        }

        private MobDetail ReadMob(IntPtr adr)
        {
            if (adr != (IntPtr)0x0)
            {
                CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x);
                CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y);
                MobDetail ret = new MobDetail();
                ret.Address = adr;
                ret.Flag = k.Memory.Read<byte>(ret.Address);
                ret.Id = k.Memory.Read<short>(ret.Address + 4);
                ret.X = k.Memory.Read<short>(ret.Address + 0x90);
                ret.Y = k.Memory.Read<short>(ret.Address + 0x92);
                ret.FX = k.Memory.Read<float>(ret.Address + 0xC0);
                ret.FY = k.Memory.Read<float>(ret.Address + 0xc4);
                ret.Range = GetDistance(CA.cInf.char_x, CA.cInf.char_y, ret.X, ret.Y);
                return ret;
            }
            else
                return new MobDetail();
        }

        private void txt_boost_TextChanged(object sender, EventArgs e)
        {
            int sti = 0;
            toolz.GetInt(txt_boost, ref sti);
            string st = "." + sti.ToString();

        }

        public void logxls()
        {
            if (CA.log)
            {
                List<MobDetail> exp = new List<MobDetail>(CA.MonsScan);
                dta_tempo.Clear();
                foreach (MobDetail fn in exp)
                    dta_tempo.Rows.Add(fn.Id.ToString(), fn.X.ToString(), fn.Y.ToString(), fn.Flag.ToString(), fn.Address.ToString(), fn.Range, fn.debug);
                IConverts.ExportXLS(dta_tempo, "moblist");
                exp = new List<MobDetail>(CA.MonsScan);
                dta_tempo.Clear();
                foreach (MobDetail fn in exp)
                    dta_tempo.Rows.Add(fn.Id.ToString(), fn.X.ToString(), fn.Y.ToString(), fn.Flag.ToString(), fn.Address.ToString(), fn.Range, fn.debug);

                IConverts.ExportXLS(dta_tempo, "moblist_stuck");
                dta_tempo.Clear();
                CA.log = false;
            }
        }
        private void btn_rem_coord_Click_1(object sender, EventArgs e)
        {
            CA.log = true;
        }

        private void RefreshMobs_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!k.Native.HasExited && KW.start_force)
            {
                List<MobDetail> rem = new List<MobDetail>();
                decimal dec_x = 32;
                decimal dec_y = -32;
                CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x);
                CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y);
                DateTime nul = new DateTime();
                foreach (MobDetail mb in CA.MonsScan)
                {
                    CA.cInf.char_x = k[CA.char_base].Read<short>(CA.cInf.pChar_x);
                    CA.cInf.char_y = k[CA.char_base].Read<short>(CA.cInf.pChar_y);
                    int pflag = mb.Flag;
                    mb.Flag = k.Memory.Read<byte>(mb.Address);
                    if (pflag != 128 && mb.Flag == 128)
                        mb.Hits = 0;
                    mb.Id = k.Memory.Read<short>(mb.Address + 4);
                    mb.X = k.Memory.Read<short>(mb.Address + 0x90);
                    mb.Y = k.Memory.Read<short>(mb.Address + 0x92);
                    mb.FX = k.Memory.Read<float>(mb.Address + 0xC0);
                    mb.FY = k.Memory.Read<float>(mb.Address + 0xc4);
                    mb.Range = GetDistance(CA.cInf.char_x, CA.cInf.char_y, mb.X, mb.Y);

                    if (mb.IsStuck == true)
                    {
                        if (DateTime.Now.Subtract(mb.Stuck).TotalSeconds >= 4)
                        {
                            mb.IsStuck = false;
                            mb.Stuck = DateTime.Now;
                            mb.Hits = 0;
                        }
                    }
                    else
                        mb.Stuck = DateTime.Now;

                }
                logxls();


                Thread.Sleep(1);
            }
        }

        private void Get_Targets()
        {
            while (CA.tgs.Count == 0)
            {
                try
                {
                    var lst = CA.MonsScan
                            .OrderBy(MonsScan => MonsScan.Range)
                            .Where((MonsScan, Range) => MonsScan.Range <= CA.hit_range && MonsScan.Id > 0 && MonsScan.IsStuck == false && MonsScan.Flag == 128)
                            .Take(KW.num_mobs);

                    
                    foreach (MobDetail m in lst)
                    {
                        int idx = CA.MonsScan.IndexOf(m);  
                            CA.tgs.Add(m); 
                    }
                }
                catch { }
                Thread.Sleep(1);
            }
        }
        private void ts_bot_Click(object sender, EventArgs e)
        {
            CA.Route.Play = !CA.Route.Play;
            if (CA.Route.Play == false)
            {
                lb_bot_state.Text = "Paused..";
                DoStop(50);
            }
            else
                lb_bot_state.Text = "Botting..";
        }

        private void cmb_rng_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRange.Text = (cmb_rng.SelectedIndex + 1).ToString(); 
        }
    }

}