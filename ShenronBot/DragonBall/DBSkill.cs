using System;
using System.Collections.Generic;
using System.Text;

namespace ShenronBot
{
    class DBSkill
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int EnergyCost { get; set; }
        public int Charge { get; set; } = 0;
        public bool Special { get; set; } = false;
        public bool Human { get; set; } = false;
        public bool Namekian { get; set; } = false;
        public bool Saiyan { get; set; } = false;
    }
}
