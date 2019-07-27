using System;
using System.Threading;

namespace Memory
{
    public class Reading : Memory
    {
        public string KeyStart = "";
        public string KeyEnd = "2ADFE278C0C6D466237A";

        private readonly int BaseChat = 0x01860A00;
        private readonly int BaseAuto = 0x007198BC;
        private readonly int BaseChar = 0x01850614;
        private readonly int BaseCharOnly = 0x00718DB4;
        private readonly int BaseReadOnly = 0x007151C8;
        public readonly int BaseMob = 0x00719D28;
        public bool Login = false;
        public int X = 0, Y = 0;
        private readonly int[] offsetName = new int[] { 0x8 };
        private readonly int[] offsetHpCur = new int[] { 0x120 };
        private readonly int[] offsetHpMax = new int[] { 0x124 };
        private readonly int[] offsetMpCur = new int[] { 0x118 };
        private readonly int[] offsetMpMax = new int[] { 0x11C };
        private readonly int[] offsetExpCur = new int[] { 0x128 };
        private readonly int[] offsetExpMax = new int[] { 0x12C };
        private readonly int[] offsetLevel = new int[] { 0x6 };
        private readonly int[] offsetState = new int[] { 0x1CC };
        private readonly int[] offsetX = new int[] { 0x6F8 };
        private readonly int[] offsetY = new int[] { 0x6FA };
        private readonly int[] offCharState = new int[] { 0xF4 };
        private readonly int[] offsetXMob = new int[] { 0x90 };
        private readonly int[] offsetYMob = new int[] { 0x92 };
        private readonly int[] offsetHpMob = new int[] { 0xED };
        private readonly int[] offDieLooted = new int[] { 0x6d4 };

        private readonly int[] offheckBuff = new int[] { 0x74C };
        private readonly int[] offsetCutSkill = new int[] { 0x70E };
        private readonly int[] BASE_IDSKILL = new int[] { 0X74A };
        private readonly int[] BASE_ATack = new int[] { 0X77A };
        private readonly int[] BASE_SKILL = new int[] { 0X74C };
        private readonly int[] BASE_NOT = new int[] { 0x71E };
        private readonly int[] BASE_For = new int[] { 0x77E };
        private readonly int[] BASE_The = new int[] { 0x71F };
        private readonly int[] BASE_public = new int[] { 0x708 };

        private readonly int[] Anti_Blind = new int[] { 0x7F8 };

        public int ReadMob()
        {
            return ReadShort(0x0185D142);
        }

        public int ReadMobId()
        {
            return ReadShort(0x0185D140);
        }

        public double ReadBlockRadius()
        {
            return Math.Sqrt((ReadX() - X) * (ReadX() - X) + (ReadY() - Y) * (ReadY() - Y));
        }

        public double ReadBlockRadius(int X, int Y)
        {
            return Math.Sqrt((ReadX() - X) * (ReadX() - X) + (ReadY() - Y) * (ReadY() - Y));
        }

        public double ReadBlockRadius(int[] Cort = null)
        {
            if (Cort == null) { return 0; }
            return Math.Sqrt((ReadX() - Cort[0]) * (ReadX() - Cort[0]) + (ReadY() - Cort[1]) * (ReadY() - Cort[1]));
        }

        public int Intermediate(int ID)
        {
            if (ID == -1) { return 0; }
            try
            {
                int noob = ID * 0x3A0 + ReadInt(0x00719D48);
                Write(0x00719D28, noob);
                return noob;
            }
            catch { return 0; }
        }

        public bool FIsNear(int X, int Y, double Area = 0)
        {
            if (ReadBlockRadius(X, Y) <= Area)
            {
                return true;
            }
            return false;
        }

        public string ReadNames(int ID)
        {
            int Read = 1320 * ID + ReadInt(BaseAuto, offsetName);
            return ReadName(Read, 20);
        }

        public double ReadRadius()
        {
            return Math.Sqrt((ReadMobX() - ReadX()) * (ReadMobX() - ReadX()) + (ReadMobY() - ReadY()) * (ReadMobY() - ReadY()));
        }

        public int ReadMobX()
        {
            return ReadShort(BaseMob, offsetXMob);
        }

        public int ReadMobY()
        {
            return ReadShort(BaseMob, offsetYMob);
        }

