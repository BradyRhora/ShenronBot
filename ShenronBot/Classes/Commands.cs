using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace ShenronBot
{
    public class Commands : ModuleBase
    {

        [Command("help"), Summary("Displays commands and descriptions.")]
        public async Task Help()
        {
            JEmbed emb = new JEmbed();
            emb.Author.Name = "Shenron Commands";
            emb.ThumbnailUrl = Context.User.AvatarId;
            emb.ColorStripe = Constants.Colours.SHENRON_GREEN;
            
            foreach(CommandInfo command in Bot.commands.Commands)
            {
                emb.Fields.Add(new JEmbedField(x =>
                {
                    string header = "db!" + command.Name;
                    foreach (ParameterInfo parameter in command.Parameters)
                    {
                        header += " [" + parameter.Name + "]";
                    }
                    x.Header = header;
                    x.Text = command.Summary;
                }));
            }

            await Context.Channel.SendMessageAsync("", embed: emb.Build());

        }

        [Command("register"), Summary("Use this to register!")]
        public async Task Register(string race)
        {
            race = race.ToLower();
            if (DBFuncs.PlayerRegistered(Context.User)) await Context.Channel.SendMessageAsync($"{Context.User.Mention} has already registered.");
            else if (race == "human" || race == "saiyan" || race == "namekian")
            {
                Player.Race wRace = Player.Race.Human;
                string planet = "";

                if (race == "human") { wRace = Player.Race.Human; planet = "Earth: https://discord.gg/UwDKgAF"; }
                else if (race == "saiyan") { wRace = Player.Race.Saiyan; planet = "Vegeta: https://discord.gg/vFaS8u2"; }
                else if (race == "namekian") { wRace = Player.Race.Namekian; planet = "Namek: https://discord.gg/tJVfgms"; }


                string[] toWrite = new string[5];
                toWrite[0] = Context.User.Username;
                toWrite[1] = $"RACE: {wRace}";
                toWrite[2] = "POWER_LVL: 10";
                toWrite[3] = "LEVEL: 1";
                toWrite[4] = "EXP: 0";

                File.WriteAllLines($@"Players\{Context.User.Id}.txt", toWrite);

                DBFuncs.LoadRoles(Context.User);
                

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} has successfully registered as a {wRace}. Go forth to Planet {planet}.");
            }
            else await Context.Channel.SendMessageAsync("Please choose either Human, Saiyan, or Namekian as your race. `db!register [race]`");
        }

        [Command("start"), Summary("Starts a new Dragon Ball Session. Can only be done by GM.")]
        public async Task Start()
        {
            if (Context.User.Id == Constants.Users.BRADY)
            {
                Bot.sess = new DBSession();
                Bot.sess.LaunchBalls();
                await Context.Channel.SendMessageAsync("Seven Dragon Balls ascend and launch through the sky in different directions.");
            }
        }

        [Command("pickup"), Alias("pu"), Summary("Picks up any Dragon Balls that are in the current area.")]
        public async Task PickUp()
        {
            var user = DBFuncs.FindDBUser(Context.User);
            if (!user.IsDead)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (Bot.sess.Balls[i].Location == Context.Channel && Bot.sess.Balls[i].Held == false)
                    {
                        await Context.Channel.SendMessageAsync($"{Context.User.Username} collects the {Bot.sess.Balls[i].ID} star Dragon Ball.");
                        var dbUser = DBFuncs.FindDBUser(Context.User);
                        dbUser.heldBalls.Add(Bot.sess.Balls[i]);
                        Console.WriteLine($"{Context.User.Username} has picked up the { Bot.sess.Balls[i].ID} star Dragon Ball in {Context.Channel.Name} of {Context.Guild.Name}.");
                        Bot.sess.Balls[i].Holder = Context.User;
                        Bot.sess.Balls[i].Held = true;
                        await Bot.sess.Balls[i].Msg1.DeleteAsync();
                        await Bot.sess.Balls[i].Msg2.DeleteAsync();
                    }
                }
            }
        }

        [Command("held"),Summary("Displays the Dragon Balls that you are currently holding.")]
        public async Task Held()
        {
            string msg = $"{Context.User.Username}, you are currently holding ";
            for (int i = 0; i < 7; i++)
            {
                if (Bot.sess.Balls[i].Held && Bot.sess.Balls[i].Holder.Id == Context.User.Id) msg += $"the **{i + 1} Star Ball**, ";
            }

            if (msg == $"{Context.User.Username}, you are currently holding ") msg += "no Dragon Balls.";
            else msg = msg.Substring(0, msg.Length - 2); msg += ".";
            await Context.Channel.SendMessageAsync(msg);
        }

        [Command("attack"), Summary("Attacks another player. Kill them and they'll drop their Dragon Balls.")]
        public async Task Attack(IUser user)
        {
            if (DBFuncs.PlayerRegistered(user))
            {
                DBUser attacker = DBFuncs.FindDBUser(Context.User);
                DBUser target = DBFuncs.FindDBUser(user);
                Random rdm = new Random();

                int attackerRole = rdm.Next(10) + 1 + Convert.ToInt32(DBFuncs.GetAttribute("POWER_LVL", Context.User));
                int targetRole = rdm.Next(10) + 1 + Convert.ToInt32(DBFuncs.GetAttribute("POWER_LVL", user)); ;

                await Context.Channel.SendMessageAsync($"{Context.User.Mention} attacks with a power of {attackerRole}, against {user.Mention}'s defense with power of {targetRole}!");
                if (attackerRole > targetRole)
                {
                    target.Hurt(Context.Channel);
                    await Context.Channel.SendMessageAsync($"{user.Username} takes damage!");
                    string msg = "They Collapse to the ground";
                    if (target.BallCount() > 0) msg += " and drop all their Dragon Balls.";
                    else msg += ".";
                    if (target.IsDead) await Context.Channel.SendMessageAsync(msg);
                }
                else if (targetRole > attackerRole)
                {
                    attacker.Hurt(Context.Channel);
                    await Context.Channel.SendMessageAsync($"{Context.User.Username} takes damage!");
                    string msg = "They Collapse to the ground";
                    if (attacker.BallCount() > 0) msg += " and drop all their Dragon Balls.";
                    else msg += ".";
                    if (attacker.IsDead) await Context.Channel.SendMessageAsync(msg);
                }
                else
                {
                    await Context.Channel.SendMessageAsync("The two collide, but nothing happens.");
                }
            }
            else await Context.Channel.SendMessageAsync("The specified player could not be found. They are probably not registered.");
        }

        [Command("give"), Summary("Gives the specified Dragon Ball to the specified user.")]
        public async Task Give(int ID, IUser user)
        {
            var dbUser = DBFuncs.FindDBUser(Context.User);
            bool hasBall = false;
            int index = -1;
            for (int i = 0; i < dbUser.BallCount(); i++) if (dbUser.heldBalls[i].ID == ID) { hasBall = true; index = i; break; }
            if (hasBall)
            {
                DragonBall transferBall = dbUser.heldBalls[index];
                dbUser.heldBalls.RemoveAt(index);
                DBFuncs.FindDBUser(user).heldBalls.Add(transferBall);
                transferBall.Holder = user;
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} has given {user.Mention} the {transferBall.ID} Star Ball");
            }
            else
            {
                await Context.Channel.SendMessageAsync("You do not have that ball.");
            }
        }

        [Command("initialize"), Alias("init")]
        public async Task Initialize()
        {
            foreach (ulong guild in Constants.Guilds.PLANETS)
            {
                var Guild = Bot.client.GetGuild(guild);
                await Guild.CreateRoleAsync("Super Saiyan", color: Constants.Colours.SS_YELLOW);
                await Guild.CreateRoleAsync("SSGSS", color: Constants.Colours.SSGSS_BLUE);
                await Guild.CreateRoleAsync("Kaioken", color: Constants.Colours.KAIOKEN);
            }
            await Context.Channel.SendMessageAsync("Done");
        }

        [Command("purge")]
        public async Task Purge(int amount)
        {
            
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);

            JEmbed embed = new JEmbed();
            embed.Title = "Messages deleted.";
            embed.Description = $"{amount} messages deleted.";
            embed.ColorStripe = Constants.Colours.SHENRON_GREEN;
            var emb = embed.Build();
            var msg = await Context.Channel.SendMessageAsync("", embed: emb);
            Thread.Sleep(2000);
            await msg.DeleteAsync();
        }

        [Command("eval")]
        public async Task EvaluateCmd([Remainder] string expression)
        {
            if (Context.User.Id == Constants.Users.BRADY)
            {
                IUserMessage msg = await ReplyAsync("Evaluating...");
                string result = await EvalService.EvaluateAsync(Context, expression);
                var user = Context.User as IGuildUser;
                if (user.RoleIds.ToArray().Count() > 1)
                {
                    var role = Context.Guild.GetRole(user.RoleIds.ElementAtOrDefault(1));
                    var emb = new EmbedBuilder().WithColor(role.Color).WithDescription(result).WithTitle("Evaluated").WithCurrentTimestamp();
                    await Context.Channel.SendMessageAsync("", embed: emb);
                }
                else
                {
                    var emb = new EmbedBuilder().WithColor(Constants.Colours.SHENRON_GREEN).WithDescription(result).WithTitle("Evaluated").WithCurrentTimestamp();
                    await Context.Channel.SendMessageAsync("", embed: emb);
                    
                }
            }

        }

        [Command("power"), Summary("Adjust your power and go to the next level! (Direction = up/down)")]
        public async Task Power(string direction)
        {
            var guilds = Constants.Guilds.PLANETS;
            if (direction == "up")
            {
                int lvl = Convert.ToInt32(DBFuncs.GetAttribute("LEVEL", Context.User));
                ulong[] roles = new ulong[3];
                if (lvl >= 100) roles = Constants.Roles.SSGSS;
                else if (lvl >= 50) roles = Constants.Roles.SUPER;
                else if (lvl >= 10) roles = Constants.Roles.KAIOKEN;
                else { await Context.Channel.SendMessageAsync("You attempt to increase your power, but nothing happens."); return; }


                for (int i = 0; i < 3; i++)
                {
                    var guildID = guilds.ElementAt(i);
                    var guild = Bot.client.GetGuild(guildID);
                    if (await Funcs.InGuild(guild, Context.User))
                    {
                        var user = guild.GetUser(Context.User.Id);
                        await user.AddRoleAsync(guild.GetRole(roles[i]));
                    }
                }
            }
            else if (direction == "down")
            {
                for (int i = 0; i < 3; i++)
                {
                    var guildID = guilds.ElementAt(i);
                    var guild = Bot.client.GetGuild(guildID);
                    if (await Funcs.InGuild(guild, Context.User))
                    {
                        var user = guild.GetUser(Context.User.Id);
                        if (user.Roles.Contains(guild.GetRole(Constants.Roles.KAIOKEN[i]))) await user.RemoveRoleAsync(guild.GetRole(Constants.Roles.KAIOKEN[i]));
                        else if (user.Roles.Contains(guild.GetRole(Constants.Roles.SUPER[i]))) await user.RemoveRoleAsync(guild.GetRole(Constants.Roles.SUPER[i]));
                        else if (user.Roles.Contains(guild.GetRole(Constants.Roles.SSGSS[i]))) await user.RemoveRoleAsync(guild.GetRole(Constants.Roles.SSGSS[i]));
                    }
                }
            }
        }
        
        [Command("profile"), Summary("View your or another users profile.")]
        public async Task Profile(IUser user)
        {
            if (DBFuncs.PlayerRegistered(user))
            {
                var emb = new JEmbed();

                emb.ThumbnailUrl = user.GetAvatarUrl();
                emb.ColorStripe = Funcs.GetColour(user, Context.Guild);

                emb.Fields.Add(new JEmbedField(x =>
                {
                    x.Header = "Race";
                    x.Text = DBFuncs.GetAttribute("RACE", user);
                }));

                emb.Fields.Add(new JEmbedField(x =>
                {
                    x.Header = "Level";
                    x.Text = DBFuncs.GetAttribute("LEVEL", user);
                    x.Inline = true;
                }));

                emb.Fields.Add(new JEmbedField(x =>
                {
                    x.Header = "EXP";
                    x.Text = DBFuncs.GetAttribute("EXP", user) + "/" + Convert.ToString(Math.Pow(Convert.ToInt32(DBFuncs.GetAttribute("LEVEL", user)), 2) + 10);
                    x.Inline = true;
                }));

                emb.Author.Name = user.Username + "'s Profile";

                var embed = emb.Build();
                await Context.Channel.SendMessageAsync("", embed: embed);
            }
            else await Context.Channel.SendMessageAsync("Player in not registered");
        }

        [Command("profile"), Summary("View your or another users profile.")]
        public async Task Profile() { await Profile(Context.User); }
        
    }

}