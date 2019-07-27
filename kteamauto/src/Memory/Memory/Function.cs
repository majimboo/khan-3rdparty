using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Memory
{
    public class Mob
    {
        public int Id { get; set; } = 0;
        public double Radius { get; set; } = 0;
        public int Hp { get; set; } = 0;
    }

    public class BlackList
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class ListCord
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Function : Assembly
    {
        private static readonly Random Random = new Random();
        public int nowMob = 0, ID_SELECT = -1, HPMob = 0, XY = 1, MouseButton = 0;
        public bool ONTrain = true, Kill = false;
        public bool IsBlockCord = false;
        public List<Mob> listMob = new List<Mob>();
        public List<BlackList> BlackList = new List<BlackList>();
        public List<Mob> listIngoreMob = new List<Mob>();
        private readonly int[] MoveLagg = new int[2];
        public int Min_limit_crep = 0, Max_limit_crep = 0;
        public int TimeAttack = 0, Blind = 0, IdSkill = 0;
        public bool CutSkill = true, SleepBuff = false, IsMoveT = false;
        public double RadiusScan = 7, ReadRange = 30, Radius = 0;

        #region Set hook - memory

        public int iMsg = 0, iMsgMem = 0;

        //public IntPtr Memory = IntPtr.Zero;

        public bool CreateMemory()
        {
            //Memory = AllocEx(hReadProcess, 300);
            return true;
        }

        public bool SetHook()
        {
            int result = MAPI.SetHook(mHandle);
            if (result == 0) { return false; }
            else { return true; }
        }

        public bool SetMsg()
        {
            iMsg = MAPI.GetMSG();
            iMsgMem = MAPI.GetMemMSG();
            if (iMsg == 0) { return false; }
            else { return true; }
        }

        public bool SetFunction(int id)
        {
            OpenHandle(id);
            //CreateMemory();
            AllocMemory(200);
            AllocMemoryAll(200);
            Thread StartCreate = new Thread(delegate () { CreateMemoryWhile(); })
            {
                IsBackground = true
            };
            StartCreate.Start();
            return true;
        }

        private void CreateMemoryWhile()
        {
            try
            {
                while (Login)
                {
                    Thread.Sleep(200000);
                    AllocMemoryAll(200);
                }
            }
            catch { }
        }

        #endregion Set hook - memory

        #region Train

        public void SetCord()
        {
            X = ReadX();
            Y = ReadY();
        }

        public void ReturnCord(double radius, bool Get = false)
        {
            if (ReadBlockRadius() >= radius)
            {
                while (ReadBlockRadius() >= radius)
                {
                    Move(X, Y, 400);
                }
            }
        }

        public void ReturnCord()
        {
            int LX = 0, LY = 0;
            int[] Cort = new int[] { X, Y };
            if (ReadBlockRadius() >= ReadRange)
            {
                while ((ReadBlockRadius() >= ReadRange) && IsBlockCord && ONTrain)
                {
                    MoveTrain(Cort);
                    Thread.Sleep(300);
                    if (LX == ReadX() && LY == ReadY())
                    {
                        break;
                    }
                    LX = ReadX();
                    LY = ReadY();
                }
            }
        }

        public void ReturnCord(double radius, int[] Cort = null)
        {
            if (Cort == null)
            {
                return;
            }
            int LX = 0, LY = 0;
            if (ReadBlockRadius(Cort) >= radius)
            {
                while (ReadBlockRadius(Cort) >= radius)
                {
                    if (LX == ReadX() && LY == ReadY())
                    {
                        break;
                    }
                    Move(Cort[0], Cort[1]);
                    Thread.Sleep(500);
                    LX = ReadX();
                    LY = ReadY();
                }
            }
        }

        public void MoveTrain(int[] Cort)
        {
            if (Cort == null)
            {
                MoveBuff();
            }
            int X = 0, Y = 0;
            int RX = ReadX(), RY = ReadY();
            if (Cort[0] < RX) { X = -2; }
            else if (Cort[0] > RX) { X = 2; }

            if (Cort[1] < RY) { Y = -2; }
            else if (Cort[1] > RY) { Y = 2; }
            Move(RX + X, RY + Y);
        }

        public void MoveBuff(int Time = 210)
        {
            if (XY == 1) { XY = -1; } else { XY = 1; }
            Move(ReadX() + XY, ReadY() + XY, Time);
        }

        public void SELECT_MOB()
        {
            double curRadius;
            int ID = 0;
            ID_SELECT = -1;
            int IdNumber = BlackList.Count;
            for (int i = 0; i < 100; i++)
            {
                ID = Intermediate(i);

                if (ReadMobState() == 128 && i != nowMob && ReadMobHp() > 0 && ReturnMob(ID))
                {
                    curRadius = ReadRadius();
                    if (curRadius < Radius)
                    {
                        Radius = curRadius;
                        ID_SELECT = i;
                        HPMob = ReadMobHp();
                    }
                }
            }
        }

        public void AttackTH(int timeout = 5)
        {
            try
            {
                if (Min_limit_crep >= Max_limit_crep || Load_Blind())
                {
                    ID_SELECT = -1;
                    ONTrain = false;
                    return;
                }
                Radius = RadiusScan;
                SELECT_MOB();
                nowMob = ID_SELECT;
                int IdBackList = Intermediate(nowMob);
                TimeSpan timeOut = TimeSpan.FromSeconds(timeout);
                Stopwatch sw = Stopwatch.StartNew();
                SetSkill(IdSkill, 20);
                while (ReadMobState() == 128 && ONTrain)
                {
                    if (!SleepBuff)
                    {
                        if (ReadX() != MoveLagg[0] || ReadY() != MoveLagg[1])
                        {
                            MoveLagg[0] = ReadX();
                            MoveLagg[1] = ReadY();
                            sw = Stopwatch.StartNew();
                            Thread.Sleep(25);
                        }

                        Attack(MouseButton);
                        if (Load_Blind()) { ONTrain = false; }
                        else if (sw.Elapsed >= timeOut && ReadMobHp() == HPMob)
                        {
                            BlackList.Add(new BlackList() { Id = IdBackList, X = ReadX(), Y = ReadY() });
                            Min_limit_crep++; break;
                        }
                        else if (ReadMobState() != 128 || ReadMobHp() <= 0) { Min_limit_crep++; break; }
                        else if (!Kill && ReadMobHp() == 256) { Min_limit_crep++; break; }
                    }
                    else
                    {
                        Thread.Sleep(300);
                        sw = Stopwatch.StartNew();
                    }
                }
            }
            catch { }
        }

        private bool ReturnMob(int ID)
        {
            try
            {
                int IdNumber = BlackList.Count;
                if (IdNumber > 0)
                {
                    foreach (BlackList aPart in BlackList)
                    {
                        if (aPart.Id == ID)
                        {
                            return false;
                        }
                    }
                    if (ReadBlockRadius(BlackList[IdNumber - 1].X, BlackList[IdNumber - 1].Y) >= 8)
                    {
                        BlackList.Clear();
                    }
                    return true;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        #endregion Train

        #region Packet Analyzing

        #region Alloc

        public void SetSkill(int id = -1, int Sleep = 25, string Opcodes = "")
        {
            if (id <= 0 || id == -1) { return; }
            PushAD(ref Opcodes);
            Push(id - 1, ref Opcodes);
            MovEax(0x00513200, ref Opcodes);
            CallEax(ref Opcodes);
            AddEsp(0x4, ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes);
            Thread.Sleep(Sleep + 5);
        }

        public void BuffPT(int Time = 200, string Opcodes = "")
        {
            PushAD(ref Opcodes);
            Push(ReadX(), ref Opcodes);
            Push(ReadY(), ref Opcodes);
            Push(4, ref Opcodes);
            MovEax(0x004D7510, ref Opcodes);
            CallEax(ref Opcodes);
            AddEsp(0xC, ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes);
            Thread.Sleep(Time);
        }

        public void Move(int x, int y, int Time = 80, string Opcodes = "")
        {
            PushAD(ref Opcodes);
            Push(y, ref Opcodes);
            Push(x, ref Opcodes);
            Push(4, ref Opcodes);
            MovEax(0x004D6B40, ref Opcodes);
            CallEax(ref Opcodes);
            AddEsp(0xC, ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes);
            Thread.Sleep(Time);
        }

        public void Pick(string Opcode = "")
        {
            for (int i = 0; i < 60; i++)
            {
                PushAD(ref Opcode);
                Push(ReadInt(0x007077D0), ref Opcode);
                Push(i, ref Opcode);
                Push(3, ref Opcode);
                MovEax(0x004D6B40, ref Opcode);
                CallEax(ref Opcode);
                AddEsp(0xC, ref Opcode);
                MovEax(1, ref Opcode);
                PopAD(ref Opcode);
                Ret(ref Opcode);
                Injection(Opcode);
                Thread.Sleep(50);
                while (ReadState() != 0)
                {
                    Thread.Sleep(50);
                }
            }
        }

        public void SendKeyQ(string Opcodes = "")
        {
            PushAD(ref Opcodes);
            MovEax(0x00513FE0, ref Opcodes);
            CallEax(ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes);
            Thread.Sleep(25);
        }

        public void SendKeyW(string Opcodes = "")
        {
            PushAD(ref Opcodes);
            MovEax(0x005140F0, ref Opcodes);
            CallEax(ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes);
            Thread.Sleep(25);
        }

        #endregion Alloc

        #region AllocV1

        public void AttackMob(int button = 0, int id = -1, bool Speed = false, string Opcodes = "")
        {
            if (id == -1 || !Login || KeyStart != KeyEnd) { return; }

            int baseButton = 0x004D6b40;
            if (button == 1) { baseButton = 0x004D7510; }
            PushAD(ref Opcodes);
            Push(ReadInt(0x007077D0), ref Opcodes);
            Push(id, ref Opcodes);
            Push(1, ref Opcodes);
            MovEax(baseButton, ref Opcodes);
            CallEax(ref Opcodes);
            AddEsp(0xC, ref Opcodes);
            MovEax(1, ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes, addressAllocV1);
            Thread.Sleep(25 + TimeAttack);
            if (CutSkill && ReadState() == 3) { SetCutSkill(0); SetStatePublic(); }
        }

        public void Attack(int button = 0, string Opcodes = "")
        {
            if (nowMob == -1 || !Login || KeyStart != KeyEnd) { return; }

            int baseButton = 0x004D6b40;
            if (button == 1) { baseButton = 0x004D7510; }
            PushAD(ref Opcodes);
            Push(ReadInt(0x007077D0), ref Opcodes);
            Push(nowMob, ref Opcodes);
            Push(1, ref Opcodes);
            MovEax(baseButton, ref Opcodes);
            CallEax(ref Opcodes);
            AddEsp(0xC, ref Opcodes);
            MovEax(1, ref Opcodes);
            PopAD(ref Opcodes);
            Ret(ref Opcodes);
            Injection(Opcodes, addressAllocV1);
            Thread.Sleep(25 + TimeAttack);
            if (CutSkill && ReadState() == 3) { SetCutSkill(0); SetStatePublic(); }
        }

        #endregion AllocV1

        public void SendText(string Text, string Opcode = "")
        {
            try
            {
                //SendKeys.Send("{ENTER}");
                SetText(Text);
                Thread.Sleep(100);
                PushAD(ref Opcode);
                MovEax(0x00511b90, ref Opcode);
                CallEax(ref Opcode);
                PopAD(ref Opcode);
                Ret(ref Opcode);
                Injection(Opcode);
                Thread.Sleep(25);
            }
            catch { }
        }

        public bool Load_Blind()
        {
            int NowRead = ReadKillBuff(), computations = 0;
            if (NowRead != Blind)
            {
                computations = NowRead - Blind;
                switch (computations)
                {
                    case 256:
                        Blind = NowRead;
                        return true;

                    case -256:

                        Blind = NowRead;
                        return false;

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        private byte[] PacketToBytes(string packet)
        {
            string packetSend = packet;
            byte[] bytes = new byte[(packetSend.Length / 2)];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(packetSend.Substring(0 + 2 * i, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bytes;
        }

        private bool WritePacketMemory(string packet, IntPtr Memory)
        {
            byte[] byteWrite = PacketToBytes(packet);
            return WriteBytesAsm(Memory, byteWrite);
        }

        #endregion Packet Analyzing
    }
}