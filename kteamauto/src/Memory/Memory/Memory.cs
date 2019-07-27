using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Memory
{
    public class Memory : MAPI
    {
        public IntPtr hReadProcess = IntPtr.Zero;
        public IntPtr handleWindow = IntPtr.Zero;
        public IntPtr mHandle = IntPtr.Zero;

        public Process mReadProcess;
        public string mName = "";

        public bool SetName(string name)
        {
            if (name != "")
            {
                mName = name;
                return true;
            }
            return false;
        }

        public bool CloseHandle()
        {
            int iRetValue;
            iRetValue = CloseHandle(hReadProcess);
            if (iRetValue == 0)
            {
                return true;
            }
            return false;
        }

        public bool OpenHandle(int id)
        {
            IntPtr pro = OpenProcess(0x001F0FFF, false, id);
            if (pro != IntPtr.Zero)
            {
                hReadProcess = pro;
                return true;
            }
            else
                return false;
        }

        public int BaseAddress()
        {
            return mReadProcess.MainModule.BaseAddress.ToInt32();
        }

        public bool SetTitle(IntPtr hWnd, string text)
        {
            if (hWnd != IntPtr.Zero)
            {
                SetWindowText(hWnd, text);
                return true;
            }
            else
                return false;
        }

        public int BaseAddress(string sModuleName)
        {
            return FindModule(sModuleName).BaseAddress.ToInt32();
        }

        public bool BytesEqual(byte[] bBytes_1, byte[] bBytes_2)
        {
            return (BitConverter.ToString(bBytes_1) == BitConverter.ToString(bBytes_2));
        }

        public int CalculatePointer(int iMemoryAddress, int[] iOffsets)
        {
            int num = iOffsets.Length - 1;
            byte[] bBuffer = new byte[4];
            int num2 = 0;
            if (num == 0)
            {
                num2 = iMemoryAddress;
            }
            for (int i = 0; i <= num; i++)
            {
                IntPtr ptr;
                if (i == num)
                {
                    ReadProcessMemory(hReadProcess, (IntPtr)num2, bBuffer, 4, out ptr);
                    return (Dec(CreateAddress(bBuffer)) + iOffsets[i]);
                }
                if (i == 0)
                {
                    ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 4, out ptr);
                    num2 = Dec(CreateAddress(bBuffer)) + iOffsets[0];
                }
                else
                {
                    ReadProcessMemory(hReadProcess, (IntPtr)num2, bBuffer, 4, out ptr);
                    num2 = Dec(CreateAddress(bBuffer)) + iOffsets[i];
                }
            }
            return 0;
        }

        public int CalculateStaticAddress(int iStaticOffset)
        {
            return (BaseAddress() + iStaticOffset);
        }

        public int CalculateStaticAddress(string sStaticOffset)
        {
            return (BaseAddress() + Dec(sStaticOffset));
        }

        public int CalculateStaticAddress(int iStaticOffset, string sModuleName)
        {
            return (BaseAddress(sModuleName) + iStaticOffset);
        }

        public int CalculateStaticAddress(string sStaticOffset, string sModuleName)
        {
            return (BaseAddress(sModuleName) + Dec(sStaticOffset));
        }

        private string CreateAddress(byte[] bBytes)
        {
            string str = "";
            for (int i = 0; i < bBytes.Length; i++)
            {
                if (Convert.ToInt16(bBytes[i]) < 0x10)
                {
                    str = "0" + bBytes[i].ToString("X") + str;
                }
                else
                {
                    str = bBytes[i].ToString("X") + str;
                }
            }
            return str;
        }

        private byte[] CreateAOBString(string sBytes)
        {
            return BitConverter.GetBytes(Dec(sBytes));
        }

        private byte[] CreateAOBText(string sBytes)
        {
            return Encoding.ASCII.GetBytes(sBytes);
        }

        public int Dec(int iHex)
        {
            return int.Parse(iHex.ToString(), NumberStyles.HexNumber);
        }

        public int Dec(string sHex)
        {
            return int.Parse(sHex, NumberStyles.HexNumber);
        }

        public int EntryPoint()
        {
            return mReadProcess.MainModule.EntryPointAddress.ToInt32();
        }

        public int EntryPoint(string sModuleName)
        {
            return FindModule(sModuleName).EntryPointAddress.ToInt32();
        }

        public string FileVersion()
        {
            return mReadProcess.MainModule.FileVersionInfo.FileVersion;
        }

        private ProcessModule FindModule(string sModuleName)
        {
            for (int i = 0; i < mReadProcess.Modules.Count; i++)
            {
                if (mReadProcess.Modules[i].ModuleName == sModuleName)
                {
                    return mReadProcess.Modules[i];
                }
            }
            return null;
        }

        public ProcessModuleCollection GetModules()
        {
            return mReadProcess.Modules;
        }

        public string Hex(int iDec)
        {
            return iDec.ToString("X");
        }

        public string Hex(string sDec)
        {
            if (IsNumeric(sDec))
            {
                return int.Parse(sDec).ToString("X");
            }
            return "0";
        }

        public bool IsNumeric(string sNumber)
        {
            return new Regex(@"^\d+$").IsMatch(sNumber);
        }

        public int MemorySize()
        {
            return mReadProcess.MainModule.ModuleMemorySize;
        }

        public int MemorySize(string sModuleName)
        {
            return FindModule(sModuleName).ModuleMemorySize;
        }

        public string Name()
        {
            return mReadProcess.ProcessName;
        }

        public bool NOP(int iMemoryAddress, int iLength)
        {
            byte[] bBuffer = new byte[iLength];
            for (int i = 0; i < iLength; i++)
            {
                bBuffer[i] = 0x90;
            }
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, (uint)iLength, out IntPtr ptr);
            return (ptr.ToInt32() == iLength);
        }

        public bool NOP(int iMemoryAddress, int[] iOffsets, int iLength)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[iLength];
            for (int i = 0; i < iLength; i++)
            {
                bBuffer[i] = 0x90;
            }
            WriteProcessMemory(hReadProcess, (IntPtr)num, bBuffer, (uint)iLength, out IntPtr ptr);
            return (ptr.ToInt32() == bBuffer.Length);
        }

        public bool OpenProcess(int processID)
        {
            hReadProcess = OpenProcess(0x001F0FFF, false, processID);
            if (hReadProcess != IntPtr.Zero)
            {
                return true;
            }
            return false;
        }

        public int PID()
        {
            return mReadProcess.Id;
        }

        public byte[] ReadAOB(int iMemoryAddress, uint iBytesToRead)
        {
            byte[] bBuffer = new byte[iBytesToRead];
            ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, iBytesToRead, out IntPtr ptr);
            return bBuffer;
        }

        public byte[] ReadAOB(int iMemoryAddress, int[] iOffsets, uint iBytesToRead)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[1];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, iBytesToRead, out IntPtr ptr);
            return bBuffer;
        }

        public byte ReadByte(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[1];
            if (ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 1, out IntPtr ptr) == 0)
            {
                return 0;
            }
            return bBuffer[0];
        }

        public byte ReadByte(int iMemoryAddress, int[] iOffsets)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[1];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 1, out IntPtr ptr);
            return bBuffer[0];
        }

        public double ReadDouble(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[8];
            if (ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 8, out IntPtr ptr) == 0)
            {
                return 0.0;
            }
            return BitConverter.ToDouble(bBuffer, 0);
        }

        public double ReadDouble(int iMemoryAddress, int[] iOffsets)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[8];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 8, out IntPtr ptr);
            return BitConverter.ToDouble(bBuffer, 0);
        }

        public float ReadFloat(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[4];
            if (ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 4, out IntPtr ptr) == 0)
            {
                return 0f;
            }
            return BitConverter.ToSingle(bBuffer, 0);
        }

        public float ReadFloat(int iMemoryAddress, int[] iOffsets)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[4];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 4, out IntPtr ptr);
            return BitConverter.ToSingle(bBuffer, 0);
        }

        public int ReadInt(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[4];
            if (ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 4, out IntPtr ptr) == 0)
            {
                return 0;
            }
            return BitConverter.ToInt32(bBuffer, 0);
        }

        public int ReadInt(int iMemoryAddress, int[] iOffsets)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[4];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 4, out IntPtr ptr);
            return BitConverter.ToInt32(bBuffer, 0);
        }

        public int ReadIntOnly(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[4];
            ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 4, out IntPtr ptr);
            return BitConverter.ToInt32(bBuffer, 0);
        }

        public long ReadLong(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[8];
            if (ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 8, out IntPtr ptr) == 0)
            {
                return 0L;
            }
            return BitConverter.ToInt64(bBuffer, 0);
        }

        public long ReadLong(int iMemoryAddress, int[] iOffsets)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[8];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 8, out IntPtr ptr);
            return BitConverter.ToInt64(bBuffer, 0);
        }

        public short ReadShort(int iMemoryAddress)
        {
            byte[] bBuffer = new byte[2];
            if (ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 2, out IntPtr ptr) == 0)
            {
                return 0;
            }
            return BitConverter.ToInt16(bBuffer, 0);
        }

        public short ReadShort(int iMemoryAddress, int[] iOffsets)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[2];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 2, out IntPtr ptr);
            return BitConverter.ToInt16(bBuffer, 0);
        }

        public string ReadString(int iMemoryAddress, int[] iOffsets, uint iTextLength, int iMode = 0)
        {
            byte[] bBuffer = new byte[iTextLength];
            int iFinalAddress = CalculatePointer(iMemoryAddress, iOffsets);
            ReadProcessMemory(hReadProcess, (IntPtr)iFinalAddress, bBuffer, iTextLength, out IntPtr ptr);
            if (iMode == 0)
            {
                return Converter.ToUnicode(Encoding.Default.GetString(bBuffer));
            }
            if (iMode == 1)
            {
                return BitConverter.ToString(bBuffer).Replace("-", "");
            }
            return "";
        }

        public string ReadText(int iMemoryAddress, int[] iOffsets, uint iStringLength, int iMode = 0)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[1];
            ReadProcessMemory(hReadProcess, (IntPtr)num, bBuffer, iStringLength, out IntPtr ptr);
            if (iMode == 0)
            {
                return Converter.ToUnicode(Encoding.UTF8.GetString(bBuffer));
            }
            if (iMode == 1)
            {
                return BitConverter.ToString(bBuffer).Replace("-", "");
            }
            return "";
        }

        public string ReadName(int iMemoryAddress, int[] iOffsets, uint iStringLength)
        {
            int iFinalAddress = CalculatePointer(iMemoryAddress, iOffsets), count = 0;
            byte[] bBuffer = new byte[iStringLength];
            ReadProcessMemory(hReadProcess, (IntPtr)iFinalAddress, bBuffer, iStringLength, out IntPtr ptrBytesRead);
            if (BitConverter.ToInt32(bBuffer, 0) == 0)
            {
                return "";
            }
            else
            {
                for (int i = 0; i < bBuffer.Length; i++)
                {
                    if (bBuffer[i] == 00) { count = (bBuffer.Length - i); break; }
                }
                string result = Encoding.Default.GetString(bBuffer, 0, bBuffer.Length - count).Trim();
                return result;
            }
        }

        public string ReadName(int iMemoryAddress, uint iStringLength)
        {
            int count = 0;
            byte[] bBuffer = new byte[iStringLength];
            ReadProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, iStringLength, out IntPtr ptrBytesRead);
            if (BitConverter.ToInt32(bBuffer, 0) == 0)
            {
                return "";
            }
            else
            {
                for (int i = 0; i < bBuffer.Length; i++)
                {
                    if (bBuffer[i] == 00) { count = (bBuffer.Length - i); break; }
                }
                string result = Encoding.Default.GetString(bBuffer, 0, bBuffer.Length - count).Trim();
                return result;
            }
        }

        public byte[] ReverseBytes(byte[] bOriginalBytes)
        {
            int length = bOriginalBytes.Length;
            byte[] buffer = new byte[length];
            for (int i = 0; i < length; i++)
            {
                buffer[(length - i) - 1] = bOriginalBytes[i];
            }
            return buffer;
        }

        public int SID()
        {
            return mReadProcess.SessionId;
        }

        public string StartTime()
        {
            return mReadProcess.StartTime.ToString();
        }

        public bool Write(int iMemoryAddress, byte bByteToWrite)
        {
            byte[] bBuffer = new byte[] { bByteToWrite };
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, 1, out IntPtr ptr);
            return (ptr.ToInt32() == 1);
        }

        public bool Write(int iMemoryAddress, double iDoubleToWrite)
        {
            byte[] bytes = BitConverter.GetBytes(iDoubleToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bytes, 8, out IntPtr ptr);
            return (ptr.ToInt32() == 8);
        }

        public bool Write(int iMemoryAddress, short iShortToWrite)
        {
            byte[] bytes = BitConverter.GetBytes(iShortToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bytes, 2, out IntPtr ptr);
            return (ptr.ToInt32() == 2);
        }

        public bool Write(int iMemoryAddress, int iIntToWrite)
        {
            byte[] bytes = BitConverter.GetBytes(iIntToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bytes, 4, out IntPtr ptr);
            return (ptr.ToInt32() == 4);
        }

        public bool Write(int iMemoryAddress, long iLongToWrite)
        {
            byte[] bytes = BitConverter.GetBytes(iLongToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bytes, 8, out IntPtr ptr);
            return (ptr.ToInt32() == 8);
        }

        public bool Write(int iMemoryAddress, float iFloatToWrite)
        {
            byte[] bytes = BitConverter.GetBytes(iFloatToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bytes, 4, out IntPtr ptr);
            return (ptr.ToInt32() == 4);
        }

        public bool Write(int iMemoryAddress, byte[] bBytesToWrite)
        {
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBytesToWrite, (uint)bBytesToWrite.Length, out IntPtr ptr);
            return (ptr.ToInt32() == bBytesToWrite.Length);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, byte bByteToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[] { bByteToWrite };
            WriteProcessMemory(hReadProcess, (IntPtr)num, bBuffer, 1, out IntPtr ptr);
            return (ptr.ToInt32() == 1);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, double iDoubleToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bytes = BitConverter.GetBytes(iDoubleToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)num, bytes, 8, out IntPtr ptr);
            return (ptr.ToInt32() == 8);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, short iShortToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bytes = BitConverter.GetBytes(iShortToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)num, bytes, 2, out IntPtr ptr);
            return (ptr.ToInt32() == 2);
        }

        public bool Write(int iMemoryAddress, string sStringToWrite, int iMode = 0)
        {
            byte[] bBuffer = new byte[1];
            if (iMode == 0)
            {
                bBuffer = CreateAOBText(sStringToWrite);
            }
            else if (iMode == 1)
            {
                bBuffer = ReverseBytes(CreateAOBString(sStringToWrite));
            }
            WriteProcessMemory(hReadProcess, (IntPtr)iMemoryAddress, bBuffer, (uint)bBuffer.Length, out IntPtr ptr);
            return (ptr.ToInt32() == bBuffer.Length);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, int iIntToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bytes = BitConverter.GetBytes(iIntToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)num, bytes, 4, out IntPtr ptr);
            return (ptr.ToInt32() == 4);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, long iLongToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bytes = BitConverter.GetBytes(iLongToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)num, bytes, 8, out IntPtr ptr);
            return (ptr.ToInt32() == 8);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, float iFloatToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bytes = BitConverter.GetBytes(iFloatToWrite);
            WriteProcessMemory(hReadProcess, (IntPtr)num, bytes, 4, out IntPtr ptr);
            return (ptr.ToInt32() == 4);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, byte[] bBytesToWrite)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            WriteProcessMemory(hReadProcess, (IntPtr)num, bBytesToWrite, (uint)bBytesToWrite.Length, out IntPtr ptr);
            return (ptr.ToInt32() == bBytesToWrite.Length);
        }

        public bool Write(int iMemoryAddress, int[] iOffsets, string sStringToWrite, int iMode = 0)
        {
            int num = CalculatePointer(iMemoryAddress, iOffsets);
            byte[] bBuffer = new byte[1];
            if (iMode == 0)
            {
                bBuffer = CreateAOBText(sStringToWrite);
            }
            else if (iMode == 1)
            {
                bBuffer = ReverseBytes(CreateAOBString(sStringToWrite));
            }
            WriteProcessMemory(hReadProcess, (IntPtr)num, bBuffer, (uint)sStringToWrite.Length, out IntPtr ptr);
            return (ptr.ToInt32() == sStringToWrite.Length);
        }

        public bool WriteBytesAsm(IntPtr iMemoryAddress, byte[] iOffsets)
        {
            int size = iOffsets.Length;
            WriteProcessMemory(hReadProcess, iMemoryAddress, iOffsets, (uint)size, out IntPtr ptrBytesWritten);
            int error = Marshal.GetLastWin32Error();
            if (error == 0 || error == 299 || error == 1400)
            {
                return (ptrBytesWritten.ToInt32() == iOffsets.Length);
            }
            else { return false; }
        }
    }

    public class Converter
    {
        private static char[] tcvnchars = {
            'à', 'á', 'ä', 'ã', 'Õ',
            'å', '¢', '¡', 'Æ', 'Ç', '£',
            'â', '¥', '¤', '¦', 'ç', '§',
            'ð', 'è', 'é', 'ë', '¨', '©',
            'ê', '«', 'ª', '¬', '­', '®',
            'ì', 'í', 'ï', 'î', '¸',
            'ò', 'ó', 'ö', 'õ', '÷',
            'ô', '°', '¯', '±', '²', 'µ',
            '½', '¶', '¾', '·', 'Þ', 'þ',
            'ù', 'ú', 'ü', 'û', 'ø',
            'ß', '×', 'Ñ', 'Ø', 'æ', 'ñ',
            'Ï', 'ý', 'Ö', 'Û', 'Ü',
            'Å', 'Â', 'Ð', 'Ê', 'Ô', '´', '¿'
        };

        private static readonly char[] unichars = {
            'à', 'á', 'ả', 'ã', 'ạ',
            'ă', 'ằ', 'ắ', 'ẳ', 'ẵ', 'ặ',
            'â', 'ầ', 'ấ', 'ẩ', 'ẫ', 'ậ',
            'đ', 'è', 'é', 'ẻ', 'ẽ', 'ẹ',
            'ê', 'ề', 'ế', 'ể', 'ễ', 'ệ',
            'ì', 'í', 'ỉ', 'ĩ', 'ị',
            'ò', 'ó', 'ỏ', 'õ', 'ọ',
            'ô', 'ồ', 'ố', 'ổ', 'ỗ', 'ộ',
            'ơ', 'ờ', 'ớ', 'ở', 'ỡ', 'ợ',
            'ù', 'ú', 'ủ', 'ũ', 'ụ',
            'ư', 'ừ', 'ứ', 'ử', 'ữ', 'ự',
            'ỳ', 'ý', 'ỷ', 'ỹ', 'ỵ',
            'Ă', 'Â', 'Đ', 'Ê', 'Ô', 'Ơ', 'Ư'
        };

        private static readonly char[] convertTable;

        static Converter()
        {
            convertTable = new char[256];
            for (int i = 0; i < 256; i++)
                convertTable[i] = (char)i;
            for (int i = 0; i < tcvnchars.Length; i++)
                convertTable[tcvnchars[i]] = unichars[i];
        }

        public static char ConverterEX(char C)
        {
            char charConvert = new char();
            for (int i = 0; i < 74; i++)
            {
                if (C == unichars[i])
                {
                    charConvert = tcvnchars[i];
                    return charConvert;
                }
            }
            return charConvert;
        }

        public static string ToVISCII(string value)
        {
            char[] chars = value.ToCharArray();
            for (int i = 0; i < value.Length; i++)
            {
                if (ConverterEX(chars[i]) != 0)
                {
                    chars[i] = ConverterEX(chars[i]);
                }
            }
            return new string(chars);
        }

        public static string ToUnicode(string value)
        {
            char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                if (chars[i] < (char)256)
                    chars[i] = convertTable[chars[i]];
            string rstr = new string(chars);
            return rstr;
        }
    }
}