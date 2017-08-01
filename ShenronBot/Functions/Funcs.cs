using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Linq;

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
            else return Constants.Colours.DEFAULT_COLOUR;
        }

    }
}
