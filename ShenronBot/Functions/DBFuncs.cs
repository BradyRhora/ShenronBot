using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord;
using System.Linq;

namespace ShenronBot
{
    public class DBFuncs
    {

        public static string GetAttribute(string attributeToGet, IUser user)
        {
            string[] attributes = File.ReadAllLines($@"Players\{user.Id}.txt");
            foreach (string attribute in attributes)
            {
                if (attribute.StartsWith(attributeToGet))
                {
                    return attribute.Substring(attributeToGet.Length + 2);
                }
            }
            return "";
        }

        public static void SetAttribute(string attributeToSet, IUser user, string value)
        {
            string[] attributes = File.ReadAllLines($@"Players\{user.Id}.txt");
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i].StartsWith(attributeToSet))
                {
                    attributes[i] = attributeToSet + ": " + value;
                    File.WriteAllLines($@"Players\{user.Id}.txt", attributes);
                    if (attributeToSet == "EXP") CheckEXP(user);
                    break;
                }
            }
        }

        public static bool PlayerRegistered(IUser user)
        {
            if (File.Exists($@"Players\{user.Id}.txt")) return true;
            else return false;
        }

        public static void LoadPlayers()
        {
            foreach (IUser user in Bot.client.GetGuild(Constants.Guilds.DBZ_EARTH).Users) Bot.Players.Add(new DBUser(user));
            foreach (IUser user in Bot.client.GetGuild(Constants.Guilds.DBZ_NAMEK).Users) Bot.Players.Add(new DBUser(user));
            foreach (IUser user in Bot.client.GetGuild(Constants.Guilds.DBZ_VEGETA).Users) Bot.Players.Add(new DBUser(user));
        }

        public async static void LoadRoles(IUser User)
        {
            string race = GetAttribute("RACE", User).ToLower();

            if (race == "human")
            {
                if (await Funcs.InGuild(Bot.client.GetGuild(Constants.Guilds.DBZ_EARTH), User))
                {
                    var guild = Bot.client.GetGuild(Constants.Guilds.DBZ_EARTH);
                    var gUser = guild.GetUser(User.Id);
                    await gUser.AddRoleAsync(guild.GetRole(Constants.Roles.ON_EARTH));

                    for (int i = 0; i < 3; i++)
                    {
                        var rGuild = Bot.client.GetGuild(Constants.Guilds.PLANETS[i]);
                        var rRole = rGuild.GetRole(Constants.Roles.HUMAN[i]);
                        if (await Funcs.InGuild(rGuild, User))
                        {
                            var rUser = rGuild.GetUser(User.Id);
                            await rUser.AddRoleAsync(rRole);
                        }
                    }
                }
            }
            else if (race == "saiyan")
            {
                if (await Funcs.InGuild(Bot.client.GetGuild(Constants.Guilds.DBZ_VEGETA), User))
                {
                    var guild = Bot.client.GetGuild(Constants.Guilds.DBZ_VEGETA);
                    var gUser = guild.GetUser(User.Id);
                    await gUser.AddRoleAsync(guild.GetRole(Constants.Roles.ON_VEGETA));

                    for (int i = 0; i < 3; i++)
                    {
                        var rGuild = Bot.client.GetGuild(Constants.Guilds.PLANETS[i]);
                        var rRole = rGuild.GetRole(Constants.Roles.SAIYAN[i]);
                        if (await Funcs.InGuild(rGuild, User))
                        {
                            var rUser = rGuild.GetUser(User.Id);
                            await rUser.AddRoleAsync(rRole);
                        }
                    }
                }
            }
            else if (race == "namekian")
            {
                if (await Funcs.InGuild(Bot.client.GetGuild(Constants.Guilds.DBZ_NAMEK), User))
                {
                    var guild = Bot.client.GetGuild(Constants.Guilds.DBZ_NAMEK);
                    var gUser = guild.GetUser(User.Id);
                    await gUser.AddRoleAsync(guild.GetRole(Constants.Roles.ON_NAMEK));

                    for (int i = 0; i < 3; i++)
                    {
                        var rGuild = Bot.client.GetGuild(Constants.Guilds.PLANETS[i]);
                        var rRole = rGuild.GetRole(Constants.Roles.NAMEKIAN[i]);
                        if (await Funcs.InGuild(rGuild, User))
                        {
                            var rUser = rGuild.GetUser(User.Id);
                            await rUser.AddRoleAsync(rRole);
                        }
                    }
                }
            }
        }

        public static DBUser FindDBUser(IUser user)
        {
            int count = 0;
            while (true)
            {
                count++;
                for (int i = 0; i < Bot.sess.Players.Count(); i++) if (Bot.sess.Players[i].User.Id == user.Id) return Bot.sess.Players[i];
                Bot.sess.Players.Add(new DBUser(user));
                if (count == 5 || count == 20 || count == 100) Console.WriteLine("Something is wrong with FindDBuser()!");
            }

        }

        public static void CheckEXP(IUser user)
        {
            int exp = Convert.ToInt32(GetAttribute("EXP", user));
            int lvl = Convert.ToInt32(GetAttribute("LEVEL", user));

            if (exp > (Math.Pow(lvl,2) + 10))
            {
                while (exp > (Math.Pow(lvl, 2) + 10))
                {
                    SetAttribute("LEVEL", user, Convert.ToString(lvl++));
                }
                Console.WriteLine($"{user.Username} has leveled up to level {lvl}");
                user.SendMessageAsync($"You have leveled up to level {lvl}.");
            }
        }
    }
}
