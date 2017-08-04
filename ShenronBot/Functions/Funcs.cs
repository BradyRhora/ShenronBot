using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ShenronBot
{
    public class Funcs
    {

        public static Color GetColour(IUser User, IGuild Guild)
        {
            var user = User as IGuildUser;

            if (user.RoleIds.ToArray().Count() > 1)
            {
                var role = Guild.GetRole(user.RoleIds.ElementAtOrDefault(1));
                return role.Color;
            }
            else return Constants.Colours.SHENRON_GREEN;
        }

        public static bool IsAdmin(IUser user)
        {
            var gUser = Bot.client.GetGuild(Constants.Guilds.DBZ_EARTH).GetUser(user.Id) as IGuildUser;
            
            if (user.Id == Constants.Users.BRADY || gUser.RoleIds.Contains(Constants.Roles.ADMIN)) return true;
            else return false;
        }

        public static async Task<bool> InGuild(IGuild guild, IUser user)
        {
            foreach (IUser User in await guild.GetUsersAsync())
            {
                if (User.Id == user.Id) return true;
            }

            return false;
        }

        public static async void GiveRole(IRole[] roles, IUser user)
        {
            var guilds = Constants.Guilds.PLANETS;

            for (int i = 0; i < 3; i++)
            {
                var guildID = guilds.ElementAt(i);
                var guild = Bot.client.GetGuild(guildID);
                if (await Funcs.InGuild(guild, user))
                {
                    var gUser = guild.GetUser(user.Id);
                    await gUser.AddRoleAsync(roles[i]);
                }
            }
        }
    }
}
