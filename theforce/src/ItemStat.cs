using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheForce;

namespace TheForce
{
    public partial class frmItemStat : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public class AOBScan
        {
            protected uint ProcessID;
            public AOBScan(uint ProcessID)
            {
                this.ProcessID = ProcessID;
            }

            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            protected static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);
            [System.Runtime.InteropServices.DllImport("kernel32.dll")]
            protected static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

            [StructLayout(LayoutKind.Sequential)]
            protected struct MEMORY_BASIC_INFORMATION
            {
                public IntPtr BaseAddress;
                public IntPtr AllocationBase;
                public uint AllocationProtect;
                public uint RegionSize;
                public uint State;
                public uint Protect;
                public uint Type;
            }

            protected List<MEMORY_BASIC_INFORMATION> MemoryRegion { get; set; }
            public int MaxScan = 0;
            public int Pos = 0;
            public int ResultCount { get; set; }
            protected void MemInfo(IntPtr pHandle)
            {
                IntPtr Addy = new IntPtr();
                while (true)
                {
                    MEMORY_BASIC_INFORMATION MemInfo = new MEMORY_BASIC_INFORMATION();
                    int MemDump = VirtualQueryEx(pHandle, Addy, out MemInfo, Marshal.SizeOf(MemInfo));
                    if (MemDump == 0) break;
                    if ((MemInfo.State & 0x1000) != 0 && (MemInfo.Protect & 0x100) == 0)
                        MemoryRegion.Add(MemInfo);
                    Addy = new IntPtr(MemInfo.BaseAddress.ToInt32() + (int)MemInfo.RegionSize);
                }
            }
            protected IntPtr Scan(byte[] sIn, byte[] sFor)
            {
                int[] sBytes = new int[256]; int Pool = 0;
                int End = sFor.Length - 1;
                for (int i = 0; i < 256; i++)
                    sBytes[i] = sFor.Length;
                for (int i = 0; i < End; i++)
                    sBytes[sFor[i]] = End - i;
                while (Pool <= sIn.Length - sFor.Length)
                {
                    for (int i = End; sIn[Pool + i] == sFor[i]; i--)
                        if (i == 0) return new IntPtr(Pool);
                    Pool += sBytes[sIn[Pool + End]];
                }
                return IntPtr.Zero;
            }
            public IntPtr AobScan(byte[] Pattern)
            {
                System.Diagnostics.Process Game = System.Diagnostics.Process.GetProcessById((int)this.ProcessID);
                if (Game.Id == 0) return IntPtr.Zero;
                MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
                MemInfo(Game.Handle);
                for (int i = 0; i < MemoryRegion.Count; i++)
                {
                    byte[] buff = new byte[MemoryRegion[i].RegionSize];
                    ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);

                    IntPtr Result = Scan(buff, Pattern);
                    if (Result != IntPtr.Zero)
                        return new IntPtr(MemoryRegion[i].BaseAddress.ToInt32() + Result.ToInt32());
                }
                return IntPtr.Zero;
            }

            protected List<int> ScanList(byte[] sIn, byte[] sFor)
            {
                List<int> ret = new List<int>();
                int[] sBytes = new int[256]; int Pool = 0;
                int End = sFor.Length - 1;
                for (int i = 0; i < 256; i++)
                    sBytes[i] = sFor.Length;
                for (int i = 0; i < End; i++)
                    sBytes[sFor[i]] = End - i;
                while (Pool <= sIn.Length - sFor.Length)
                {
                    for (int i = End; sIn[Pool + i] == sFor[i]; i--)
                        if (i == 0)
                        {
                            ret.Add(Pool);
                            break;
                        }
                    Pool += sBytes[sIn[Pool + End]];
                }

                return ret;
            }
            public List<uint> AobScanAddress(byte[] Pattern)
            {
                List<uint> ret = new List<uint>();
                System.Diagnostics.Process Game = System.Diagnostics.Process.GetProcessById((int)this.ProcessID);
                ResultCount = 0;
                if (Game.Id == 0) return ret;
                MemoryRegion = new List<MEMORY_BASIC_INFORMATION>();
                MemInfo(Game.Handle);
                MaxScan = MemoryRegion.Count;
                Pos = 0;
                for (int i = 0; i < MemoryRegion.Count; i++)
                {
                    byte[] buff = new byte[MemoryRegion[i].RegionSize];
                    ReadProcessMemory(Game.Handle, MemoryRegion[i].BaseAddress, buff, MemoryRegion[i].RegionSize, 0);
                    Pos++;
                    List<int> Result = ScanList(buff, Pattern);
                    if (Result.Count > 0)
                        foreach (var res in Result)
                            ret.Add(Convert.ToUInt32(MemoryRegion[i].BaseAddress.ToInt32() + res));
                }
                ResultCount = ret.Count;

                return ret;
            }
        }
        private int pId = 0;
        public AOBScan scan;
        List<uint> item_adds;
        private ProcessMemoryReader khan = new ProcessMemoryReader();
        public frmItemStat(int pi)
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;
            pId = pi;
            scan = new AOBScan((uint)pId);
            InitializeComponent();
        }

        private void frmItemStat_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == (Char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {

            if (!bg_scan_item.IsBusy)
                bg_scan_item.RunWorkerAsync();
        }
        private void bg_scan_item_DoWork(object sender, DoWorkEventArgs e)
        {
            int melee_min = 0;
            int melee_max = 0;
            int magic_min = 0;
            int magic_max = 0;
            toolz.GetInt(txt_n_min, ref melee_min);
            toolz.GetInt(txt_n_max, ref melee_max);
            toolz.GetInt(txt_s_min, ref magic_min);
            toolz.GetInt(txt_s_max, ref magic_max);
            byte[] n_min = new byte[2];
            byte[] n_max = new byte[2];
            byte[] s_min = new byte[2];
            byte[] s_max = new byte[2];
            n_min = BitConverter.GetBytes((short)melee_min);
            n_max = BitConverter.GetBytes((short)melee_max);
            s_min = BitConverter.GetBytes((short)magic_min);
            s_max = BitConverter.GetBytes((short)magic_max);
            int pat_len = n_min.Length + n_max.Length + s_min.Length + s_max.Length;
            byte[] pat = new byte[pat_len];
           // for (int x = 0; x < pat_len; x++)
            //{
                int x = 0;
                for (int i = 0; i < n_min.Length; i++)
                {
                    pat[x] = n_min[i];
                    x++;
                }
                for (int i = 0; i < n_max.Length; i++)
                {
                    pat[x] = n_max[i];
                    x++;
                }
                for (int i = 0; i < s_min.Length; i++)
                {
                    pat[x] = s_min[i];
                    x++;
                }
                for (int i = 0; i < s_max.Length; i++)
                {
                    pat[x] = s_max[i];
                    x++;
                }
            // }
            scan.Pos = 0;
            lb_fnd.Text = "Found : 0";
            if (!bg_status.IsBusy)
                bg_status.RunWorkerAsync();
            item_adds = scan.AobScanAddress(pat);
        }

        private void bg_status_DoWork(object sender, DoWorkEventArgs e)
        {
            scan_bar.Maximum = scan.MaxScan;
            scan_bar.Value = 0;
            while (scan.Pos < scan.MaxScan)
            {
                scan_bar.Value = scan.Pos; 
                Thread.Sleep(5);
            }
            lb_fnd.Text = "Found : " + scan.ResultCount.ToString();
            scan_bar.Value = scan.MaxScan;
            if (scan.ResultCount == 0)
            {
                btn_set.Enabled = false;
                MessageBox.Show("Scan failed.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                btn_set.Enabled = true;
                MessageBox.Show("Scan successful.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            
        }

        private void btn_set_Click(object sender, EventArgs e)
        {
            int melee_min = 0;
            int melee_max = 0;
            int magic_min = 0;
            int magic_max = 0;
            int nx = 0;
            toolz.GetInt(txt_n_min, ref melee_min);
            toolz.GetInt(txt_n_max, ref melee_max);
            toolz.GetInt(txt_s_min, ref magic_min);
            toolz.GetInt(txt_s_max, ref magic_max);
            toolz.GetInt(txt_i_times, ref nx);
            melee_min *= nx;
            melee_max *= nx;
            magic_min *= nx;
            magic_max *= nx;
            byte[] n_min = new byte[2];
            byte[] n_max = new byte[2];
            byte[] s_min = new byte[2];
            byte[] s_max = new byte[2];
            n_min = BitConverter.GetBytes((short)melee_min);
            n_max = BitConverter.GetBytes((short)melee_max);
            s_min = BitConverter.GetBytes((short)magic_min);
            s_max = BitConverter.GetBytes((short)magic_max);
            int pat_len = n_min.Length + n_max.Length + s_min.Length + s_max.Length;
            byte[] pat = new byte[pat_len];
            
            int x = 0;
            for (int i = 0; i < n_min.Length; i++)
            {
                pat[x] = n_min[i];
                x++;
            }
            for (int i = 0; i < n_max.Length; i++)
            {
                pat[x] = n_max[i];
                x++;
            }
            for (int i = 0; i < s_min.Length; i++)
            {
                pat[x] = s_min[i];
                x++;
            }
            for (int i = 0; i < s_max.Length; i++)
            {
                pat[x] = s_max[i];
                x++;
            }
            foreach (uint ad in item_adds)
            {
                int br = 0; 
                khan.WriteMemory((IntPtr)ad, pat, out br);
            }
            this.Close();
        }

        private void frmItemStat_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(pId);
            if (process != null)
            {
                khan.ReadProcess = process;
                khan.OpenProcess();
            }
            if (!bg_hk.IsBusy)
                bg_hk.RunWorkerAsync();
        }

        private void bg_hk_DoWork(object sender, DoWorkEventArgs e)
        {
            bool start = true;
            while (start)
            {
                if (Keyboard.IsKeyDown(Keys.Escape))
                    this.Close();
                Thread.Sleep(100);
            }
        }

        private void frmItemStat_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen red = new Pen(Color.Red);
            Pen black = new Pen(Color.Black);
            Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            Rectangle rect_shad = new Rectangle(2, 2, this.Width - 4, this.Height - 4);
            g.DrawRectangle(red, rect);
            g.DrawRectangle(black, rect_shad);
        }
    }
}
