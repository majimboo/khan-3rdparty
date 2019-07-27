using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Setting
{
    public class KeyString
    {
        public static string[] Main = new string[] {
            "Main", "Speed", "IsHp", "Hp", "IsMp",
            "Mp", "IsExp", "Exp", "IsCut", "Cut",
            "IsFast", "AutoClick", "Mouse" };

        public static string[] Train = new string[] {
            "Train", "Kill", "Buff", "IsHpK",
            "HpK", "Range", "RangeT", "Lag" };

        public static string[] Skill = new string[] {
            "Skill","Class", "IsAttack", "IAttack",
            "IsBuff1", "IBuff1", "Time1", "Load1",
            "IsBuff2", "IBuff2", "Time2", "Load2",
            "IsBuff3", "IBuff3", "Time3", "Load3",
            "IsBuff4", "IBuff4", "Time4", "Load4" };

        public static string[] NameAdd = new string[] { "Hotkey", "ListClick" };

        public static string[,] Hotkeys = new string[,] {
            { "C1", "I1" },
            { "C2", "I2" },
            { "C3", "I3" },
            { "C4", "I4" },
            { "C5", "I5" },
            { "C6", "I6" },
            { "C7", "I7" },
            { "C8", "I8" } };
    }

    public static class ReadXML
    {
        private static XmlDocument doc = new XmlDocument();
        private static readonly string Path = "output.config";

        public static void Create(string TextNew = "")
        {
            doc.LoadXml(string.Format(
                "<book genre='novel' ISBN='1-861001-57-5'>" +
                "<name>{0}</name>" +
                "</book>", TextNew));

            doc.Save(Path);
        }

        public static string FilePath()
        {
            return Path;
        }

        public static void SetNew(string TextNew = "")
        {
            try
            {
                doc.Load(Path);
                doc.Save(Console.Out);
                var product = doc.SelectSingleNode("descendant::book[@genre='novel']/name");
                product.InnerText = TextNew;
                doc.Save(Console.Out);
                doc.Save(Path);
            }
            catch
            {
                Create(TextNew);
            }
        }

        public static string Read()
        {
            try
            {
                doc.Load(Path);
                return doc.InnerText;
            }
            catch
            {
                return "";
            }
        }
    }
}