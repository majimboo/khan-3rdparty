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

namespace TheForce
{
    public partial class temp : Form
    {
        private DataTable prf;
        private int pId = 0;
        private bool detect = true;
        private DateTimePicker oDateTimePicker;
        //private ProcessMemoryReader khan = new ProcessMemoryReader();
        public ProcessSharp k;

        public temp(ref DataTable dt, int pi)
        {
            //dts_adm = dts as DataSet;
            //dgv_account.DataMember = "Account_Profile";//dts_adm.Tables["Account_Profile"].TableName;
            prf = dt as DataTable;
            pId = pi;
            InitializeComponent();

        }

        private void temp_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(pId);
            if (process != null)
            {
                k = new ProcessSharp(pId, Process.NET.Memory.MemoryType.Local);
                k.Memory = new ExternalProcessMemory(k.Handle);
                /*
                khan.ReadProcess = process;
                khan.OpenProcess();
                */
            }
            
            foreach (DataRow dr in prf.Rows)
                tab_acc_prof.Rows.Add(dr.Field<Int32?>("acc_id"),
                             dr["user_code"].ToString(),
                             dr.Field<Boolean?>("OpenBooster"),
                             dr.Field<Boolean?>("Extreme"),
                             dr.Field<Boolean?>("Multi_Target"),
                             dr.Field<Boolean?>("NPC"),
                             dr.Field<Boolean?>("Items"),
                             dr.Field<DateTime?>("Expiration"),
                             dr.Field<Boolean?>("Bot"),
                             dr.Field<Int32?>("MaxBoost"),
                             dr.Field<Boolean?>("WeapMod"),
                             dr.Field<Boolean?>("ArmorHack"));
        
            bg_get_user.RunWorkerAsync();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            prf.Clear();
            foreach (DataRow dr in tab_acc_prof.Rows)
                prf.Rows.Add(dr.Field<Int32?>("acc_id"),
                             dr["user_code"].ToString(),
                             dr.Field<Boolean?>("OpenBooster"),
                             dr.Field<Boolean?>("Extreme"),
                             dr.Field<Boolean?>("Multi_Target"),
                             dr.Field<Boolean?>("NPC"),
                             dr.Field<Boolean?>("Items"),
                             dr.Field<DateTime?>("Expiration"),
                             dr.Field<Boolean?>("Bot"),
                             dr.Field<Int32?>("MaxBoost"),
                             dr.Field<Boolean?>("WeapMod"),
                             dr.Field<Boolean?>("ArmorHack"));
                        
               /* prf.Rows.Add(dr["acc_id"].ToString(),
                             dr["user_code"].ToString(),
                             dr["OpenBooster"].ToString(),
                             dr["Extreme"].ToString(),
                             dr["Multi_Targ"].ToString(),
                             dr["NPC"].ToString(),
                             dr["Items"].ToString(),
                             dr.Field<DateTime?>("Expiration"),
                             //dr["Expiration"].ToString(),
                             dr["Bot"].ToString());
                             */
        }

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
        private void bg_get_user_DoWork(object sender, DoWorkEventArgs e)
        {
            string usr = "";
            IntPtr Acc_Id = (IntPtr)0x00713164;
            IntPtr u_id1 = (IntPtr)0x0071107C;
            IntPtr u_id2 = (IntPtr)0x0184B30C;
            while (detect)
            {
                int aid = k.Memory.Read<int>(Acc_Id) * 11; //BitConverter.ToInt32(khan.ReadByte(Acc_Id, (uint)4), 0) * 4;
                //byte[] usr_b = khan.ReadByte(u_id1, (uint)14);
                /*
                usr = System.Text.Encoding.Default.GetString(usr_b);
                if (usr.Length < 3)
                    usr = BitConverter.ToString(khan.ReadByte(u_id2, (uint)14), 0);
                */
                usr = Readstr(u_id1, 14); //k.Memory.Read<string>(CA.cInf.u_id1);//System.Text.Encoding.Default.GetString(usr_b);
                if (usr.Length < 3)
                    usr = Readstr(u_id2, 14); //k.Memory.Read<string>(CA.cInf.u_id2);

                if (usr != "")
                {
                    string crp_usr = Cryptography.AESEncryptString(usr, "DmXtreme", "@waKening");
                    txt_id.Text = aid.ToString();
                    txt_code.Text = crp_usr;
                }
                Thread.Sleep(100);
            }
        }
        
        private void btn_add_Click(object sender, EventArgs e)
        {
            tab_acc_prof.Rows.Add(txt_id.Text, txt_code.Text);
        }
    }
}
