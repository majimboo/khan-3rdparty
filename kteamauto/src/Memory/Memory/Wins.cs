using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Memory
{
    public class Wins
    {
        public enum WindowLocation : byte
        {
            TopLeft = 0,
            TopRight = 1,
            BottomRight = 2,
            BottomLeft = 3,
            Center = 4,
            None = 5,
            TopCenter = 6,
            RightCenter = 7,
            BottomCenter = 8,
            LeftCenter = 9,
        }

        public enum WindowShowStyle : uint
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimized = 2,
            ShowMaximized = 3,
            Maximize = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimized = 11
        }

        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }

        public static IntPtr GetHandle(int processId, string className)
        {
            IntPtr window = GetWindow(GetForegroundWindow(), GetWindow_Cmd.GW_HWNDFIRST);
            StringBuilder builder = new StringBuilder(100);
            while (window != IntPtr.Zero)
            {
                GetClassName(window, builder, 100);
                if (builder.ToString().IndexOf(className) != -1)
                {
                    GetWindowThreadProcessId(window, out int num);
                    if (num == processId)
                        return window;
                }
                window = GetWindow(window, GetWindow_Cmd.GW_HWNDNEXT);
            }
            return IntPtr.Zero;
        }

        public static IntPtr GetHandle(string className, string windowText)
        {
            IntPtr window = GetWindow(GetForegroundWindow(), GetWindow_Cmd.GW_HWNDFIRST);
            StringBuilder builder = new StringBuilder(100);
            StringBuilder builderText = new StringBuilder(100);
            while (window != IntPtr.Zero)
            {
                GetClassName(window, builder, 100);
                GetWindowText(window, builderText, 100);
                if (builder.ToString().IndexOf(className) != -1 && builderText.ToString().IndexOf(windowText) != -1)
                {
                    return window;
                }
                window = GetWindow(window, GetWindow_Cmd.GW_HWNDNEXT);
            }
            return IntPtr.Zero;
        }

        public static void Active(IntPtr handle)
        {
            GetWindowRect(handle, out RECT rec);
            bool isMini = rec.Left == -32000;
            if (!IsWindowVisible(handle))
                ShowWindow(handle, WindowShowStyle.Show);
            if (isMini)
                ShowWindow(handle, WindowShowStyle.Restore);
            if (GetForegroundWindow() != handle)
                BringWindowToTop(handle);
        }

        public static void Move(Form form, WindowLocation location)
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHight = Screen.PrimaryScreen.Bounds.Height;
            switch (location)
            {
                case WindowLocation.TopLeft:
                    form.Location = new Point(0, 0);
                    break;

                case WindowLocation.TopRight:
                    form.Location = new Point(screenWidth - form.Width, 0);
                    break;

                case WindowLocation.BottomRight:
                    form.Location = new Point(screenWidth - form.Width, screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
                    break;

                case WindowLocation.BottomLeft:
                    form.Location = new Point(0, screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
                    break;

                case WindowLocation.Center:
                    form.Location = new Point((screenWidth - form.Width) / 2, (screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
                    break;

                case WindowLocation.None:
                    form.Location = new Point(-1000, -1000);
                    break;

                case WindowLocation.TopCenter:
                    form.Location = new Point((screenWidth - form.Width) / 2, 0);
                    break;

                case WindowLocation.RightCenter:
                    form.Location = new Point(screenWidth - form.Width, (screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
                    break;

                case WindowLocation.BottomCenter:
                    form.Location = new Point((screenWidth - form.Width) / 2, screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
                    break;

                case WindowLocation.LeftCenter:
                    form.Location = new Point(0, (screenHight - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
                    break;
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    }
}