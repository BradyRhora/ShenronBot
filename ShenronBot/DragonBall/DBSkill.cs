using System;
using System.Collections.Generic;
using System.Text;

namespace ShenronBot
{
    public class DBSkill
    {
        public string Name { get; set; }
        public int Power { get; set; } = 0;
        public int EnergyCost { get; set; } = 0;
        public int Charge { get; set; } = 0;
        public bool Special { get; set; } = false;
        public bool Human { get; set; } = false;
        public bool Namekian { get; set; } = false;
        public bool Saiyan { get; set; } = false;
        public ulong[] Role { get; set; }

        public static bool operator ==(DBSkill skill1, DBSkill skill2)
        {
            if (skill1.Name == skill2.Name) return true;
            else return false;
        }

        public static bool operator !=(DBSkill skill1, DBSkill skill2)
        {
            if (skill1.Name == skill2.Name) return false;
            else return true;
        }

        public override bool Equals(object o) //dont use lol
        {
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

    }
}
