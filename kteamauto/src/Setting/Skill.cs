namespace Setting
{
    public class Skill
    {
        public static string[] ClassName => new string[] { "Assassasin", "Knight", "Sorcerer", "Necromancer", "Micko", "Cleric" };

        private static string[,] Assassin(bool Attack = false)
        {
            if (Attack)
            {
                return new string[,]
                         {
                            {"N/A", "0" },
                            {"Gouge" , "4"},
                            {"Numbing Stab", "5"},
                            {"Thief", "10"},
                            {"Galvano Strike", "12"},
                            {"Assassination", "17"},
                            {"Shade Shatter", "21" },
                            { "Backstab", "24"}
                         };
            }
            else
            {
                return new string[,]
                        {
                            { "N/A" , "0", "0"},
                            { "Focus" , "6", "152"},
                            {"Swiftment", "15","62" }
                        };
            }
        }

        private static string[,] Knight(bool Attack = false)
        {
            if (Attack)
            {
                return new string[,]
                    {
                        {"N/A" ,"0" },
                        {"Stab" , "4"},
                        {"Upper Blow" , "5"},
                        {"Lower Blow" , "7"},
                        {"Whirlwind" , "9"},
                        {"Meteor Slash" , "13"},
                        {"Thunder Spike Slash", "15" },
                        {"Bone Spirit Slash", "19" },
                        {"Clashal Spike Slash", "21" },
                        {"Hell Die’s slash", "23" },
                        {"Voltaic Storm Slash" , "24"},
                        {"Soul Bane Slash", "25" }
                    };
            }
            else
            {
                return new string[,]
                    {
                        { "N/A",  "0",  "0"},
                        {"Enrage", "6", "244"},
                        {"Light Armor", "8", "244"},
                        {"Berserk", "12", "244"},
                        {"Heavy Armor", "16", "244"},
                        {"Speed of vitality", "18", "122"},
                        {"Battle Shout", "20", "63"}
                    };
            }
        }

        private static string[,] Sorcerer(bool Attack)
        {
            if (Attack)
            {
                return new string[,]
                {
                    { "N/A", "0"},
                    { "Ice Bolt" , "1"},
                    {"Flame Strike" , "2"},
                    {"Deadly Shock", "3" },
                    {"Explosion"  , "4"},
                    {"Ice Lenses", "6" },
                    {"Meteor" , "7"},
                    {"Lightning", "8" },
                    {"Clashal Spike", "10" },
                    {"Thunder Fall", "11" },
                    {"Electric Jail" , "14"},
                    {"Hell Flash" , "15"},
                    {"Frozen Flower", "17" },
                    {"Thunder Twister", "19" },
                    {"Mana Tab", "20" },
                    {"Zero Divide", "21" },
                    {"Crimson Hellfire", "23" },
                    {"Sudden Frozen", "24" },
                    {"Sun Flame", "25" }
                };
            }
            else
            {
                return new string[,]
                {
                    { "N/A", "0", "0"},
                    {"Concentration", "5", "248"},
                    {"Wisdom", "9", "312"}
                };
            }
        }

        private static string[,] Necromancer(bool Attack)
        {
            if (Attack)
            {
                return new string[,]
                {
                    { "N/A" , "0"},
                    { "Fire Bolt" , "1"},
                    {"Fire Bal", "2"},
                    {"Lightning Bolt", "4"},
                    {"Call Nilba", "6"},
                    {"Meteor", "8"},
                    {"Lightning", "9"},
                    {"Bone Spear", "11"},
                    {"Thunder Spark", "13"},
                    {"Bone Spirit", "15"},
                    {"Abyss", "17"},
                    {"Black Hole", "19"},
                    {"Mana Tab", "21"},
                    {"Burning Subversion","24"},
                    {"nightmare's Infusion","25"}
                };
            }
            else
            {
                return new string[,]
                {
                    {"N/A", "0", "0"},
                    {"Lightning Shield", "3", "248"},
                    {"Thunder Blade Shield", "10", "248"},
                    {"Bone Armor", "16", "248"},
                    {"Curse Bone Armor", "22", "248"}
                };
            }
        }

        private static string[,] Micko(bool Attack)
        {
            if (Attack)
            {
                return new string[,]
                {
                    {"N/A","0"},
                    {"Befis Bring", "3"},
                    {"Dill Brand", "4"},
                    {"Fire Arrow", "6"},
                    {"Blade Hout", "8"},
                    {"Freezing Arrow", "10"},
                    {"Cold Arrow", "12"},
                    {"Bone Spirit Arrow", "14"},
                    {"Guided Arrow", "16"},
                    {"Minuet Arrow", "18"},
                    {"Black Rose Arrow", "20" },
                    {"Life Tap", "23"},
                    {"Soul Arrow", "25"}
                };
            }
            else
            {
                return new string[,]
                {
                    {"N/A", "0", "0"},
                    {"Knight of God", "7", "242"},
                    {"Blessing", "13", "242"},
                    {"Regeneration", "19", "182"}
                };
            }
        }

        private static string[,] Cleric(bool Attack)
        {
            if (Attack)
            {
                return new string[,]
                {
                    {"N/A", "0"},
                    {"Punishment", "2"},
                    {"Greater Punishment", "9"},
                    {"The Wrath of God", "13"}
                };
            }
            else
            {
                return new string[,]
                {
                    {"N/A", "0", "0"},
                    {"Strength of Heaven", "4", "248"},
                    {"Blood Leech", "17", "188"},
                    {"Blood Leech 125", "25", "188"}
                };
            }
        }

        public static string[,] Name_Skill(string NameChar, bool Attack = false)
        {
            switch (NameChar)
            {
                case "Assassasin":
                    return Assassin(Attack);

                case "Knight":
                    return Knight(Attack);

                case "Sorcerer":
                    return Sorcerer(Attack);

                case "Necromancer":
                    return Necromancer(Attack);

                case "Micko":
                    return Micko(Attack);

                case "Cleric":
                    return Cleric(Attack);

                default:
                    return new string[,]
                    {
                        {"N/A", "0", "0"}
                    };
            }
        }

        //return new string[,]
        //         {
        //            {"N/A", "0" },
        //            {"Gouge" , "4"},
        //            {"Numbing Stab", "5"},
        //            {"Swift Sting" , "7"},
        //            {"Rapid Torment", "11"},
        //            {"Galvano Strike", "12"},
        //            {"Imbue Spear", "14"},
        //            {"Assassination", "17"},
        //            {"Stinging Rose", "20"},
        //            {"Shade Shatter", "21" },
        //            { "Backstab", "24"},
        //            {"Javelination", "25" }
        //         };
    }
}