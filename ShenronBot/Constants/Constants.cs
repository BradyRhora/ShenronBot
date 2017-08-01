using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace ShenronBot
{
    class Constants
    {

        public class Guilds
        {
            public static ulong DBZ_EARTH = 341819643033550848;
            public static ulong DBZ_NAMEK = 341819718694600708;
            public static ulong DBZ_VEGETA = 341819801603276801;
        }

        public class Users
        {
            public static ulong BRADY = 108312797162541056;
            public static ulong ZAIM = 142068521193439232;
        }

        public class Roles
        {
<<<<<<< HEAD
            //Earth, Namek, Vegeta.
            public static ulong[] SAIYAN = { 342060192944619520 };
            public static ulong[] HUMAN = { 342060189387587594 };
            public static ulong[] NAMEKIAN = { 342060191518425088 };

            public static ulong[] ON_EARTH = { 342063065824755712 };
            public static ulong[] ON_NAMEK = { 000000000000000000 };
            public static ulong[] ON_VEGETA = { 000000000000000000 };
=======
            public static ulong SAIYAN = 000000000000000000;
            public static ulong HUMAN = 000000000000000000;
            public static ulong NAMEKIAN = 000000000000000000;

            public static ulong EARTH = 000000000000000000;
            public static ulong NAMEK = 000000000000000000;
            public static ulong VEGETA = 000000000000000000;
>>>>>>> origin/master
        }

        public class Colours
        {
            public static Color SHENRON_GREEN = new Color(1); //Insert some actual colour here \/
            public static Color SS_YELLOW = new Color(2);
            public static Color SSG_RED = new Color(3);
            public static Color SSGSS_BLUE = new Color(3);
            public static Color NAMEKIAN_GREEN = new Color(3); 
            public static Color DEFAULT_COLOUR = new Color(0);
        }
    }
}