        public int ReadMobHp()
        {
            return ReadShort(BaseMob, offsetHpMob);
        }

        public int ReadMobState()
        {
            return ReadShort(ReadInt(BaseMob));
        }

        public void SetStateDie()
        {
            if (!Login || KeyStart != KeyEnd) { return; }
            Write(BaseAuto, offDieLooted, (byte)128);
            Write(BaseAuto, offsetCutSkill, (short)1000);
        }

        public void SetCutSkill(int Time = 25)
        {
            if (!Login || KeyStart != KeyEnd) { return; }
            Write(BaseAuto, offsetCutSkill, (short)1000);
            Thread.Sleep(Time);
        }

        public void SetCutSkill(short ID = 1000)
        {
            if (!Login || KeyStart != KeyEnd) { return; }
            Write(BaseAuto, offsetCutSkill, (short)ID);
        }

        //public void SetCutSkill(bool Now = true)
        //{
        //    if (!Login || KeyStart != KeyEnd) { return; }
        //    Write(BaseAuto, BASE_NOT, (double)1.12152901605963E-321);
        //    Write(BaseAuto, BASE_The, (short)0);
        //    Write(BaseAuto, BASE_For, (short)1);
        //    Write(BaseAuto, offsetCutSkill, (short)1000);
        //}

        public void SetStatePublic()
        {
            if (!Login || KeyStart != KeyEnd) { return; }
            Write(BaseAuto, BASE_NOT, (double)1.12152901605963E-321);
            //Write(BaseAuto, BASE_public, (long)281474976710656000);
            Write(BaseAuto, BASE_The, (short)0);
            Write(BaseAuto, BASE_For, (short)1);
        }

        public void SetSkill(int ID, bool Ok = true)
        {
            Write(BaseAuto, BASE_SKILL, (short)ID);
        }

        public int ReadKillBuff()
        {
            return ReadShort(BaseAuto, Anti_Blind);
        }

        public void SetText(string Text)
        {
            Write(BaseChat, Text);
        }

        public void SetState()
        {
            Write(BaseAuto, offsetCutSkill, (short)1200);
        }

        public int GetStateBuff()
        {
            if (!Login || KeyStart != KeyEnd) { return 0; }

            return ReadInt(BaseAuto, offheckBuff);
        }

        public int ReadState()
        {
            return ReadShort(BaseChar, offsetState);
        }

        public int ReadX()
        {
            return ReadShort(BaseAuto, offsetX);
        }

        public int ReadY()
        {
            return ReadShort(BaseAuto, offsetY);
        }

        public int ReadCharState()
        {
            return ReadShort(BaseChar, offCharState);
        }

        public int ReadCutSkill()
        {
            return ReadShort(BaseAuto, offsetCutSkill);
        }

        public string ReadChar()
        {
            return ReadName(BaseAuto, offsetName, 20);
        }

        public string ReadCharRead()
        {
            return ReadName(BaseCharOnly, 20);
        }

        public bool ReadLoading()
        {
            return Convert.ToBoolean(ReadIntOnly(BaseReadOnly));
        }

        public int ReadLevel()
        {
            return ReadShort(BaseAuto, offsetLevel);
        }

        public int GetIdSkill()
        {
            return ReadShort(BaseAuto, BASE_IDSKILL);
        }

        public double ReadHp()
        {
            double hpCur = ReadInt(BaseAuto, offsetHpCur);
            double hpMax = ReadInt(BaseAuto, offsetHpMax);
            return Math.Round((hpCur / hpMax) * 100);
        }

        public double ReadMp()
        {
            double mpCur = ReadInt(BaseAuto, offsetMpCur);
            double mpMax = ReadInt(BaseAuto, offsetMpMax);
            return Math.Round((mpCur / mpMax) * 100);
        }

        public double ReadMp(bool Maths = false)
        {
            if (Maths)
            {
                double mpCur = ReadInt(BaseAuto, offsetMpCur);
                double mpMax = ReadInt(BaseAuto, offsetMpMax);
                return Math.Round((mpCur / mpMax) * 100);
            }
            return ReadInt(BaseAuto, offsetMpCur);
        }

        public double ReadExp()
        {
            double expCur = ReadInt(BaseAuto, offsetExpCur);
            double expMax = ReadInt(BaseAuto, offsetExpMax);
            return Math.Round((expCur / expMax) * 100);
        }
    }
}