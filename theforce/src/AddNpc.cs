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
    public partial class frm_AddNpc : Form
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
        private DataTable dt = null;
        private DataTable s_dt = null;
        private ComboBox cb = null;
        private IntPtr[] b_addr = new IntPtr[2] { (IntPtr)0, (IntPtr)0 };
        private int pId = 0;
        int[] n_scanned;
        int scan_range = 0;
        int start_add = 0;
        int n_scanner;
        int range_per_thread = 0;
        AOBScan newscan;
        //byte[] res;
        private ProcessMemoryReader khan = new ProcessMemoryReader();
        public frm_AddNpc(ref DataTable tbl, ref ComboBox c, ref DataTable ts, IntPtr[] sb, int pi)
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;
            dt = tbl as DataTable;
            s_dt = ts as DataTable;
            cb = c as ComboBox;
            b_addr = sb;
            pId = pi;
            n_scanner = 1;            
            start_add = (int)b_addr[0];
            scan_range = 0xFFFFFFF;//(int)b_addr[1];
            range_per_thread = scan_range;//Convert.ToInt32(Convert.ToDecimal(scan_range) / Convert.ToDecimal(n_scanner));
            newscan = new AOBScan((uint)pId);
            InitializeComponent();
        }

        private void frm_AddNpc_Load(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(pId);
            if (process != null)
            {
                khan.ReadProcess = process;
                khan.OpenProcess();
                if (cb.Text != "<New>")
                {
                    txt_npc_name.Text = dt.Rows[cb.SelectedIndex]["Npc"].ToString();
                    btn_NPC_Search.Enabled = true;
                }
                if (s_dt.Rows.Count > 0)
                    scan_bar.Value = 100;
                else
                    scan_bar.Maximum = range_per_thread * n_scanner;

                _bg_hk.RunWorkerAsync();
            }
            else
                this.Hide();
        }

        public uint FindNPC()
        {
            int no_rec = s_dt.Rows.Count;
            int sel = 0;
            uint found = 0x0;
            while (no_rec > sel)
            {
                uint addr = (uint)s_dt.Rows[sel]["address"];
                byte[] ret = khan.ReadByte(addr, (uint)2);
                byte[] cnv = new byte[2] { ret[0], ret[1] };
                int val = (int)BitConverter.ToInt16(cnv, 0);
                if ( val == 9224)
                {
                    found = addr;
                    break;
                }
                sel++;
            }
            if (found > 0)
                MessageBox.Show("NPC found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No NPC found in search.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return found;
        }
        private void btn_NPC_save_Click(object sender, EventArgs e)
        {
            string st = txt_npc_name.Text;
            if (st != "")
            {
                uint ok = FindNPC();
                if (ok > 0)
                {
                    if (cb.Text == "<New>")
                    {
                        dt.Rows.Add(st, ok);
                        cb.Text = st;
                    }
                    else
                    {
                        dt.Rows[cb.SelectedIndex].SetField("Npc", st);
                        dt.Rows[cb.SelectedIndex].SetField("Addr", ok);
                    }
                    this.Hide();
                }
            }
            else
                MessageBox.Show("Please enter the name of the NPC.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btn_NPC_Search_Click(object sender, EventArgs e)
        {

            n_scanned = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            btn_NPC_Search.Enabled = false;
            //if (!scan_status.IsBusy)
            //    scan_status.RunWorkerAsync();

            if (!_scanner1.IsBusy && n_scanner >= 1)
            {
                _scanner1.RunWorkerAsync();
                if (!scan_status.IsBusy)
                    scan_status.RunWorkerAsync();
            }
             
        }
        private void _scanner1_DoWork(object sender, DoWorkEventArgs e)
        {
            scan_bar.Value = 0;
            List<uint> npc_adds = newscan.AobScanAddress(new byte[] { 0x09, 0x24});
            foreach (var vl in npc_adds)
            {
                s_dt.Rows.Add(vl, 9225);
            }
        }
        private void _scanner1_DoWork_working(object sender, DoWorkEventArgs e)
        {
            const int sRange = 999999;
            const int nScan = 0xFFFFFFF / sRange;
            UInt64 start = 0x02000000;
            byte[] hex_out = new byte[sRange + 1];
            UInt64[] found = new UInt64[5000];
            int f_index = 0;
            //short
            int br = 0;
            scan_bar.Maximum = nScan;
            for (int i = 0; i < 5000; i++)
                found[i] = 0x0;

            for (int i = 0; i < nScan; i++)
            {
                hex_out = khan.ReadMemory((IntPtr)start, sRange + 1, out br);
                for (int s = 0; s < sRange-3; s++)
                {
                    //byte[] raw = new byte[4] { hex_out[s], hex_out[s + 1], hex_out[s + 2], hex_out[s + 3] };
                    //byte[] cmp = new byte[4] { 0x09, 0x24, 0x40, 0x50 };
                    //UInt32 val = BitConverter.ToUInt32(raw,0);
                   // UInt32 val2 = BitConverter.ToUInt32(cmp, 0);
                    //val = (val << 8) + hex_out[s + 1];
                    //val = (val << 8) + hex_out[s];
                    //if (val == 9225)
                    if (hex_out[s] == 0x09)
                    {
                        if (hex_out[s + 1] == 0x24)
                        {
                            found[f_index] = start;
                            f_index++;
                        }
                    }
                    start++; 
                }
                //txt_npc_name.Text = start.ToString();
                scan_bar.Value = i;
            }
            
            foreach (UInt64 vl in found)
            {
                if (vl == 0x0)
                    break;

                s_dt.Rows.Add(vl, 9225);
            }
            
            MessageBox.Show("NPC Scanning finished.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        /*
        private void _scanner1_DoWork(object sender, DoWorkEventArgs e)
        {
            const int sRange = 999999;
            const int nScan = 0xFFFFFFF / sRange;
            UInt64 start = 0x02000000;
            byte[] hex_out = new byte[sRange + 1];
            UInt64[] found = new UInt64[5000];
            int f_index = 0;
            //short
            int br = 0;
            scan_bar.Maximum = nScan;
            for (int i = 0; i < 5000; i++)
                found[i] = 0x0;

            for (int i = 0; i < nScan; i++)
            {
                hex_out = khan.ReadMemory((IntPtr)start, sRange + 1, out br);
                for (int s = 0; s < sRange; s++)
                {
                    byte[] raw = new byte[2] { hex_out[s], hex_out[s + 1] };
                    short val = BitConverter.ToInt16(raw, 0);
                    //val = (val << 8) + hex_out[s + 1];
                    //val = (val << 8) + hex_out[s];
                    if (val == 9225)
                    {
                        found[f_index] = start;
                        f_index++;
                    }
                    start++;
                }
                //txt_npc_name.Text = start.ToString();
                scan_bar.Value = i;
            }

            foreach (UInt64 vl in found)
            {
                if (vl == 0x0)
                    break;

                s_dt.Rows.Add(vl, 9225);
            }

            MessageBox.Show("NPC Scanning finished.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        */
        /*
        private void _scanner1_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 0;
            int rpt = 0;
            decimal split = 0;
            
            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
             }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        s_dt.Rows.Add(th_start + a, val);
                }
            }
        }
        */
        private void _scanner2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void _scanner3_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 2;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner4_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 3;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner5_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 4;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner6_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 5;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner7_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 6;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner8_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 7;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner9_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 8;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }

        private void _scanner10_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 9;
            int rpt = 0;
            decimal split = 0;

            if (range_per_thread > 2000000)
            {
                split = Convert.ToInt32((Convert.ToDecimal(range_per_thread) / (decimal)2000000));
                rpt = Convert.ToInt32(Convert.ToDecimal(range_per_thread) / split);
            }
            for (int s = 0; s <= split; s++)
            {
                uint th_start = ((uint)start_add + (uint)(s * rpt)) - 1;
                byte[] res = khan.ReadByte(th_start, (uint)rpt);

                for (int a = 0; a < (rpt - 1); a++)
                {
                    byte[] v = new byte[2] { 0x0, 0x0 };
                    v[0] = res[a];
                    v[1] = res[a + 1];
                    int val = (int)BitConverter.ToInt16(v, 0);
                    n_scanned[th_no]++;
                    if (val == 9225)
                        tab_NPC_Scan.Rows.Add(th_start + a, val);
                }
            }
        }
        /*
        private void _scanner10_DoWork(object sender, DoWorkEventArgs e)
        {
            int th_no = 9;
            uint th_start = (uint)start_add + ((uint)th_no * (uint)range_per_thread);
            byte[] res = khan.ReadByte(th_start, (uint)range_per_thread);

            for (int a = 0; a < range_per_thread - 1; a++)
            {
                byte[] v = new byte[2] { 0x0, 0x0 };
                v[0] = res[a];
                v[1] = res[a + 1];
                int val = (int)BitConverter.ToInt16(v, 0);
                n_scanned[th_no]++;
                if (val == 9225)
                    tab_NPC_Scan.Rows.Add(th_start + a, val);
            }
        }
        */
        private void scan_status_DoWork(object sender, DoWorkEventArgs e)
        {
            //newscan.Pos = 0;
            scan_bar.Maximum = newscan.MaxScan;
            scan_bar.Value = newscan.Pos;
            while (newscan.Pos < newscan.MaxScan)
            {
                scan_bar.Value = newscan.Pos;
                Thread.Sleep(5);
            }
            scan_bar.Value = newscan.MaxScan;
            if (newscan.ResultCount == 0)
                MessageBox.Show("Scan failed.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Question);
            else
                MessageBox.Show("Scan successful.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Question);
            btn_NPC_Search.Enabled = true;
        }
        private void scan_status_DoWork_XX(object sender, DoWorkEventArgs e)
        {
            scan_bar.Maximum = range_per_thread * n_scanner;
            int total = scan_range;
            scan_bar.Value = 0;
            int scan_total = 0;
            int dif = 0;
            while (scan_total < total) 
            {
                scan_total = 0;
                dif = 0;
                for (int k = 0; k < n_scanner; k++)
                    scan_total += n_scanned[k];
                if (scan_total <= scan_bar.Maximum)
                    scan_bar.Value = scan_total;
                //txt_npc_name.Text = scan_total.ToString() + " - " + scan_bar.Maximum.ToString();

                dif += scan_range - scan_total;
                if (dif < 100)
                    scan_total += dif;
                //grd_ScanResult.ResetBindings();
                Thread.Sleep(1);
            }
            MessageBox.Show("NPC Scanning is complete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btn_NPC_Search.Enabled = true;
        }

        private void frm_AddNpc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void frm_AddNpc_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen red = new Pen(Color.Red);
            Pen black = new Pen(Color.Black);
            Rectangle rect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            Rectangle rect_shad = new Rectangle(2, 2, this.Width - 4, this.Height - 4);
            g.DrawRectangle(red, rect);
            g.DrawRectangle(black, rect_shad);
        }

        private void frm_AddNpc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Escape)
            {
                this.Hide();
            }
        }

        private void _bg_hk_DoWork(object sender, DoWorkEventArgs e)
        {
            bool start = true;
            while (start)
            {
                if (Keyboard.IsKeyDown(Keys.Escape))
                    this.Hide();
                Thread.Sleep(100);
            }
        }
    }
}
