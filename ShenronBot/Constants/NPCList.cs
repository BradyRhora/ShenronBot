using System;
using System.Collections.Generic;
using System.Text;

namespace ShenronBot
{
    class NPCList
    {
        public static DBNPC[] NPCs = {
        new DBNPC
        {
            Name = "Master Roshi",
            Race = "Human",
            Power_Level = 1000,
            Location = Constants.Channels.KAME_HOUSE,
            Dialogue = "I don't just take on anyone as a student you know! But.. You look like you have potential.",
            Response1 = "Alright! Let's get started then.",
            Response2 = "Hm. Fine, maybe another time.",
            Skill = DBFuncs.GetSkill("Kamehameha")
        },

        new DBNPC
        {
            Name = "Goku",
            Race = "Saiyan",
            Power_Level = 100000,
            Location = Constants.Channels.GOKUS_HOUSE,
            Dialogue = "You want to fight me?? Wow! I won't go easy on you!",
            Response1 = "Aw yeah! Let's do this!",
            Response2 = "Aw.. Maybe later?"
        },

        new DBNPC
        {
            Name = "Krillin",
            Race = "Human",
            Power_Level = 10000,
            Location = Constants.Channels.KAME_HOUSE,
            Dialogue = "I might not look like much compared to the others, but I can pack just as much of a punch!",
            Response1 = "Okay, come at me!",
            Response2 = "What? C'mon I can take you!",
            Skill = DBFuncs.GetSkill("Destructo Disk")
        },

        new DBNPC
        {
            Name = "Piccolo",
            Race = "Namekian",
            Power_Level = 50000,
            Location = Constants.Channels.WASTELAND,
            Dialogue = "Hmph. I guess I can waste a few minutes to entertain you.",
            Response1 = "Let's go then.",
            Response2 = "Whatever.",
            Skill = DBFuncs.GetSkill("Special Beam Cannon")
        },

        new DBNPC
        {
            Name = "Vegeta",
            Race = "Saiyan",
            Power_Level = 99999,
            Location = Constants.Channels.CAPSULE_CORP,
            Dialogue = "Ha! Fool, you really thing that I, the Saiyan Prince, would waste my time with you? Pathetic.",
            Response1 = "Wait.. You're serious? Ha! Fine then, fight me!",
            Response2 = "That's right, get out.",
            Skill = DBFuncs.GetSkill("Galick Gun")
        },

        new DBNPC
        {
            Name = "Frieza",
            Power_Level = 60000,
            Location = Constants.Channels.FRIEZAS_SHIP,
            Dialogue = "Mwahahah... Who might you be? Another [RACE] in line for death?",
            Response1 = "You dare challenge me?!",
            Response2 = "Get out of my sight then!"
        },

        new DBNPC
        {
            Name = "Namek Villager",
            Race = "Namekian",
            Power_Level = 500,
            Location = Constants.Channels.MOORIS_VILLAGE,
            Dialogue = "What? You want me to train you? I suppose I could try..",
            Response1 = "Alright, I'm ready.",
            Response2 = "Okay, be safe."
        },
        new DBNPC
        {
            Name = "Gohan",
            Race = "Saiyan",
            Power_Level = 75000,
            Location = Constants.Channels.GOKUS_HOUSE,
            Dialogue = "Oh, you want to fight? Sure, that sounds fun!",
            Response1 = "Okay, let's do this!",
            Response2 = "Alright, maybe another time!",
            Skill = DBFuncs.GetSkill("Masenko")
        }
    };


        
    }
}
