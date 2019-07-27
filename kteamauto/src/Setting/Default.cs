namespace Setting
{
    public class Default
    {
        public static string ProcessesByName = "Khan_Project";

        // show and not show form

        public static string GroupTrainShow = "Visible";   //Collapsed = hide  , Visible = show
        public static string AutoTrainShow = "Visible";    //Collapsed = hide  , Visible = show
        public static string KillToDieShow = "Visible";    //Collapsed = hide  , Visible = show
        public static string AutoBuffShow = "Visible";     //Collapsed = hide  , Visible = show
        public static string MoveShow = "Visible";         //Collapsed = hide  , Visible = show
        public static string ShutdownShow = "Visible";     //Collapsed = hide  , Visible = show
        public static string BlockCordShow = "Visible";    //Collapsed = hide  , Visible = show
        public static string Attack2Show = "Visible";      //Collapsed = hide  , Visible = show

        // Main setting
        public static bool IsCheckOpen = false;

        public static bool IsAttackV = true;

        public static bool IsAuto = false; // Bật auto
        public static bool IsHp = true;
        public static bool IsMp = true;
        public static bool IsExP = false;
        public static bool IsCut = true;
        public static bool IsClickSpeed = false;
        public static bool IsAutoClick = false;
        public static bool IsMouseClick = false;

        public static bool IsAttack = false;
        public static bool IsAttack2 = false;
        public static bool IsBuff_1 = false;
        public static bool IsBuff_2 = false;
        public static bool IsBuff_3 = false;
        public static bool IsBuff_4 = false;

        // Main Auto

        public static bool IsAutoTrain = false;
        public static bool IsKillToDie = false;
        public static bool IsMpNot = false;
        public static bool IsMoveTrain = true;
        public static bool IsAddCort = false;
        public static bool IsAutoBuff = false;
        public static bool IsMove = false;
        public static bool IsBlockCord = false;
        public static bool IsShutdown = false;
        // Input main

        public static int IDSKIll = 12;
        public static int TimeSkill = 25;

        public static int Autoinput = 320;
        public static int HpInput = 60;
        public static int HpShutDown = 10;
        public static int MpInput = 20;
        public static int ExpInput = 90;
        public static int CutInput = 15;

        public static double ReadRangeinput = 30;
        public static double RadiusInput = 7;
        public static int LagInput = 3;

        #region public static Skill

        // id and skill

        public static int TimeSkill_1 = 300;
        public static int TimeSkill_2 = 300;
        public static int TimeSkill_3 = 300;
        public static int TimeSkill_4 = 300;

        public static int LoadSkill_1 = 3;
        public static int LoadSkill_2 = 3;
        public static int LoadSkill_3 = 3;
        public static int LoadSkill_4 = 3;

        public static int IComboxName = 0;
        public static int ISkillAttack = 0;
        public static int ISkillBuff_1 = 0;
        public static int ISkillBuff_2 = 0;
        public static int ISkillBuff_3 = 0;
        public static int ISkillBuff_4 = 0;

        #endregion public static Skill

        #region public static Hotkey

        public static int VStartAuto = 0;
        public static int VStopAuto = 0;
        public static int VStartTrain = 0;
        public static int VStopTrain = 0;
        public static int VStartBuff = 0;
        public static int VOnOffAutoClick = 0;
        public static int VClickLeft = 2;
        public static int VClickRight = 2;

        public static int VaStartAuto = 36;
        public static int VaStopAuto = 37;
        public static int VaStartTrain = 33;
        public static int VaStopTrain = 32;
        public static int VaStartBuff = 34;
        public static int VaOnOffAutoClick = 26;
        public static int VaClickLeft = 26;
        public static int VaClickRight = 27;

        // hotkeyset

        #endregion public static Hotkey
    }
}