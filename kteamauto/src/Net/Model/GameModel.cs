using KhanEngine;
using Lang;
using Memory;
using Setting;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace Net.Model
{
    public class GameModel : ObserverProperty
    {
        private ObservableCollection<AccountModel> _listAccount = new ObservableCollection<AccountModel>();
        private bool _IsEnable = true;
        private int _Clients = 0;
        private bool _WinActiveOld = true;
        private bool _WinActiveAttack = true;
        private bool _IsAttack2 = Default.IsAttackV;
        private Thread _WinActive;
        private Thread _THotkeyAuto;

        #region Public data

        public ObservableCollection<AccountModel> ListAccount { get => _listAccount; set => OnPropertyChanged(ref _listAccount, value); }

        public int Clients { get => _Clients; set => OnPropertyChanged(ref _Clients, value); }
        public bool IsEnable { get => _IsEnable; set => OnPropertyChanged(ref _IsEnable, value); }
        public Thread WinActive { get => _WinActive; set => OnPropertyChanged(ref _WinActive, value); }
        public Thread THotkeyAuto { get => _THotkeyAuto; set => OnPropertyChanged(ref _THotkeyAuto, value); }
        public bool WinActiveOld { get => _WinActiveOld; set => OnPropertyChanged(ref _WinActiveOld, value); }
        public bool WinActiveAttack { get => _WinActiveAttack; set => OnPropertyChanged(ref _WinActiveAttack, value); }

        public bool IsAttack2 { get => _IsAttack2; set => OnPropertyChanged(ref _IsAttack2, value); }

        #endregion Public data
    }

    public class AccountModel : ObserverProperty
    {
        private static Language Language = new Language();
        private int _CortStart = 0;
        public int CortStart { get => _CortStart; set => OnPropertyChanged(ref _CortStart, value); }

        /// <summary>
        /// Các thông số cố định quản lý client game
        /// </summary>
        private Function _Func = new Function(); // Khởi tạo Function (chứa memory đọc/ghi/assembly)

        private int _pid; // Khởi tạo PID client game
        private Thread _tAutoTrain; // Khởi tạo MultiThread auto train
        private Thread _FastClick; // Khởi tạo MultiThread tang toc click
        private Thread _SetSkill2; // Khởi tạo MultiThread Skill 2
        private Thread _tAuto; // Khởi tạo MultiThread auto
        private Thread _tAddcort; // Khởi tạo MultiThread thêm tọa độ

        private int _Tene = 0;
        private int _IDCort = 0;
        private string _Start = Language.StartAutos.Replace(" : ", "");
        private string _Status = Language.StopAuto.Replace(" Auto : ", "");
        private string _name = Language.NA; // Tên nhân vật
        private string _hp = Language.NA; // HP nhân vật
        private string _mp = Language.NA; // MP nhân vật
        private string _exp = Language.NA; // EXP nhân vật
        private string _level = Language.NA; // Level nhân vật
        private int _Limit_Crep = 0; // Giới hạn mob
        private string _Click = Language.MouseClick[0];
        private string _CharClass = Language.NA;
        private ObservableCollection<ListCord> _listCord = new ObservableCollection<ListCord>(); // Listview tọa độ

        // Main setting

        private bool _isAuto = Default.IsAuto; // Bật auto
        private bool _IsHp = Default.IsHp;
        private bool _IsMp = Default.IsMp;
        private bool _IsExP = Default.IsExP;
        private bool _IsCut = Default.IsCut;
        private bool _IsClickSpeed = Default.IsClickSpeed;
        private bool _IsAutoClick = Default.IsAutoClick;
        private bool _IsMouseClick = Default.IsMouseClick;

        private bool _IsAttack = Default.IsAttack;
        private bool _IsAttack2 = Default.IsAttack;

        private bool[] _IsBuff = new bool[4]
        {
            Default.IsBuff_1,
            Default.IsBuff_2,
            Default.IsBuff_3,
            Default.IsBuff_4
        };

        // Main Auto

        private Stopwatch[] _TimeBuff = new Stopwatch[4]
        {
            Stopwatch.StartNew(),
            Stopwatch.StartNew(),
            Stopwatch.StartNew(),
            Stopwatch.StartNew()
        };

        private int[] _ListIdSkill = new int[5];

        private bool _IsAutoTrain = Default.IsAutoTrain;
        private bool _IsKillToDie = Default.IsKillToDie;
        private bool _IsAddCort = Default.IsAddCort;
        private bool _IsAutoBuff = Default.IsAutoBuff;
        private bool _IsMove = Default.IsMove;
        private bool _IsBlockCord = Default.IsBlockCord;
        private bool _IsShutdown = Default.IsShutdown;
        // Input main

        private string _AddCortText = Language.Addcort[0];
        private int _Autoinput = Default.Autoinput;
        private int _HpInput = Default.HpInput;
        private int _HpShutDown = Default.HpShutDown;
        private int _MpInput = Default.MpInput;
        private int _ExpInput = Default.ExpInput;
        private int _CutInput = Default.CutInput;

        private int _IDSKIll = Default.IDSKIll;
        private int _TimeSkill = Default.TimeSkill;

        private int _XR = 0;
        private int _YR = 0;

        private double _ReadRangeinput = Default.ReadRangeinput;
        private double _RadiusInput = Default.RadiusInput;
        private int _LagInput = Default.LagInput;

        #region Public Hotkey

        #region Create

        private ObservableCollection<C_HotKey> _Source_S_Start = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_Start = new C_HotKey();
        private int _Index_S_Start = Default.VStartAuto;

        private ObservableCollection<C_HotKey> _Source_E_Start = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_Start = new C_HotKey();
        private int _Index_E_Start = Default.VaStartAuto;

        private ObservableCollection<C_HotKey> _Source_S_Stop = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_Stop = new C_HotKey();
        private int _Index_S_Stop = Default.VStopAuto;

        private ObservableCollection<C_HotKey> _Source_E_Stop = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_Stop = new C_HotKey();
        private int _Index_E_Stop = Default.VaStopAuto;

        private ObservableCollection<C_HotKey> _Source_S_StartTrain = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_StartTrain = new C_HotKey();
        private int _Index_S_StartTrain = Default.VStartTrain;

        private ObservableCollection<C_HotKey> _Source_E_StartTrain = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_StartTrain = new C_HotKey();
        private int _Index_E_StartTrain = Default.VaStartTrain;

        private ObservableCollection<C_HotKey> _Source_S_StopTrain = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_StopTrain = new C_HotKey();
        private int _Index_S_StopTrain = Default.VStopTrain;

        private ObservableCollection<C_HotKey> _Source_E_StopTrain = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_StopTrain = new C_HotKey();
        private int _Index_E_StopTrain = Default.VaStopTrain;

        private ObservableCollection<C_HotKey> _Source_S_StartBuff = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_StartBuff = new C_HotKey();
        private int _Index_S_StartBuff = Default.VStartBuff;

        private ObservableCollection<C_HotKey> _Source_E_StartBuff = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_StartBuff = new C_HotKey();
        private int _Index_E_StartBuff = Default.VaStartBuff;

        private ObservableCollection<C_HotKey> _Source_S_OnOff = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_OnOff = new C_HotKey();
        private int _Index_S_OnOff = Default.VOnOffAutoClick;

        private ObservableCollection<C_HotKey> _Source_E_OnOff = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_OnOff = new C_HotKey();
        private int _Index_E_OnOff = Default.VaOnOffAutoClick;

        private ObservableCollection<C_HotKey> _Source_S_Left = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_Left = new C_HotKey();
        private int _Index_S_Left = Default.VClickLeft;

        private ObservableCollection<C_HotKey> _Source_E_Left = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_Left = new C_HotKey();
        private int _Index_E_Left = Default.VaClickLeft;

        private ObservableCollection<C_HotKey> _Source_S_Right = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_S_Right = new C_HotKey();
        private int _Index_S_Right = Default.VClickRight;

        private ObservableCollection<C_HotKey> _Source_E_Right = new ObservableCollection<C_HotKey>();
        private C_HotKey _Item_E_Right = new C_HotKey();
        private int _Index_E_Right = Default.VaClickRight;

        #endregion Create

        #region Read

        public ObservableCollection<C_HotKey> Source_S_Start { get => _Source_S_Start; set => OnPropertyChanged(ref _Source_S_Start, value); }
        public C_HotKey Item_S_Start { get => _Item_S_Start; set => OnPropertyChanged(ref _Item_S_Start, value); }
        public int Index_S_Start { get => _Index_S_Start; set => OnPropertyChanged(ref _Index_S_Start, value); }
        public ObservableCollection<C_HotKey> Source_E_Start { get => _Source_E_Start; set => OnPropertyChanged(ref _Source_E_Start, value); }
        public C_HotKey Item_E_Start { get => _Item_E_Start; set => OnPropertyChanged(ref _Item_E_Start, value); }
        public int Index_E_Start { get => _Index_E_Start; set => OnPropertyChanged(ref _Index_E_Start, value); }
        public ObservableCollection<C_HotKey> Source_S_Stop { get => _Source_S_Stop; set => OnPropertyChanged(ref _Source_S_Stop, value); }
        public C_HotKey Item_S_Stop { get => _Item_S_Stop; set => OnPropertyChanged(ref _Item_S_Stop, value); }
        public int Index_S_Stop { get => _Index_S_Stop; set => OnPropertyChanged(ref _Index_S_Stop, value); }
        public ObservableCollection<C_HotKey> Source_E_Stop { get => _Source_E_Stop; set => OnPropertyChanged(ref _Source_E_Stop, value); }
        public C_HotKey Item_E_Stop { get => _Item_E_Stop; set => OnPropertyChanged(ref _Item_E_Stop, value); }
        public int Index_E_Stop { get => _Index_E_Stop; set => OnPropertyChanged(ref _Index_E_Stop, value); }
        public ObservableCollection<C_HotKey> Source_S_StartTrain { get => _Source_S_StartTrain; set => OnPropertyChanged(ref _Source_S_StartTrain, value); }
        public C_HotKey Item_S_StartTrain { get => _Item_S_StartTrain; set => OnPropertyChanged(ref _Item_S_StartTrain, value); }
        public int Index_S_StartTrain { get => _Index_S_StartTrain; set => OnPropertyChanged(ref _Index_S_StartTrain, value); }
        public ObservableCollection<C_HotKey> Source_E_StartTrain { get => _Source_E_StartTrain; set => OnPropertyChanged(ref _Source_E_StartTrain, value); }
        public C_HotKey Item_E_StartTrain { get => _Item_E_StartTrain; set => OnPropertyChanged(ref _Item_E_StartTrain, value); }
        public int Index_E_StartTrain { get => _Index_E_StartTrain; set => OnPropertyChanged(ref _Index_E_StartTrain, value); }
        public ObservableCollection<C_HotKey> Source_S_StopTrain { get => _Source_S_StopTrain; set => OnPropertyChanged(ref _Source_S_StopTrain, value); }
        public C_HotKey Item_S_StopTrain { get => _Item_S_StopTrain; set => OnPropertyChanged(ref _Item_S_StopTrain, value); }
        public int Index_S_StopTrain { get => _Index_S_StopTrain; set => OnPropertyChanged(ref _Index_S_StopTrain, value); }
        public ObservableCollection<C_HotKey> Source_E_StopTrain { get => _Source_E_StopTrain; set => OnPropertyChanged(ref _Source_E_StopTrain, value); }
        public C_HotKey Item_E_StopTrain { get => _Item_E_StopTrain; set => OnPropertyChanged(ref _Item_E_StopTrain, value); }
        public int Index_E_StopTrain { get => _Index_E_StopTrain; set => OnPropertyChanged(ref _Index_E_StopTrain, value); }
        public ObservableCollection<C_HotKey> Source_S_StartBuff { get => _Source_S_StartBuff; set => OnPropertyChanged(ref _Source_S_StartBuff, value); }
        public C_HotKey Item_S_StartBuff { get => _Item_S_StartBuff; set => OnPropertyChanged(ref _Item_S_StartBuff, value); }
        public int Index_S_StartBuff { get => _Index_S_StartBuff; set => OnPropertyChanged(ref _Index_S_StartBuff, value); }
        public ObservableCollection<C_HotKey> Source_E_StartBuff { get => _Source_E_StartBuff; set => OnPropertyChanged(ref _Source_E_StartBuff, value); }
        public C_HotKey Item_E_StartBuff { get => _Item_E_StartBuff; set => OnPropertyChanged(ref _Item_E_StartBuff, value); }
        public int Index_E_StartBuff { get => _Index_E_StartBuff; set => OnPropertyChanged(ref _Index_E_StartBuff, value); }
        public ObservableCollection<C_HotKey> Source_S_OnOff { get => _Source_S_OnOff; set => OnPropertyChanged(ref _Source_S_OnOff, value); }
        public C_HotKey Item_S_OnOff { get => _Item_S_OnOff; set => OnPropertyChanged(ref _Item_S_OnOff, value); }
        public int Index_S_OnOff { get => _Index_S_OnOff; set => OnPropertyChanged(ref _Index_S_OnOff, value); }
        public ObservableCollection<C_HotKey> Source_E_OnOff { get => _Source_E_OnOff; set => OnPropertyChanged(ref _Source_E_OnOff, value); }
        public C_HotKey Item_E_OnOff { get => _Item_E_OnOff; set => OnPropertyChanged(ref _Item_E_OnOff, value); }
        public int Index_E_OnOff { get => _Index_E_OnOff; set => OnPropertyChanged(ref _Index_E_OnOff, value); }
        public ObservableCollection<C_HotKey> Source_S_Left { get => _Source_S_Left; set => OnPropertyChanged(ref _Source_S_Left, value); }
        public C_HotKey Item_S_Left { get => _Item_S_Left; set => OnPropertyChanged(ref _Item_S_Left, value); }
        public int Index_S_Left { get => _Index_S_Left; set => OnPropertyChanged(ref _Index_S_Left, value); }
        public ObservableCollection<C_HotKey> Source_E_Left { get => _Source_E_Left; set => OnPropertyChanged(ref _Source_E_Left, value); }
        public C_HotKey Item_E_Left { get => _Item_E_Left; set => OnPropertyChanged(ref _Item_E_Left, value); }
        public int Index_E_Left { get => _Index_E_Left; set => OnPropertyChanged(ref _Index_E_Left, value); }
        public ObservableCollection<C_HotKey> Source_S_Right { get => _Source_S_Right; set => OnPropertyChanged(ref _Source_S_Right, value); }
        public C_HotKey Item_S_Right { get => _Item_S_Right; set => OnPropertyChanged(ref _Item_S_Right, value); }
        public int Index_S_Right { get => _Index_S_Right; set => OnPropertyChanged(ref _Index_S_Right, value); }
        public ObservableCollection<C_HotKey> Source_E_Right { get => _Source_E_Right; set => OnPropertyChanged(ref _Source_E_Right, value); }
        public C_HotKey Item_E_Right { get => _Item_E_Right; set => OnPropertyChanged(ref _Item_E_Right, value); }
        public int Index_E_Right { get => _Index_E_Right; set => OnPropertyChanged(ref _Index_E_Right, value); }

        public void CreateHotkey()
        {
            ObservableCollection<C_HotKey> _Hotkeynew = new ObservableCollection<C_HotKey>();
            ObservableCollection<C_HotKey> _HotkeyControl = new ObservableCollection<C_HotKey>();

            Hotkey HotKey = new Hotkey();

            foreach (var item in HotKey.HotKey)
            {
                _Hotkeynew.Add(new C_HotKey { Name = item.Key, Control = item.Value });
            }
            foreach (var item in HotKey.KeyControls)
            {
                _HotkeyControl.Add(new C_HotKey { Name = item.Key, Control = item.Value });
            }

            Source_S_Start.Clear();
            Source_E_Start.Clear();
            Source_S_Stop.Clear();
            Source_E_Stop.Clear();
            Source_S_StartTrain.Clear();
            Source_E_StartTrain.Clear();
            Source_S_StopTrain.Clear();
            Source_E_StopTrain.Clear();
            Source_S_StartBuff.Clear();
            Source_E_StartBuff.Clear();
            Source_S_OnOff.Clear();
            Source_E_OnOff.Clear();
            Source_S_Left.Clear();
            Source_E_Left.Clear();
            Source_S_Right.Clear();
            Source_E_Right.Clear();

            Source_S_Start = _HotkeyControl;
            Source_E_Start = _Hotkeynew;
            Source_S_Stop = _HotkeyControl;
            Source_E_Stop = _Hotkeynew;
            Source_S_StartTrain = _HotkeyControl;
            Source_E_StartTrain = _Hotkeynew;
            Source_S_StopTrain = _HotkeyControl;
            Source_E_StopTrain = _Hotkeynew;
            Source_S_StartBuff = _HotkeyControl;
            Source_E_StartBuff = _Hotkeynew;
            Source_S_OnOff = _HotkeyControl;
            Source_E_OnOff = _Hotkeynew;
            Source_S_Left = _HotkeyControl;
            Source_E_Left = _Hotkeynew;
            Source_S_Right = _HotkeyControl;
            Source_E_Right = _Hotkeynew;
        }

        #endregion Read

        #endregion Public Hotkey

        #region id and skill

        #region create

        private int[] _STimeSkill = new int[4]
        {
            Default.TimeSkill_1,
            Default.TimeSkill_2,
            Default.TimeSkill_3,
            Default.TimeSkill_4
        };

        private int[] _LoadSkill = new int[]
        {
            Default.LoadSkill_1,
            Default.LoadSkill_2,
            Default.LoadSkill_3,
            Default.LoadSkill_4
        };

        private int _IComboxName = Default.IComboxName;

        private int _ISkillAttack = Default.ISkillAttack;
        private int _ISkillBuff_1 = Default.ISkillBuff_1;
        private int _ISkillBuff_2 = Default.ISkillBuff_2;
        private int _ISkillBuff_3 = Default.ISkillBuff_3;
        private int _ISkillBuff_4 = Default.ISkillBuff_4;

        public ListChar _iListClass = new ListChar();
        private ListSkill _iListAttack = new ListSkill();
        private ListSkill _iListBuff_1 = new ListSkill();
        private ListSkill _iListBuff_2 = new ListSkill();
        private ListSkill _iListBuff_3 = new ListSkill();
        private ListSkill _iListBuff_4 = new ListSkill();

        private ObservableCollection<ListChar> _listClass = new ObservableCollection<ListChar>(); // Combobox Skill Class
        private ObservableCollection<ListSkill> _listAttack = new ObservableCollection<ListSkill>(); // Combobox Skill attack
        private ObservableCollection<ListSkill> _listBuff_1 = new ObservableCollection<ListSkill>(); // Combobox Skill Buff 1
        private ObservableCollection<ListSkill> _listBuff_2 = new ObservableCollection<ListSkill>(); // Combobox Skill Buff 2
        private ObservableCollection<ListSkill> _listBuff_3 = new ObservableCollection<ListSkill>(); // Combobox Skill Buff 3
        private ObservableCollection<ListSkill> _listBuff_4 = new ObservableCollection<ListSkill>(); // Combobox Skill Buff 4

        #endregion create

        #region Call Class

        public Stopwatch[] TimeBuff { get => _TimeBuff; set => OnPropertyChanged(ref _TimeBuff, value); }

        public ListChar IListClass
        {
            get => _iListClass;
            set
            {
                OnPropertyChanged(ref _iListClass, value);
                if (_iListClass != null)
                {
                    CharClass = _iListClass.Name;
                    SkillAttack(_iListClass.Name);
                    ISkillAttack = 0;
                    ISkillBuff_1 = 0;
                    ISkillBuff_2 = 0;
                    ISkillBuff_3 = 0;
                    ISkillBuff_4 = 0;
                }
            }
        }

        public ListSkill IListAttack
        {
            get => _iListAttack;
            set
            {
                OnPropertyChanged(ref _iListAttack, value);
                if (_iListAttack != null)
                {
                    if (_iListAttack.Id > 0)
                    {
                        IsAttack = true;
                    }
                    else
                    {
                        IsAttack = false;
                    }
                }
            }
        }

        public ListSkill IListBuff_1
        {
            get => _iListBuff_1;
            set
            {
                OnPropertyChanged(ref _iListBuff_1, value);
                if (_iListBuff_1 != null)
                {
                    if (_iListBuff_1.Id > 0)
                    {
                        ListIdSkill[1] = _iListBuff_1.Id;
                        IsBuff[0] = true;
                    }
                    else
                    {
                        ListIdSkill[1] = 0;
                        IsBuff[0] = false;
                    }
                    STimeSkill[0] = _iListBuff_1.Time / 2;
                }
            }
        }

        public ListSkill IListBuff_2
        {
            get => _iListBuff_2;
            set
            {
                OnPropertyChanged(ref _iListBuff_2, value);
                if (_iListBuff_2 != null)
                {
                    if (_iListBuff_2.Id > 0)
                    {
                        ListIdSkill[2] = _iListBuff_2.Id;
                        IsBuff[1] = true;
                    }
                    else
                    {
                        ListIdSkill[2] = 0;
                        IsBuff[1] = false;
                    }
                    STimeSkill[1] = _iListBuff_2.Time / 2;
                }
            }
        }

        public ListSkill IListBuff_3
        {
            get => _iListBuff_3;
            set
            {
                OnPropertyChanged(ref _iListBuff_3, value);
                if (_iListBuff_3 != null)
                {
                    if (_iListBuff_3.Id > 0)
                    {
                        ListIdSkill[3] = _iListBuff_3.Id;
                        IsBuff[2] = true;
                    }
                    else
                    {
                        ListIdSkill[3] = 0;
                        IsBuff[2] = false;
                    }
                    STimeSkill[2] = _iListBuff_3.Time / 2;
                }
            }
        }

        public ListSkill IListBuff_4
        {
            get => _iListBuff_4;
            set
            {
                OnPropertyChanged(ref _iListBuff_4, value);
                if (_iListBuff_4 != null)
                {
                    if (_iListBuff_4.Id > 0)
                    {
                        ListIdSkill[4] = _iListBuff_4.Id;
                        IsBuff[3] = true;
                    }
                    else
                    {
                        ListIdSkill[4] = 0;
                        IsBuff[3] = false;
                    }
                    STimeSkill[3] = _iListBuff_4.Time / 2;
                }
            }
        }

        public ObservableCollection<ListChar> ListClass { get => _listClass; set => OnPropertyChanged(ref _listClass, value); }
        public ObservableCollection<ListSkill> ListAttack { get => _listAttack; set => OnPropertyChanged(ref _listAttack, value); }
        public ObservableCollection<ListSkill> ListBuff_1 { get => _listBuff_1; set => OnPropertyChanged(ref _listBuff_1, value); }
        public ObservableCollection<ListSkill> ListBuff_2 { get => _listBuff_2; set => OnPropertyChanged(ref _listBuff_2, value); }
        public ObservableCollection<ListSkill> ListBuff_3 { get => _listBuff_3; set => OnPropertyChanged(ref _listBuff_3, value); }
        public ObservableCollection<ListSkill> ListBuff_4 { get => _listBuff_4; set => OnPropertyChanged(ref _listBuff_4, value); }

        #endregion Call Class

        #region Read

        public int[] ListIdSkill { get => _ListIdSkill; set => OnPropertyChanged(ref _ListIdSkill, value); }
        public int IComboxName { get => _IComboxName; set => OnPropertyChanged(ref _IComboxName, value); }
        public int ISkillAttack { get => _ISkillAttack; set => OnPropertyChanged(ref _ISkillAttack, value); }
        public int ISkillBuff_1 { get => _ISkillBuff_1; set => OnPropertyChanged(ref _ISkillBuff_1, value); }
        public int ISkillBuff_2 { get => _ISkillBuff_2; set => OnPropertyChanged(ref _ISkillBuff_2, value); }
        public int ISkillBuff_3 { get => _ISkillBuff_3; set => OnPropertyChanged(ref _ISkillBuff_3, value); }
        public int ISkillBuff_4 { get => _ISkillBuff_4; set => OnPropertyChanged(ref _ISkillBuff_4, value); }
        public string AddCortText { get => _AddCortText; set => OnPropertyChanged(ref _AddCortText, value); }

        #endregion Read

        #region Event

        public void SkillAttack(string Name)
        {
            ObservableCollection<ListSkill> _ListAttack = new ObservableCollection<ListSkill>();
            ObservableCollection<ListSkill> _ListBuff = new ObservableCollection<ListSkill>();

            string[,] IdAttack = Skill.Name_Skill(Name, true);

            for (int i = 0; i < IdAttack.GetLength(0); i++)
            {
                _ListAttack.Add(new ListSkill { Name = IdAttack[i, 0], Id = int.Parse(IdAttack[i, 1]) });
            }

            string[,] IdBuff = Skill.Name_Skill(Name, false);
            for (int i = 0; i < IdBuff.GetLength(0); i++)
            {
                _ListBuff.Add(new ListSkill { Name = IdBuff[i, 0], Id = int.Parse(IdBuff[i, 1]), Time = int.Parse(IdBuff[i, 2]) });
            }
            ListAttack.Clear();
            ListBuff_1.Clear();
            ListBuff_2.Clear();
            ListBuff_3.Clear();
            ListBuff_4.Clear();

            ListAttack = _ListAttack;
            ListBuff_1 = _ListBuff;
            ListBuff_2 = _ListBuff;
            ListBuff_3 = _ListBuff;
            ListBuff_4 = _ListBuff;
        }

        public void AddClassChar()
        {
            try
            {
                string[] ListClassr = Skill.ClassName;
                ListClass.Clear();

                foreach (var item in ListClassr)
                {
                    ListClass.Add(new ListChar { ID = item, Name = item });
                }

                CharClass = ListClassr[0];
                SkillAttack(ListClassr[0]);
                CreateHotkey();
            }
            catch { }
        }

        #endregion Event

        #endregion id and skill

        #region Public data

        public ObservableCollection<ListCord> ListCord { get => _listCord; set => OnPropertyChanged(ref _listCord, value); }

        #region Event

        public bool IsAuto
        {
            get => _isAuto;
            set
            {
                OnPropertyChanged(ref _isAuto, value);
                if (_isAuto)
                {
                    Status = Language.StartAutos.Replace(" Auto : ", "");
                    Start = Language.StopAuto.Replace(" : ", "");
                }
                else
                {
                    Status = Language.StopAuto.Replace(" Auto : ", "");
                    Start = Language.StartAutos.Replace(" : ", "");
                }
            }
        }

        public bool IsCut
        {
            get => _IsCut;
            set
            {
                OnPropertyChanged(ref _IsCut, value);

                try
                {
                    Func.CutSkill = _IsCut;
                }
                catch { }
            }
        }

        public bool IsAutoTrain
        {
            get => _IsAutoTrain;
            set
            {
                OnPropertyChanged(ref _IsAutoTrain, value);
                try
                {
                    Func.ONTrain = _IsAutoTrain;
                }
                catch
                {
                    Func.ONTrain = false;
                }
            }
        }

        public bool IsKillToDie
        {
            get => _IsKillToDie;
            set
            {
                OnPropertyChanged(ref _IsKillToDie, value);
                try
                {
                    Func.Kill = _IsKillToDie;
                }
                catch
                {
                    Func.Kill = false;
                }
            }
        }

        public bool IsMove
        {
            get => _IsMove;
            set
            {
                OnPropertyChanged(ref _IsMove, value);
                try
                {
                    Func.IsMoveT = _IsMove;
                }
                catch
                {
                }
            }
        }

        public bool IsBlockCord
        {
            get => _IsBlockCord;
            set
            {
                OnPropertyChanged(ref _IsBlockCord, value);
                try
                {
                    Func.IsBlockCord = _IsBlockCord;
                }
                catch { Func.IsBlockCord = false; _IsBlockCord = false; }
            }
        }

        public int CutInput
        {
            get => _CutInput;
            set
            {
                OnPropertyChanged(ref _CutInput, value);
                try
                {
                    Func.TimeAttack = _CutInput;
                }
                catch { Func.TimeAttack = 0; }
            }
        }

        public double ReadRangeInput
        {
            get => _ReadRangeinput;
            set
            {
                OnPropertyChanged(ref _ReadRangeinput, value);
                try
                {
                    Func.ReadRange = _ReadRangeinput;
                }
                catch { }
            }
        }

        public double RadiusInput
        {
            get => _RadiusInput;
            set
            {
                OnPropertyChanged(ref _RadiusInput, value);
                try
                {
                    Func.RadiusScan = _RadiusInput;
                }
                catch { }
            }
        }

        public bool IsAttack
        {
            get => _IsAttack;
            set
            {
                OnPropertyChanged(ref _IsAttack, value);
                if (_iListAttack != null)
                {
                    if (_IsAttack && _iListAttack.Id > 0)
                    {
                        Func.MouseButton = 1;
                        Func.IdSkill = _iListAttack.Id;
                        ListIdSkill[0] = _iListAttack.Id;
                    }
                    else
                    {
                        ListIdSkill[0] = 0;
                        Func.MouseButton = 0;
                        Func.IdSkill = 0;
                    }
                }
                else
                {
                    ListIdSkill[0] = 0;
                    Func.MouseButton = 0;
                    Func.IdSkill = 0;
                }
            }
        }

        #endregion Event

        #region Thread

        public Function Func { get => _Func; set => OnPropertyChanged(ref _Func, value); }
        public Thread TAutoTrain { get => _tAutoTrain; set => OnPropertyChanged(ref _tAutoTrain, value); }
        public Thread FastClick { get => _FastClick; set => OnPropertyChanged(ref _FastClick, value); }
        public Thread TAuto { get => _tAuto; set => OnPropertyChanged(ref _tAuto, value); }

        #endregion Thread

        #region Read

        public int Pid { get => _pid; set => OnPropertyChanged(ref _pid, value); }
        public string Name { get => _name; set => OnPropertyChanged(ref _name, value); }
        public string Hp { get => _hp; set => OnPropertyChanged(ref _hp, value); }
        public string Mp { get => _mp; set => OnPropertyChanged(ref _mp, value); }
        public string Exp { get => _exp; set => OnPropertyChanged(ref _exp, value); }
        public string Level { get => _level; set => OnPropertyChanged(ref _level, value); }
        public int Limit_Crep { get => _Limit_Crep; set => OnPropertyChanged(ref _Limit_Crep, value); }
        public string CharClass { get => _CharClass; set => OnPropertyChanged(ref _CharClass, value); }
        public string Start { get => _Start; set => OnPropertyChanged(ref _Start, value); }
        public bool IsHp { get => _IsHp; set => OnPropertyChanged(ref _IsHp, value); }
        public bool IsMp { get => _IsMp; set => OnPropertyChanged(ref _IsMp, value); }
        public bool IsExP { get => _IsExP; set => OnPropertyChanged(ref _IsExP, value); }
        public bool IsClickSpeed { get => _IsClickSpeed; set => OnPropertyChanged(ref _IsClickSpeed, value); }
        public bool IsAutoClick { get => _IsAutoClick; set => OnPropertyChanged(ref _IsAutoClick, value); }
        public string Click { get => _Click; set => OnPropertyChanged(ref _Click, value); }
        public bool IsMouseClick { get => _IsMouseClick; set => OnPropertyChanged(ref _IsMouseClick, value); }
        public bool IsAddCort { get => _IsAddCort; set => OnPropertyChanged(ref _IsAddCort, value); }
        public bool IsShutdown { get => _IsShutdown; set => OnPropertyChanged(ref _IsShutdown, value); }
        public bool IsAutoBuff { get => _IsAutoBuff; set => OnPropertyChanged(ref _IsAutoBuff, value); }
        public int AutoInput { get => _Autoinput; set => OnPropertyChanged(ref _Autoinput, value); }
        public int HpInput { get => _HpInput; set => OnPropertyChanged(ref _HpInput, value); }
        public int HpShutDown { get => _HpShutDown; set => OnPropertyChanged(ref _HpShutDown, value); }
        public int MpInput { get => _MpInput; set => OnPropertyChanged(ref _MpInput, value); }
        public int ExpInput { get => _ExpInput; set => OnPropertyChanged(ref _ExpInput, value); }
        public int LagInput { get => _LagInput; set => OnPropertyChanged(ref _LagInput, value); }
        public int XR { get => _XR; set => OnPropertyChanged(ref _XR, value); }
        public int YR { get => _YR; set => OnPropertyChanged(ref _YR, value); }
        public int[] STimeSkill { get => _STimeSkill; set => OnPropertyChanged(ref _STimeSkill, value); }
        public int[] LoadSkill { get => _LoadSkill; set => OnPropertyChanged(ref _LoadSkill, value); }
        public bool IsAttack2 { get => _IsAttack2; set => OnPropertyChanged(ref _IsAttack2, value); }
        public bool[] IsBuff { get => _IsBuff; set => OnPropertyChanged(ref _IsBuff, value); }
        public Thread TAddcort { get => _tAddcort; set => OnPropertyChanged(ref _tAddcort, value); }
        public int Tene { get => _Tene; set => OnPropertyChanged(ref _Tene, value); }
        public int IDCort { get => _IDCort; set => OnPropertyChanged(ref _IDCort, value); }
        public string Status { get => _Status; set => OnPropertyChanged(ref _Status, value); }
        public int IDSKIll { get => _IDSKIll; set => OnPropertyChanged(ref _IDSKIll, value); }
        public int TimeSkill { get => _TimeSkill; set => OnPropertyChanged(ref _TimeSkill, value); }
        public Thread SetSkill2 { get => _SetSkill2; set => OnPropertyChanged(ref _SetSkill2, value); }

        #endregion Read

        #endregion Public data
    }
}