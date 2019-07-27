using KhanEngine;
using Microsoft.Win32;
using Net.ControlModel;
using Setting;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Net.Model
{
    public class ComboxP : ObserverProperty
    {
        private string _Name = "";
        private int _ID = 0;
        private int _Cost = 0;
        private string _Msg = "";

        public string Name { get => _Name; set => OnPropertyChanged(ref _Name, value); }
        public int ID { get => _ID; set => OnPropertyChanged(ref _ID, value); }
        public int Cost { get => _Cost; set => OnPropertyChanged(ref _Cost, value); }
        public string Msg { get => _Msg; set => OnPropertyChanged(ref _Msg, value); }
    }

    public class Mill
    {
        public static IniFile IniFile = new IniFile();
        public static string ProcessesByName = Default.ProcessesByName;

        public static dynamic Change(int X, int Y)
        {
            return X.ToString() + "|" + Y.ToString();
        }

        public static string GetXaml()
        {
            return ReadXML.Read();
        }

        public static bool FilePath()
        {
            try
            {
                return File.Exists(ReadXML.FilePath());
            }
            catch
            {
                return false;
            }
        }

        public static void CreateXaml(string Name = "")
        {
            ReadXML.Create(Name);
        }

        public static void SetXaml(string Name = "")
        {
            ReadXML.SetNew(Name);
        }

        public static dynamic Change(dynamic ID, string Key = null)
        {
            if (Key == null) { return ID.ToString(); }
            switch (ID.GetTypeCode().ToString())
            {
                case "string":
                    return Key.ToString();

                case "Double":
                    return Convert.ToDouble(Key);

                case "Int32":
                    return Convert.ToInt32(Key);

                case "Int16":
                    return Convert.ToInt16(Key);

                case "Int64":
                    return Convert.ToInt64(Key);

                case "SByte":
                    return Convert.ToSByte(Key);

                case "Byte":
                    return Convert.ToByte(Key);

                case "Char":
                    return Convert.ToChar(Key);

                case "Boolean":
                    return Convert.ToBoolean(Key);

                default:
                    return Key.ToString();
            }
        }

        public static dynamic Change(string Key = null, string Value = null, dynamic ID = null)
        {
            if (Key == null && Value == null) { return ID.ToString(); }
            switch (ID.GetTypeCode().ToString())
            {
                case "string":
                    return IniRead(Key, Value).ToString();

                case "Double":
                    return Convert.ToDouble(IniRead(Key, Value));

                case "Int32":
                    return Convert.ToInt32(IniRead(Key, Value));

                case "Int16":
                    return Convert.ToInt16(IniRead(Key, Value));

                case "Int64":
                    return Convert.ToInt64(IniRead(Key, Value));

                case "SByte":
                    return Convert.ToSByte(IniRead(Key, Value));

                case "Byte":
                    return Convert.ToByte(IniRead(Key, Value));

                case "Char":
                    return Convert.ToChar(IniRead(Key, Value));

                case "Boolean":
                    return Convert.ToBoolean(IniRead(Key, Value));

                default:
                    return IniRead(Key, Value).ToString();
            }
        }

        public static string IniRead(string Key, string Value)
        {
            return IniFile.IniReadValue(Key, Value);
        }

        public static string[] IniRead(dynamic Key, dynamic Value)
        {
            return IniFile.IniReadValue(Key.ToString(), Value.ToString()).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] IniReadSection(dynamic Key)
        {
            return IniFile.IniReadSection(Key.ToString());
        }

        public static void IniWrite(dynamic Section, dynamic Key, dynamic Value)
        {
            IniFile.IniWriteValue(Section.ToString(), Key.ToString(), Value.ToString());
        }

        public static void IniWrite(dynamic Section, dynamic Key, int X, int Y)
        {
            IniFile.IniWriteValue(Section.ToString(), Key.ToString(), Change(X, Y));
        }
    }

    public class Show_hidden
    {
        public string GroupTrainShow { get { return Default.GroupTrainShow; } }

        public string AutoTrainShow { get { return Default.AutoTrainShow; } }

        public string KillToDieShow { get { return Default.KillToDieShow; } }

        public string AutoBuffShow { get { return Default.AutoBuffShow; } }

        public string MoveShow { get { return Default.MoveShow; } }

        public string ShutdownShow { get { return Default.ShutdownShow; } }

        public string BlockCordShow { get { return Default.BlockCordShow; } }

        public string Attack2Show { get { return Default.Attack2Show; } }
    }

    #region Model

    public class InfoView : ObserverProperty
    {
        private bool _IsCheckOpen = Default.IsCheckOpen;

        public Process CurrentProcess = Process.GetCurrentProcess();
        private string _Version = "0.0.0.0";
        private bool _IsEnabled = false;

        #region Public Data

        public string Version { get { return _Version; } set => OnPropertyChanged(ref _Version, value); }
        public bool IsEnabled { get { return _IsEnabled; } set => OnPropertyChanged(ref _IsEnabled, value); }
        public bool IsCheckOpen { get => _IsCheckOpen; set => OnPropertyChanged(ref _IsCheckOpen, value); }
        public int Current => CurrentProcess.Id;

        public InfoView()
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                var version = fvi.FileVersion;
                Version = version.ToString();
            }
            catch { }
        }

        #endregion Public Data
    }

    public class ListSkill : ObserverProperty
    {
        private Stopwatch _TimeLoad;
        private TimeSpan _TimeOut;
        private string _name = "N/A";
        private int _id = 0;
        private int _time = 0;
        private int _count = 0;

        #region Public data

        public string Name { get => _name; set => OnPropertyChanged(ref _name, value); }
        public int Id { get => _id; set => OnPropertyChanged(ref _id, value); }
        public int Time { get => _time; set => OnPropertyChanged(ref _time, value); }
        public int Count { get => _count; set => OnPropertyChanged(ref _count, value); }
        public Stopwatch TimeLoad { get => _TimeLoad; set => OnPropertyChanged(ref _TimeLoad, value); }
        public TimeSpan TimeOut { get => _TimeOut; set => OnPropertyChanged(ref _TimeOut, value); }

        #endregion Public data
    }

    public class ListChar : ObserverProperty
    {
        private string _ID = "N/A";
        private string _Name = "N/A";

        public string ID { get => _ID; set => OnPropertyChanged(ref _ID, value); }
        public string Name { get => _Name; set => OnPropertyChanged(ref _Name, value); }
    }

    public class C_HotKey : ObserverProperty
    {
        private string _Name;
        private Keys _Control;
        public string Name { get => _Name; set => OnPropertyChanged(ref _Name, value); }
        public Keys Control { get => _Control; set => OnPropertyChanged(ref _Control, value); }
    }

    public class LoadWor
    {
        public static bool Read { get; set; } = false;
    }

    #endregion Model

    #region File ini

    public class IniFile
    {
        public string @path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString
            (
                string section,
                string key,
                string val,
                string filePath
            );

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString
            (
                string section,
                string key,
                string def,
                StringBuilder retVal,
                int size,
                string filePath
            );

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern UInt32 GetPrivateProfileSection
           (
               [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
               [In] IntPtr pReturnedString,
               [In] UInt32 nSize,
               [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
           );

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>

        public void ChangePath(string INIPath)
        {
            path = INIPath;
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.@path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.@path);
            return temp.ToString();
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns>Array</returns>
        public string[] IniReadSection(string strSectionName)
        {
            IntPtr pBuffer = Marshal.AllocHGlobal(32767);
            string[] strArray = new string[0];
            UInt32 uiNumCharCopied = 0;

            uiNumCharCopied = GetPrivateProfileSection(strSectionName, pBuffer, 256, this.@path);

            int iStartAddress = pBuffer.ToInt32();
            int iEndAddress = iStartAddress + (int)uiNumCharCopied;

            while (iStartAddress < iEndAddress)
            {
                int iArrayCurrentSize = strArray.Length;
                Array.Resize<string>(ref strArray, iArrayCurrentSize + 1);
                string strCurrent = Marshal.PtrToStringAnsi(new IntPtr(iStartAddress));
                strArray[iArrayCurrentSize] = strCurrent;
                iStartAddress += (strCurrent.Length + 1);
            }

            Marshal.FreeHGlobal(pBuffer);
            pBuffer = IntPtr.Zero;

            return strArray;
        }

        public string Path()
        {
            return @path;
        }
    }

    #endregion File ini

    #region scan win

    public class NameProcess : ObserverProperty
    {
        private int _ID = 0;
        private Process _Name;

        public int ID
        {
            get => _ID;
            set
            {
                OnPropertyChanged(ref _ID, value);
                try
                {
                    Name = Process.GetProcessById(_ID);
                }
                catch
                {
                    Name = null;
                }
            }
        }

        public Process Name { get => _Name; set => OnPropertyChanged(ref _Name, value); }
    }

    public class WindowsFind
    {
        private static WindowFinder wf = new WindowFinder();
        public static ObservableCollection<NameProcess> Get = new ObservableCollection<NameProcess>();

        public static void FindWindows(string Class = "Khan_Project")
        {
            wf.FindWindows(0, new Regex(Class), null, null, new WindowFinder.FoundWindowCallback(FoundWindow));
        }

        private static bool FoundWindow(int handle)
        {
            // Print the window info.
            MAPN.GetWindowThreadProcessId(handle, out int pid);
            Get.Add(new NameProcess { ID = pid });

            // Continue on with next window.
            return true;
        }

        private void PrintWindowInfo(int handle)
        {
            //// Get the class.
            //StringBuilder sbClass = new StringBuilder(256);
            //MAPI.GetClassName(handle, sbClass, sbClass.Capacity);

            //// Get the text.
            //int txtLength = MAPI.SendMessage(handle, WM_GETTEXTLENGTH, 0, 0);
            //StringBuilder sbText = new StringBuilder(txtLength + 1);
            //MAPI.SendMessage(handle, WM_GETTEXT, sbText.Capacity, sbText);

            //// Now we can write out the information we have on the window.
            //Console.WriteLine("Handle: " + handle);
            //Console.WriteLine("Class : " + sbClass);
            //Console.WriteLine("Text  : " + sbText);
            //Console.WriteLine();
        }
    }

    public class WindowFinder
    {
        public const int WM_GETTEXT = 0x000D;

        public const int WM_GETTEXTLENGTH = 0x000E;

        // Win32 functions that have all been used in previous blogs.

        private event FoundWindowCallback foundWindow;

        public delegate bool FoundWindowCallback(int hWnd);

        private int parentHandle;

        private Regex className;
        private Regex windowText;
        private Regex process;

        public void FindWindows(int parentHandle, Regex className, Regex windowText, Regex process, FoundWindowCallback fwc)
        {
            this.parentHandle = parentHandle;
            this.className = className;
            this.windowText = windowText;
            this.process = process;

            foundWindow = fwc;

            MAPN.EnumChildWindows(parentHandle, new MAPN.PChildCallBack(EnumChildWindowsCallback), 0);
        }

        private bool EnumChildWindowsCallback(int handle, int lParam)
        {
            // If a class name was provided, check to see if it matches the window.
            if (className != null)
            {
                StringBuilder sbClass = new StringBuilder(256);
                MAPN.GetClassName(handle, sbClass, sbClass.Capacity);

                // If it does not match, return true so we can continue on with the next window.
                if (!className.IsMatch(sbClass.ToString()))
                    return true;
            }

            // If a window text was provided, check to see if it matches the window.
            if (windowText != null)
            {
                int txtLength = MAPN.SendMessage(handle, WM_GETTEXTLENGTH, 0, 0);
                StringBuilder sbText = new StringBuilder(txtLength + 1);
                MAPN.SendMessage(handle, WM_GETTEXT, sbText.Capacity, sbText);

                // If it does not match, return true so we can continue on with the next window.
                if (!windowText.IsMatch(sbText.ToString()))
                    return true;
            }

            // If a process name was provided, check to see if it matches the window.
            if (process != null)
            {
                int processID;
                MAPN.GetWindowThreadProcessId(handle, out processID);

                // Now that we have the process ID, we can use the built in .NET function to obtain a process object.
                Process p = Process.GetProcessById(processID);

                // If it does not match, return true so we can continue on with the next window.
                if (!process.IsMatch(p.ProcessName))
                    return true;
            }

            // If we get to this point, the window is a match. Now invoke the foundWindow event and based upon
            // the return value, whether we should continue to search for windows.
            return foundWindow(handle);
        }
    }

    #endregion scan win

    #region Get Mac

    public static class HardwareAnalyzer
    {
        #region Methods: Imports

        [DllImport("Iphlpapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        internal static extern Int32 GetAdaptersInfo(IntPtr handle, ref UInt32 size);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean CloseHandle([In] IntPtr handle);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern Boolean DeviceIoControl([In] IntPtr handle, [In] UInt32 controlCode, [In, Optional] IntPtr bufferIn, [In] UInt32 bufferInSize, [Out, Optional] IntPtr bufferOut, [In] UInt32 bufferOutSize, [Out] out UInt32 bytesReturned, [In, Out, Optional] IntPtr overlapped);

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
        internal static extern IntPtr CreateFile([In] String fileName, [In] EFileAccess fileAccess, [In] EFileShare fileShare, [In, Optional] IntPtr fileSecurity, [In] ECreationDisposition creationDisposition, [In] UInt32 flags, [In, Optional] IntPtr handleTemplateFile);

        #endregion Methods: Imports

        #region Methods: Functions

        public static string GetMacAddress()
        {
            return ReadMacAddress().ToString();
        }

        public static PhysicalAddress ReadMacAddress()
        {
            var myInterfaceAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .OrderByDescending(n => n.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                .Select(n => n.GetPhysicalAddress())
                .FirstOrDefault();

            return myInterfaceAddress;
        }

        public static string GetMD5()
        {
            string str_md5 = "";
            byte[] mang = Encoding.UTF8.GetBytes(CreateFingerprint());

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
        }

        public static string GetMD5(string Key)
        {
            string str_md5 = "";
            byte[] mang = Encoding.UTF8.GetBytes(Key);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
        }

        private static String RetrieveDiskSerial()
        {
            String serial = String.Empty;

            try
            {
                IntPtr handle = IntPtr.Zero;

                for (Int32 i = 0; i < 16; ++i)
                {
                    handle = CreateFile(String.Format("\\\\.\\PhysicalDrive{0}", i), (EFileAccess.GenericRead | EFileAccess.GenericWrite), (EFileShare.Read | EFileShare.Write), IntPtr.Zero, ECreationDisposition.OpenExisting, 0, IntPtr.Zero);

                    if (handle != IntPtr.Zero)
                    {
                        serial = RetrieveDiskSerialSmart(handle);

                        if (serial.Length == 0)
                            serial = RetrieveDiskSerialStorageQuery(handle);

                        if (serial.Length == 0)
                            continue;

                        if (!CloseHandle(handle))
                            Console.WriteLine("WARNING: a file handle has not been correctly closed.");

                        break;
                    }
                }
            }
            catch { }

            return serial;
        }

        private static String RetrieveDiskSerialSmart(IntPtr handle)
        {
            IntPtr bufferIn = Marshal.AllocHGlobal(32);
            IntPtr bufferOut = Marshal.AllocHGlobal(24);
            String serial = String.Empty;
            UInt32 bytesReturned = 0;

            try
            {
                if (DeviceIoControl(handle, 0x074080, IntPtr.Zero, 0, bufferOut, 24, out bytesReturned, IntPtr.Zero))
                {
                    if ((Marshal.ReadInt32(bufferOut, 4) & 4) > 0)
                    {
                        SCInputParameters parameters = new SCInputParameters();
                        bufferOut = Marshal.ReAllocHGlobal(bufferOut, (IntPtr)528);

                        Marshal.StructureToPtr(parameters, bufferIn, true);

                        if (DeviceIoControl(handle, 0x07C088, bufferIn, 32, bufferOut, 528, out bytesReturned, IntPtr.Zero))
                        {
                            String serialANSI = Marshal.PtrToStringAnsi((IntPtr)(bufferOut.ToInt32() + 36), 20);

                            if (serialANSI.Length != 0)
                            {
                                Char[] serialANSICharacters = serialANSI.ToCharArray();

                                for (Int32 i = 0; i <= (serialANSICharacters.Length - 2); i += 2)
                                {
                                    Char current = serialANSICharacters[i];

                                    serialANSICharacters[i] = serialANSICharacters[(i + 1)];
                                    serialANSICharacters[(i + 1)] = current;
                                }

                                serial = new String(serialANSICharacters).Trim();
                            }
                        }
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(bufferIn);
                Marshal.FreeHGlobal(bufferOut);
            }

            return serial;
        }

        private static String RetrieveDiskSerialStorageQuery(IntPtr handle)
        {
            IntPtr bufferIn = Marshal.AllocHGlobal(12);
            IntPtr bufferOut = Marshal.AllocHGlobal(1024);
            StoragePropertyQuery query = new StoragePropertyQuery();
            String serial = String.Empty;
            UInt32 bytesReturned = 0;

            try
            {
                Marshal.StructureToPtr(query, bufferIn, true);

                if (DeviceIoControl(handle, 0x2D1400, bufferIn, 12, bufferOut, 1024, out bytesReturned, IntPtr.Zero))
                {
                    Int32 address = bufferOut.ToInt32();
                    Int32 offset = Marshal.ReadInt32(bufferOut, 24);

                    if (offset != 0)
                    {
                        String serialANSI = Marshal.PtrToStringAnsi((IntPtr)(address + offset));

                        if (serialANSI.Length != 0)
                        {
                            StringBuilder builder = new StringBuilder();

                            for (Int32 i = 0; i < serialANSI.Length; i += 4)
                            {
                                for (Int32 j = 1; j >= 0; --j)
                                {
                                    Int32 sum = 0;

                                    for (Int32 y = 0; y < 2; ++y)
                                    {
                                        sum *= 16;

                                        switch (serialANSI[(i + (j * 2) + y)])
                                        {
                                            case '0':
                                                sum += 0;
                                                break;

                                            case '1':
                                                sum += 1;
                                                break;

                                            case '2':
                                                sum += 2;
                                                break;

                                            case '3':
                                                sum += 3;
                                                break;

                                            case '4':
                                                sum += 4;
                                                break;

                                            case '5':
                                                sum += 5;
                                                break;

                                            case '6':
                                                sum += 6;
                                                break;

                                            case '7':
                                                sum += 7;
                                                break;

                                            case '8':
                                                sum += 8;
                                                break;

                                            case '9':
                                                sum += 9;
                                                break;

                                            case 'a':
                                                sum += 10;
                                                break;

                                            case 'b':
                                                sum += 11;
                                                break;

                                            case 'c':
                                                sum += 12;
                                                break;

                                            case 'd':
                                                sum += 13;
                                                break;

                                            case 'e':
                                                sum += 14;
                                                break;

                                            case 'f':
                                                sum += 15;
                                                break;
                                        }
                                    }

                                    if (sum > 0)
                                        builder.Append((Char)sum);
                                }
                            }

                            serial = builder.ToString().Trim();
                        }
                    }
                }
            }
            finally
            {
                Marshal.FreeHGlobal(bufferIn);
                Marshal.FreeHGlobal(bufferOut);
            }

            return serial;
        }

        private static String RetrieveMACAddress()
        {
            String address = String.Empty;

            try
            {
                UInt32 size = 0;
                Int32 result = GetAdaptersInfo(IntPtr.Zero, ref size);

                if ((result == 0) || (result == 111))
                {
                    IntPtr buffer = Marshal.AllocHGlobal((IntPtr)size);
                    result = GetAdaptersInfo(buffer, ref size);

                    if (result == 0)
                    {
                        while (true)
                        {
                            String adapterName = Marshal.PtrToStringAnsi((IntPtr)(buffer.ToInt32() + 8));
                            IntPtr handle = CreateFile(String.Format("\\\\.\\{0}", adapterName), (EFileAccess.GenericRead | EFileAccess.GenericWrite), (EFileShare.Read | EFileShare.Write), IntPtr.Zero, ECreationDisposition.OpenExisting, 0, IntPtr.Zero);

                            if (handle != IntPtr.Zero)
                            {
                                IntPtr bufferIn = GCHandle.Alloc(0x1010101, GCHandleType.Pinned).AddrOfPinnedObject();
                                IntPtr bufferOut = Marshal.AllocHGlobal(6);
                                UInt32 bytesReturned = 0;

                                try
                                {
                                    if (DeviceIoControl(handle, 0x170002, bufferIn, 4, bufferOut, 6, out bytesReturned, IntPtr.Zero))
                                    {
                                        String temporaryAddress = String.Empty;

                                        for (Int32 i = 0; i < 6; ++i)
                                            temporaryAddress += Marshal.ReadByte(bufferOut, i).ToString("X2") + ((i == 5) ? "" : ":");

                                        if (temporaryAddress != "00:00:00:00:00:00")
                                        {
                                            address = temporaryAddress;
                                            break;
                                        }
                                    }
                                }
                                finally
                                {
                                    if (!CloseHandle(handle))
                                        Console.WriteLine("WARNING: a file handle has not been correctly closed.");

                                    Marshal.FreeHGlobal(bufferOut);
                                }
                            }

                            Int32 nextAdapterOffset = Marshal.ReadInt32(buffer);

                            if (nextAdapterOffset != 0)
                                buffer = (IntPtr)nextAdapterOffset;
                            else
                                break;
                        }
                    }
                }
            }
            catch { }

            return address;
        }

        private static String RetrieveSMBiosData()
        {
            String data = String.Empty;

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\MSSMBios\Data", false))
                {
                    if (key != null)
                    {
                        Byte[] keyData = (Byte[])key.GetValue("SMBiosData");

                        if (keyData != null)
                        {
                            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
                                keyData = provider.ComputeHash(keyData);

                            for (Int32 i = 0; i < keyData.Length; ++i)
                                data += keyData[i].ToString("X2");
                        }
                    }
                }
            }
            catch { }

            return data;
        }

        public static String CreateFingerprint()
        {
            String serial = RetrieveDiskSerial();
            String address = RetrieveMACAddress();
            String data = RetrieveSMBiosData();

            if ((serial.Length == 0) && (address.Length == 0) && (data.Length == 0))
                return "0000-0000-0000-0000-0000-0000-0000-0000";

            String fingerprint = String.Empty;

            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                Byte[] hash = provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(serial + " - " + address + " - " + data));

                for (Int32 i = 0; i < 16; ++i)
                {
                    fingerprint += hash[i].ToString("X2");

                    if (((i & 1) != 0) && (i != 15))
                        fingerprint += "-";
                }
            }

            return fingerprint;
        }

        #endregion Methods: Functions

        #region Nesting: Enumerators

        public enum ECreationDisposition : uint
        {
            New = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5
        }

        [Flags]
        public enum EFileAccess : uint
        {
            Delete = 0x00010000,
            ReadControl = 0x00020000,
            WriteDAC = 0x00040000,
            WriteOwner = 0x00080000,
            Synchronize = 0x00100000,
            AccessSystemSecurity = 0x01000000,
            MaximumAllowed = 0x02000000,
            GenericAll = 0x10000000,
            GenericExecute = 0x20000000,
            GenericWrite = 0x40000000,
            GenericRead = 0x80000000
        }

        [Flags]
        public enum EFileShare : uint
        {
            None = 0x00000000,
            Read = 0x00000001,
            Write = 0x00000002,
            Delete = 0x00000004
        }

        #endregion Nesting: Enumerators

        #region Nesting: Structures

        [StructLayout(LayoutKind.Sequential)]
        private class SCInputParameters
        {
            private readonly int BufferSize = 528;
            private readonly Byte Features = 0;
            private readonly Byte SectorCount = 1;
            private readonly Byte SectorNumber = 1;
            private readonly Byte LowOrderCylinder = 0;
            private readonly Byte HighOrderCylinder = 0;
            private readonly Byte DriveHead = 160;
            private readonly Byte Command = 236;
            private readonly Byte Reserved = 0;
            private readonly Byte DriveNumber = 0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            private readonly Byte[] UselessData = new Byte[16];
        }

        [StructLayout(LayoutKind.Sequential)]
        private class StoragePropertyQuery
        {
            private readonly Int32 PropertyID;
            private readonly Int32 QueryType;
            private readonly Int32 UselessData;
        }

        #endregion Nesting: Structures
    }

    #endregion Get Mac
}