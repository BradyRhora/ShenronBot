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

            public static string EARTH_LINK = "https://discord.gg/UwDKgAF";
            public static string NAMEK_LINK = "https://discord.gg/tJVfgms";
            public static string VEGETA_LINK = "https://discord.gg/vFaS8u2";
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

            public static ulong[] KAIOKEN = { 342516608448856065, 342516610726363136, 342516612873846794 };
            public static ulong[] SUPER = { 342516606448041985, 342516609446969354, 342516611284205591 };
            public static ulong[] SSGSS = { 342516607794544640, 342516610080440321, 342516611879534596 };

            public static ulong ON_EARTH = 342063065824755712;
            public static ulong ON_NAMEK = 342175783688732672;
            public static ulong ON_VEGETA = 342177434830897153;

            public static ulong ADMIN = 000000000000000000;
        }

        public class Colours
        {
            public static Color SHENRON_GREEN = new Color(89, 171, 71);
            public static Color KAIOKEN = new Color(221, 0, 42);
            public static Color SS_YELLOW = new Color(238, 218, 58);
            public static Color SSG_RED = new Color(3);
            public static Color SSGSS_BLUE = new Color(42, 205, 238);
            public static Color NAMEKIAN_GREEN = new Color(35, 143, 23); 
            public static Color DEFAULT_COLOUR = new Color(0);
        }

        public class Channels
        {
            public static ulong VEGETA_INFO = 342068857147621386;
            public static ulong VEGETA_GEN = 342343936469106689;

            public static ulong EARTH_INFO = 342181627595587585;
            public static ulong EARTH_GEN = 342181267778830339;
            public static ulong COMMANDS = 342177164852199425;

            public static ulong NAMEK_INFO = 342068642470428673;
            public static ulong NAMEK_GEN = 342344230011666434;

            public static ulong[] BLOCKED_CHANNELS = { VEGETA_INFO, EARTH_INFO, NAMEK_INFO, VEGETA_GEN, EARTH_GEN, NAMEK_GEN, COMMANDS};
            
            public static ulong KAME_HOUSE = 342163249703288844;
            public static ulong CAPSULE_CORP = 342163338169548801;
            public static ulong WEST_CITY = 341819643033550848;
            public static ulong GOKUS_HOUSE = 343063372171837441;
            public static ulong WASTELAND = 343063411145441280;

            public static ulong GRAND_ELDERS_HOUSE = 342163696090611713;
            public static ulong MOORIS_VILLAGE = 342191959517036554;
            public static ulong NAMEK_FIELD = 341819718694600708;
            public static ulong FRIEZAS_SHIP = 342164634041712642;
        }

        public class Images
        {
            public static string Roshi = @"DragonBall\Images\Roshi.jpg";
            public static string Goku = @"DragonBall\Images\Goku.jpg";
            public static string Vegeta = @"DragonBall\Images\Vegeta.jpg";
            public static string Krillin = @"DragonBall\Images\Krillin.jpg";
            public static string Piccolo = @"DragonBall\Images\Piccolo.jpg";
            public static string Namekian = @"DragonBall\Images\Namekian.jpg";
        }
    }
}
