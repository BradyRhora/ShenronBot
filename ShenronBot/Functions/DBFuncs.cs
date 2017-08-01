using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Discord;

namespace ShenronBot
{
    public class DBFuncs
    {

        public static string GetAttribute(IUser user, string attributeToGet)
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
    }
}
