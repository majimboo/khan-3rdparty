using System.Collections.Generic;
using System.Windows.Forms;

namespace Setting
{
    public class Hotkey
    {
        public dynamic KeyControls = new Dictionary<string, Keys>();
        public dynamic HotKey = new Dictionary<string, Keys>();

        public Hotkey()
        {
            // Control key
            KeyControls["None"] = Keys.None;
            KeyControls["Shift"] = Keys.ShiftKey;
            KeyControls["Ctrl"] = Keys.ControlKey;
            KeyControls["Alt"] = Keys.Alt;
            // Hotkey
            HotKey["A"] = Keys.A;
            HotKey["B"] = Keys.B;
            HotKey["C"] = Keys.C;
            HotKey["D"] = Keys.D;
            HotKey["E"] = Keys.E;
            HotKey["F"] = Keys.F;
            HotKey["G"] = Keys.G;
            HotKey["H"] = Keys.H;
            HotKey["I"] = Keys.I;
            HotKey["J"] = Keys.J;
            HotKey["K"] = Keys.K;
            HotKey["L"] = Keys.L;
            HotKey["M"] = Keys.M;
            HotKey["N"] = Keys.N;
            HotKey["O"] = Keys.O;
            HotKey["P"] = Keys.P;
            HotKey["Q"] = Keys.Q;
            HotKey["R"] = Keys.R;
            HotKey["S"] = Keys.S;
            HotKey["T"] = Keys.T;
            HotKey["U"] = Keys.U;
            HotKey["V"] = Keys.V;
            HotKey["W"] = Keys.W;
            HotKey["X"] = Keys.X;
            HotKey["Y"] = Keys.Y;
            HotKey["Z"] = Keys.Z;
            HotKey["F1"] = Keys.F1;
            HotKey["F2"] = Keys.F2;
            HotKey["F3"] = Keys.F3;
            HotKey["F4"] = Keys.F4;
            HotKey["F5"] = Keys.F5;
            HotKey["F6"] = Keys.F6;
            HotKey["F7"] = Keys.F7;
            HotKey["F8"] = Keys.F8;
            HotKey["F9"] = Keys.F9;
            HotKey["F10"] = Keys.F10;
            HotKey["F11"] = Keys.F11;
            HotKey["F12"] = Keys.F12;
            HotKey["0"] = Keys.D0;
            HotKey["1"] = Keys.D1;
            HotKey["2"] = Keys.D2;
            HotKey["3"] = Keys.D3;
            HotKey["4"] = Keys.D4;
            HotKey["5"] = Keys.D5;
            HotKey["6"] = Keys.D6;
            HotKey["7"] = Keys.D7;
            HotKey["8"] = Keys.D8;
            HotKey["9"] = Keys.D9;
        }
    }
}