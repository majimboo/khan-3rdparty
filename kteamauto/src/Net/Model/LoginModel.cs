using KhanEngine;
using System.Collections.ObjectModel;
using System.Threading;

namespace Net.Model
{
    public class LoginModel : ObserverProperty
    {
        private string _KeyStart = "2ADFE278C0C6D466237A";
        private bool _Login = true; // kiem tra dang nhap
        private string _id = ""; // Tài khoản
        private string _Email = ""; // Email
        private string _gender = ""; // Giới tính
        private string _password = ""; // Mật khẩu
        private string _type = ""; // Loại tài khoản
        private string _balance = ""; // Xu (tiền trong tài khoản)
        private string _regDate = ""; // Ngày đăng ký
        private int _RegClient = 0;// Số lượng client đã đăng ký

        private string _Server = "";
        private Thread _CheckOnline; // Khởi tạo MultiThread online

        private int _Max_limit_crep = 0;
        private string _PayCost = "";
        private string _PayCostAll = "";
        private string _MsgEvent = "";
        private string _TMsgEvent = "";
        private ComboxP _iClients = new ComboxP();
        private ComboxP _iPayments = new ComboxP();

        private ObservableCollection<ComboxP> _sPaymemts = new ObservableCollection<ComboxP>();
        private ObservableCollection<ComboxP> _sClients = new ObservableCollection<ComboxP>();

        public ComboxP IPayment
        {
            get => _iPayments;
            set
            {
                OnPropertyChanged(ref _iPayments, value);
                if (_iPayments != null)
                {
                    PayCost = _iPayments.Cost.ToString("#,###");
                    PayCostAll = (_iPayments.Cost * _iClients.Cost).ToString("#,###");
                }
            }
        }

        public ComboxP IClients
        {
            get => _iClients;
            set
            {
                OnPropertyChanged(ref _iClients, value);
                PayCostAll = (_iPayments.Cost * _iClients.Cost).ToString("#,###");
            }
        }

        public ObservableCollection<ComboxP> SPaymemts { get => _sPaymemts; set => OnPropertyChanged(ref _sPaymemts, value); }
        public ObservableCollection<ComboxP> SClients { get => _sClients; set => OnPropertyChanged(ref _sClients, value); }

        public string Id { get => _id; set => OnPropertyChanged(ref _id, value); }
        public string Password { get => _password; set => OnPropertyChanged(ref _password, value); }
        public string Type { get => _type; set => OnPropertyChanged(ref _type, value); }
        public string Balance { get => _balance; set => OnPropertyChanged(ref _balance, value); }
        public string RegDate { get => _regDate; set => OnPropertyChanged(ref _regDate, value); }
        public string Server { get => _Server; set => OnPropertyChanged(ref _Server, value); }
        public string MsgEvent { get => _MsgEvent; set => OnPropertyChanged(ref _MsgEvent, value); }
        public string TMsgEvent { get => _TMsgEvent; set => OnPropertyChanged(ref _TMsgEvent, value); }
        public string PayCost { get => _PayCost; set => OnPropertyChanged(ref _PayCost, value); }
        public string PayCostAll { get => _PayCostAll; set => OnPropertyChanged(ref _PayCostAll, value); }
        public Thread CheckOnline { get => _CheckOnline; set => OnPropertyChanged(ref _CheckOnline, value); }
        public string Email { get => _Email; set => OnPropertyChanged(ref _Email, value); }
        public string Gender { get => _gender; set => OnPropertyChanged(ref _gender, value); }
        public bool Login { get => _Login; set => OnPropertyChanged(ref _Login, value); }
        public int RegClient { get => _RegClient; set => OnPropertyChanged(ref _RegClient, value); }
        public int Max_limit_crep { get => _Max_limit_crep; set => OnPropertyChanged(ref _Max_limit_crep, value); }
        public string KeyStart { get => _KeyStart; set => OnPropertyChanged(ref _KeyStart, value); }
    }
}