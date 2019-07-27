using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Net.ControlModel
{
    public static class MAPN
    {
        public delegate bool PChildCallBack(int hWnd, int lParam);

        [DllImport("User32.Dll")]
        public static extern void GetClassName(int hWnd, StringBuilder s, int nMaxCount);

        [DllImport("User32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

        //[DllImport("user32")]
        //public static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        [DllImport("user32.Dll")]
        public static extern Boolean EnumChildWindows(int hWndParent, PChildCallBack lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow();

        [DllImport("user32")]
        public static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string text);

        public static Int32 GetWindowProcessID(Int32 hwnd)
        {
            GetWindowThreadProcessId(hwnd, out int pid);
            return pid;
        }

        public static bool WinActive(int Pid)
        {
            Int32 hwnd = 0;
            int appProcessName = 0;
            try
            {
                hwnd = GetForegroundWindow();
                appProcessName = Process.GetProcessById(GetWindowProcessID(hwnd)).Id;
                return (appProcessName == Pid);
            }
            catch
            {
                return false;
            }
        }

        public static int WinActive()
        {
            Int32 hwnd = 0;
            try
            {
                hwnd = GetForegroundWindow();
                return Process.GetProcessById(GetWindowProcessID(hwnd)).Id;
            }
            catch
            {
                return 0;
            }
        }

        public static bool KeyIsDown(Keys key)
        {
            return (GetAsyncKeyState(key) < 0);
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