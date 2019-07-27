using Net.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Net.Connect
{
    public static class ConnectData
    {
        private static WebClient Wb = new WebClient();
        private static Random random = new Random();

        public static string ServerUrl { get; set; } = "http://home.kteamauto.com/inc/ctl.php?app=";
        public static string ServerName { get; set; } = "Khan Blaze";
        public static string Mac { get; set; } = HardwareAnalyzer.CreateFingerprint();
        public static string Username { get; set; } = "";
        public static string Password { get; set; } = "";
        public static string Token { get; set; } = "";
        public static bool LogoutKey { get; set; } = false;

        private static string RandomString(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool RandomToken(int length = 20)
        {
            try
            {
                Token = RandomString(length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Function server

        public static string Logout(bool quit = false)
        {
            try
            {
                string Data = "";
                if (quit || LogoutKey)
                {
                    Data = Wb.DownloadString(string.Format("{0}{1}&username={2}&password={3}&server={4}", ServerUrl, "Logout", Username, Password, ServerName));
                    //MessageBox.Show("Bạn đã đăng xuất!", "Thông báo");
                    return Data;
                }
                return "false";
            }
            catch
            {
                return "Error";
            }
        }

        public static string Login()
        {
            try
            {
                if (string.IsNullOrEmpty(Username))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng nhập", "Thông báo");
                    return "NullFill";
                }
                string loginResult = Wb.DownloadString(string.Format("{0}{1}&username={2}&password={3}", ServerUrl, "Login", Username, Password));
                switch (loginResult)
                {
                    case "wrong":
                    case "Exception":
                    case "false":
                    case "Shell":
                        return loginResult;
                }
                return GetDataInfo();
            }
            catch
            {
                return "false";
            }
        }

        public static string GetDataInfo()
        {
            try
            {
                return Wb.DownloadString(string.Format("{0}{1}&username={2}&server={3}", ServerUrl, "GetDataInfo", Username, ServerName));
            }
            catch
            {
                return "{'credits':'0','username':'','email':'','gender':'male','is_admin':'0','register_date':'00/00/0000','birthday':'0000 - 00 - 00','account':0,'banned':'0'}";
            }
        }

        public static string GetAuthen()
        {
            try
            {
                return Wb.DownloadString(string.Format("{0}{1}&username={2}&server={3}", ServerUrl, "AuthenAccounts", Username, ServerName));
            }
            catch
            {
                return "false";
            }
        }

        public static object Website()
        {
            try
            {
                return JsonConvert.DeserializeObject(Wb.DownloadString(string.Format("{0}{1}", ServerUrl, "Website")));
            }
            catch
            {
                return "false";
            }
        }

        public static string CostPay()
        {
            try
            {
                return Wb.DownloadString(string.Format("{0}{1}&server={2}", ServerUrl, "CostPay", ServerName));
            }
            catch
            {
                return "false";
            }
        }

        public static string Payment(int client, int payment)
        {
            try
            {
                return Wb.DownloadString(string.Format("{0}{1}&username={2}&client={3}&payment={4}&server={5}", ServerUrl, "Payment", Username, client, payment, ServerName));
            }
            catch
            {
                return "false";
            }
        }

        public static string InInsertOnline()
        {
            try
            {
                return Wb.DownloadString(string.Format("{0}{1}&username={2}&mac={3}&token={4}&server={5}", ServerUrl, "InsertOnline", Username, Mac, Token, ServerName));
            }
            catch
            {
                return "Error";
            }
        }

        public static string Limit()
        {
            try
            {
                return Wb.DownloadString(string.Format("{0}{1}&username={2}&server={3}", ServerUrl, "Limit", Username, ServerName));
            }
            catch
            {
                return "0";
            }
        }

        public static dynamic Online()
        {
            string _Data = "{'Online':'Error','Client':0,'Client':0}";
            try
            {
                _Data = Wb.DownloadString(string.Format("{0}{1}&username={2}&server={3}", ServerUrl, "Online", Username, ServerName));
                return JsonConvert.DeserializeObject(_Data);
            }
            catch
            {
                return JsonConvert.DeserializeObject(_Data);
            }
        }

        #endregion Function server
    }
}