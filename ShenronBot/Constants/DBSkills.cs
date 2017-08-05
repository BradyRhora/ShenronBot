using System;
using System.Collections.Generic;
using System.Text;

namespace ShenronBot
{
    class DBSkills
    {
        public static DBSkill[] Skills = {

        new DBSkill
        {
            Name = "Ki Blast",
            Power = 5,
            EnergyCost = 1,
            Human = true,
            Namekian = true,
            Saiyan = true
        },

        new DBSkill
        {
            Name = "Kamehameha",
            Power = 10,
            EnergyCost = 5,
            Charge = 2,
            Human = true,
            Namekian = true,
            Saiyan = true
        },

        new DBSkill
        {
            Name = "Destructo Disk",
            Power = 20,
            EnergyCost = 20,
            Charge = 5,
            Human = true
        },

        new DBSkill
        {
            Name = "Spirit Bomb",
            Power = 100,
            EnergyCost = 250,
            Charge = 20,
            Saiyan = true
        },

        new DBSkill
        {
            Name = "Dodon Ray",
            Power = 7,
            EnergyCost = 3,
            Human = true
        },

        new DBSkill
        {
            Name = "Tri-Beam",
            Power = 30,
            EnergyCost = 100,
            Charge = 5,
            Human = true
        },

        new DBSkill
        {
            Name = "Instant Transimission",
            EnergyCost = 10,
            Charge = 0,
            Human = true,
            Namekian = true,
            Saiyan = true,
            Special = true
        },

        new DBSkill
        {
            Name = "Masenko",
            Power = 10,
            EnergyCost = 5,
            Human = true
        },

        new DBSkill
        {
            Name = "Special Beam Cannon",
            Power = 50,
            EnergyCost = 20,
            Charge = 10,
            Namekian = true
        },

        new DBSkill
        {
            Name = "Galick Gun",
            Power = 10,
            EnergyCost = 6,
            Charge = 1
        },

        new DBSkill
        {
            Name = "Power Ball",
            EnergyCost = 10,
            Charge = 4,
            Saiyan = true,
            Special = true
        },

        new DBSkill
        {
            Name = "After Image",
            EnergyCost = 2,
            Saiyan = true,
            Human = true,
            Namekian = true,
            Special = true
        },

        new DBSkill
        {
            Name = "Kaioken",
            Power = 10,
            EnergyCost = 1000,
            Saiyan = true,
            Human = true,
            Namekian = true,
            Special = true,
            Role = Constants.Roles.KAIOKEN
        },

        new DBSkill
        {
            Name = "Super Saiyan",
            Power = 50,
            EnergyCost = 500,
            Saiyan = true,
            Special = true,
            Role = Constants.Roles.SUPER
        },

        new DBSkill
        {
            Name = "SSGSS",
            Power = 100,
            EnergyCost = 2000,
            Saiyan = true,
            Special = true,
            Role = Constants.Roles.SSGSS
        },

        };

        public static DBSkill Kaioken = DBFuncs.GetSkill("Kaioken");
        public static DBSkill Super_Saiyan = DBFuncs.GetSkill("Super Saiyan");
        public static DBSkill SSGSS = DBFuncs.GetSkill("SSGSS");

        public static DBSkill[] Saiyan_Forms = { Kaioken, Super_Saiyan, SSGSS };
        public static DBSkill[] Human_Forms = { Kaioken, Super_Saiyan, SSGSS };
        public static DBSkill[] Namekian_Forms = { Kaioken, Super_Saiyan, SSGSS };
    }
}
