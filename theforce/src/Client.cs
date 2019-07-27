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

namespace TheForce
{
    public partial class Client : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public string cn;
        public bool cont;
        private bool me;
        //private string tn;
        public Client(ref string tname, bool m)
        {
            //cName = tname;
            //cn = tname as string;
            //tn = tname as string;
            me = m;
            InitializeComponent();
            txt_name.Text = tname as string;
            txt_name.TextAlign = HorizontalAlignment.Center;
            label1.Visible = me;
            txt_name.Visible = me;
            btn_Cont.Visible = me;
            if (!me)
            {
                cn = txt_name.Text;
                cont = true;
                bg_ClientMonitor.RunWorkerAsync();
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Cont_Click(object sender, EventArgs e)
        {
            cn = txt_name.Text;
            if (btn_Cont.Text == "Continue")
            {
                cont = true;
                txt_name.Enabled = false;
                btn_Cont.Text = "Cancel";

                if (!bg_ClientMonitor.IsBusy)
                    bg_ClientMonitor.RunWorkerAsync();
            }
            else
            {
                cont = false;
                txt_name.Enabled = true;
                btn_Cont.Text = "Continue";
            }

            //this.Hide();
        }

        private void bg_ClientMonitor_DoWork(object sender, DoWorkEventArgs e)
        {
            int pid = 0;
            bool exit = false;
            //cont = false;
            while (!exit)
            {
                while (cont)
                {
                    if (toolz.GetProcName(cn , ref pid))
                    {
                        exit = true;
                        cont = false;
                    }
                    Thread.Sleep(100);
                }

                //if (toolz.GetProcName(CA.game_name, ref pId))
                //{
                //}
                Thread.Sleep(100);
            }
            this.Hide();
        }

        private void Client_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
