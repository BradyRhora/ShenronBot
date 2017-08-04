using System;
using System.Collections.Generic;
using System.Text;

namespace ShenronBot
{
    public class DBNPC
    {
        public string Name { get; set; }
        public string Race { get; set; } = "?";
        public int Power_Level { get; set; }
        public ulong Location { get; set; }
        public string Dialogue { get; set; }
        public string Response1 { get; set; }
        public string Response2 { get; set; }
    }
}
