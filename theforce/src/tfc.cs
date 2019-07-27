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

namespace TheForce
{
    public class tfc
    {
        const int PROCESS_WM_WRITE = 0x0020;
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_OPERATION = 0x0008;
        int PROCESS_ALL_ACCESS = (0x1F0FFF);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess,
               bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
          byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress,
            byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public static uint FindDmaAddy(Int32 hProcHandle, int BaseAdress, uint[] offsets, int PointerLevel)
        {
            int pointer = BaseAdress;
            byte[] pTemp = new byte[4] { 0, 0,0,0};
            //byte[] pointerAddr = new byte[4];
            uint pointerAddr = 0;
            int bytesRead = 0;
            for (int c = 0; c < PointerLevel; c++)
            {
                if (c == 0)
                {
                    ReadProcessMemory(hProcHandle, pointer, pTemp, pTemp.Length, ref bytesRead);
                }
                pointerAddr = BitConverter.ToUInt32(pTemp,0) + offsets[c];
                ReadProcessMemory((int)hProcHandle, Convert.ToInt32(pointerAddr), pTemp, pTemp.Length, ref bytesRead);
            }
            return pointerAddr;
        }

        public static short ReadMemory2Byte(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel)
        {
            byte[] value = new byte[2];
            int bytesRead = 0;
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            if (ReadProcessMemory(hProcHandle, (int)ActToRead, value, value.Length, ref bytesRead) == false)
            {
                return 0;
            }
            return BitConverter.ToInt16(value, 0);
        }
        public static int ReadMemoryInt(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel)
        {
            byte[] value = new byte[4];
            int bytesRead = 0;
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            if (ReadProcessMemory(hProcHandle, (int)ActToRead, value, value.Length, ref bytesRead) == false)
            {
                return 0;
            }
            return BitConverter.ToInt32(value, 0);
        }
        public static void WriteMemoryByte(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel, byte value)
        {
            int bytesRead = 0;
            byte[] w_val = BitConverter.GetBytes(value);
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            WriteProcessMemory(hProcHandle, (int)ActToRead, w_val, w_val.Length, ref bytesRead);
        }
        public static void WriteMemory2Byte(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel, short value)
        {
            int bytesRead = 0;
            byte[] w_val = BitConverter.GetBytes(value);
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            WriteProcessMemory(hProcHandle, (int)ActToRead, w_val, w_val.Length, ref bytesRead);
        }

        public static void WriteMemoryInt(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel, int value)
        {
            int bytesRead = 0;
            byte[] w_val = BitConverter.GetBytes(value);
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            WriteProcessMemory(hProcHandle, (int)ActToRead, w_val, w_val.Length, ref bytesRead);
        }
        public static void WriteMemoryDouble(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel, double value)
        {
            int bytesRead = 0;
            byte[] w_val = BitConverter.GetBytes(value);
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            WriteProcessMemory(hProcHandle, (int)ActToRead, w_val, w_val.Length, ref bytesRead);
        }
        public static void WriteMemoryFloat(Int32 hProcHandle, uint BaseAddress, uint[] offsets, int PointerLevel, float value)
        {
            int bytesRead = 0;
            byte[] w_val = BitConverter.GetBytes(value);
            uint ActToRead = FindDmaAddy(hProcHandle, (int)BaseAddress, offsets, PointerLevel);
            WriteProcessMemory(hProcHandle, (int)ActToRead, w_val, w_val.Length, ref bytesRead);
        }
    }
}
