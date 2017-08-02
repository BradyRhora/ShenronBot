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

            public static ulong[] PLANETS = { 341819643033550848, 341819718694600708 , 341819801603276801 };
        }

        public class Users
        {
            public static ulong BRADY = 108312797162541056;
            public static ulong ZAIM = 142068521193439232;
        }

        public class Roles
        {
            //Earth, Namek, Vegeta.
            public static ulong[] SAIYAN = { 342060192944619520, 342066927268528145, 342066927935422464 };
            public static ulong[] HUMAN = { 342060189387587594, 342066928556310528, 342066928858300429 };
            public static ulong[] NAMEKIAN = { 342060191518425088, 342066929756012554, 342066930963709952 };

            public static ulong ON_EARTH = 342063065824755712;
            public static ulong ON_NAMEK = 342175783688732672;
            public static ulong ON_VEGETA = 342177434830897153;

            public static ulong ADMIN = 000000000000000000;
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

        public class Channels
        {
            public static ulong VEGETA_INFO = 342068857147621386;
            public static ulong VEGETA_GEN = 341819801603276801;

            public static ulong EARTH_INFO = 342067064359354368;
            public static ulong EARTH_GEN = 341819643033550848;

            public static ulong NAMEK_INFO = 342068642470428673;
            public static ulong NAMEK_GEN = 341819718694600708;

            public static ulong[] BLOCKED_CHANNELS = { VEGETA_INFO, EARTH_INFO, NAMEK_INFO, VEGETA_GEN, EARTH_GEN, NAMEK_GEN };
        }
    }
}
