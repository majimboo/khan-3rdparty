using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Memory
{
    public class Assembly : Reading
    {
        public IntPtr addressAlloc = IntPtr.Zero;
        public IntPtr addressAllocV1 = IntPtr.Zero;

        public bool AllocMemory(int Size = 200)
        {
            addressAlloc = AllocEx(hReadProcess, Size);
            if (addressAlloc == IntPtr.Zero) { return false; }
            return true;
        }

        public bool AllocMemoryAll(int Size = 200)
        {
            addressAllocV1 = AllocEx(hReadProcess, Size);
            if (addressAllocV1 == IntPtr.Zero) { return false; }
            return true;
        }

        #region Assembly

        public string ToHex(int iDec, int n)
        {
            string Hex = "";
            string toHex = "0000000" + iDec.ToString("X");
            toHex = toHex.Substring(toHex.Length - n);
            for (int i = 0; i < (toHex.Length / 2); i++)
            {
                Hex += toHex.Substring(toHex.Length - 2 - 2 * i, 2);
            }
            return Hex;
        }

        public void AddEsp(int i, ref string Item)
        {
            Item += "83C4" + ToHex(i, 2);
        }

        public void MovEax(int i, ref string Item)
        {
            Item += "B8" + ToHex(i, 8);
        }

        public void MovEbx(int i, ref string Item)
        {
            Item += "BB" + ToHex(i, 8);
        }

        public void MovEcx(int i, ref string Item)
        {
            Item += "B9" + ToHex(i, 8);
        }

        public void MovEdx(int i, ref string Item)
        {
            Item += "BA" + ToHex(i, 8);
        }

        public void MovEsi(int i, ref string Item)
        {
            Item += "BE" + ToHex(i, 8);
        }

        public void MovEdi(int i, ref string Item)
        {
            Item += "BF" + ToHex(i, 8);
        }

        public void Push(int i, ref string Item)
        {
            Item += "68" + ToHex(i, 8);
        }

        public void PushEax(ref string Item)
        {
            Item += "50";
        }

        public void PushEdx(ref string Item)
        {
            Item += "52";
        }

        public void PushEcx(ref string Item)
        {
            Item += "51";
        }

        public void PushEbx(ref string Item)
        {
            Item += "53";
        }

        public void PushEsi(ref string Item)
        {
            Item += "56";
        }

        public void PushEdi(ref string Item)
        {
            Item += "57";
        }

        public void CallEax(ref string Item)
        {
            Item += "FFD0";
        }

        public void CallEbx(ref string Item)
        {
            Item += "FFD3";
        }

        public void CallEcx(ref string Item)
        {
            Item += "FFD1";
        }

        public void CallEdx(ref string Item)
        {
            Item += "FFD2";
        }

        public void CallEsi(ref string Item)
        {
            Item += "FFD6";
        }

        public void CallEdi(ref string Item)
        {
            Item += "FFD7";
        }

        public void PushAD(ref string Item)
        {
            Item += "60";
        }

        public void Ret(ref string Item)
        {
            Item += "C3";
        }

        public void PopAD(ref string Item)
        {
            Item += "61";
        }

        #endregion Assembly

        public IntPtr AllocEx(IntPtr process, int dataSize)
        {
            return MAPI.VirtualAllocEx(process, IntPtr.Zero, (uint)dataSize, MAPI.AllocationType.Commit | MAPI.AllocationType.Reserve, MAPI.MemoryProtection.ExecuteReadWrite);
        }

        public IntPtr RemoteThread(IntPtr address)
        {
            return MAPI.CreateRemoteThread(hReadProcess, IntPtr.Zero, 0, address, IntPtr.Zero, 0, IntPtr.Zero);
        }

        public void WaitThread(IntPtr threadId)
        {
            MAPI.WaitForSingleObject(threadId, 10000);
        }

        public void Close(IntPtr handle)
        {
            MAPI.CloseHandle(handle);
        }

        public bool FreeEx(IntPtr process, IntPtr address, int dataSize)
        {
            return MAPI.VirtualFreeEx(process, address, dataSize, MAPI.FreeType.Decommit);
        }

        public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[(str.Length / 2)];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(str.Substring(0 + 2 * i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bytes;
        }

        public void Injection(string Opcodes)
        {
            try
            {
                if (!Login) { return; }
                if (addressAlloc != IntPtr.Zero)
                {
                    byte[] bytesWrite = GetBytes(Opcodes);
                    if (WriteBytesAsm(addressAlloc, bytesWrite) == false)
                    {
                        return;
                    }
                    IntPtr thread = RemoteThread(addressAlloc);
                    if (thread == IntPtr.Zero) { return; }
                    WaitThread(thread);
                    Close(thread);
                }
                else { return; }
            }
            catch { return; }
        }

        public void Injection(string Opcodes, IntPtr NewressAllocs)
        {
            try
            {
                if (!Login) { return; }
                if (NewressAllocs != IntPtr.Zero)
                {
                    byte[] bytesWrite = GetBytes(Opcodes);
                    if (WriteBytesAsm(NewressAllocs, bytesWrite) == false) { return; }
                    IntPtr thread = RemoteThread(NewressAllocs);
                    if (thread == IntPtr.Zero) { return; }
                    WaitThread(thread);
                    Close(thread);
                }
                else { return; }
            }
            catch { return; }
        }

        public int LastError()
        {
            return Marshal.GetLastWin32Error();
        }
    }
}