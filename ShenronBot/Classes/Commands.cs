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

            emb.ColorStripe = Funcs.GetColour(Context.User, Context.Guild);
            
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
                
                if (race == "human")
                {
                    wRace = Player.Race.Human;
                    planet = "Earth: https://discord.gg/UwDKgAF";
                    var guild = Bot.client.GetGuild(Constants.Guilds.DBZ_EARTH);
                    var gUser = guild.GetUser(Context.User.Id);
                    await gUser.AddRoleAsync(guild.GetRole(Constants.Roles.ON_EARTH));

                    for (int i = 0; i < 3; i++)
                    {
                        var rGuild = Bot.client.GetGuild(Constants.Guilds.PLANETS[i]);
                        var rRole = rGuild.GetRole(Constants.Roles.HUMAN[i]);
                        var rUser = rGuild.GetUser(Context.User.Id);
                        await rUser.AddRoleAsync(rRole);
                    }
                }
                else if (race == "saiyan")
                {
                    wRace = Player.Race.Saiyan;
                    planet = "Vegeta: https://discord.gg/vFaS8u2";
                    var guild = Bot.client.GetGuild(Constants.Guilds.DBZ_VEGETA);
                    var gUser = guild.GetUser(Context.User.Id);
                    await gUser.AddRoleAsync(guild.GetRole(Constants.Roles.ON_VEGETA));

                    for (int i = 0; i < 3; i++)
                    {
                        var rGuild = Bot.client.GetGuild(Constants.Guilds.PLANETS[i]);
                        var rRole = rGuild.GetRole(Constants.Roles.SAIYAN[i]);
                        var rUser = rGuild.GetUser(Context.User.Id);
                        await rUser.AddRoleAsync(rRole);
                    }
                }
                else if (race == "namekian")
                {
                    wRace = Player.Race.Namekian;
                    planet = "Namek: https://discord.gg/tJVfgms";
                    var guild = Bot.client.GetGuild(Constants.Guilds.DBZ_NAMEK);
                    var gUser = guild.GetUser(Context.User.Id);
                    await gUser.AddRoleAsync(guild.GetRole(Constants.Roles.ON_NAMEK));

                    for (int i = 0; i < 3; i++)
                    {
                        var rGuild = Bot.client.GetGuild(Constants.Guilds.PLANETS[i]);
                        var rRole = rGuild.GetRole(Constants.Roles.NAMEKIAN[i]);
                        var rUser = rGuild.GetUser(Context.User.Id);
                        await rUser.AddRoleAsync(rRole);
                    }
                }

                string[] toWrite = new string[3];
                toWrite[0] = Context.User.Username;
                toWrite[1] = $"RACE: {wRace}";
                toWrite[2] = "POWER_LVL: 10";

                File.WriteAllLines($@"Players\{Context.User.Id}.txt", toWrite);
                

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
            FindDBUser(Context.User);

            for (int i = 0; i < 7; i++)
            {
                if (Bot.sess.Balls[i].Location == Context.Channel && Bot.sess.Balls[i].Held == false)
                {
                    await Context.Channel.SendMessageAsync($"{Context.User.Username} collects the {Bot.sess.Balls[i].ID} star Dragon Ball.");
                    var dbUser = FindDBUser(Context.User);
                    dbUser.heldBalls.Add(Bot.sess.Balls[i]);
                    Console.WriteLine($"{Context.User.Username} has picked up the { Bot.sess.Balls[i].ID} star Dragon Ball in {Context.Channel.Name} of {Context.Guild.Name}.");
                    Bot.sess.Balls[i].Holder = Context.User;
                    Bot.sess.Balls[i].Held = true;
                    await Bot.sess.Balls[i].Msg1.DeleteAsync();
                    await Bot.sess.Balls[i].Msg2.DeleteAsync();
                }
            }
        }

        [Command("held"),Summary("Displays the Dragon Balls that you are currently holding.")]
        public async Task Held()
        {
            string msg = $"{Context.User.Username}, you are currently holding ";
            for (int i = 0; i < 7; i++)
            {
                if (Bot.sess.Balls[i].Held && Bot.sess.Balls[i].Holder == Context.User) msg += $"the **{i + 1} Star Ball**, ";
            }

            if (msg == $"{Context.User.Username}, you are currently holding ") msg += "no Dragon Balls.";
            else msg = msg.Substring(0, msg.Length - 2); msg += ".";
            await Context.Channel.SendMessageAsync(msg);
        }

        [Command("attack"), Summary("Attacks another player. Kill them and they'll drop their Dragon Balls.")]
        public async Task Attack(IUser user)
        {
            DBUser attacker = FindDBUser(Context.User);
            DBUser target = FindDBUser(user);
            Random rdm = new Random();

            int attackerRole = rdm.Next(10) + 1;
            int targetRole = rdm.Next(10) + 1;

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

        [Command("give"), Summary("Gives the specified Dragon Ball to the specified user.")]
        public async Task Give(int ID, IUser user)
        {
            var dbUser = FindDBUser(Context.User);
            bool hasBall = false;
            int index = -1;
            for (int i = 0; i < dbUser.BallCount(); i++) if (dbUser.heldBalls[i].ID == ID) { hasBall = true; index = i; break; }
            if (hasBall)
            {
                DragonBall transferBall = dbUser.heldBalls[index];
                dbUser.heldBalls.RemoveAt(index);
                FindDBUser(user).heldBalls.Add(transferBall);
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
            IRole human = Context.Guild.GetRole(Constants.Roles.HUMAN[0]);
            IRole saiyan = Context.Guild.GetRole(Constants.Roles.SAIYAN[0]);
            IRole namekian = Context.Guild.GetRole(Constants.Roles.NAMEKIAN[0]);

            IGuild namek = Bot.client.GetGuild(Constants.Guilds.DBZ_NAMEK);
            IGuild vegeta = Bot.client.GetGuild(Constants.Guilds.DBZ_VEGETA);

            await namek.CreateRoleAsync(saiyan.Name, saiyan.Permissions, saiyan.Color, saiyan.IsHoisted);
            await vegeta.CreateRoleAsync(saiyan.Name, saiyan.Permissions, saiyan.Color, saiyan.IsHoisted);

            await namek.CreateRoleAsync(human.Name, human.Permissions, human.Color, human.IsHoisted);
            await vegeta.CreateRoleAsync(human.Name, human.Permissions, human.Color, human.IsHoisted);
            
            await namek.CreateRoleAsync(namekian.Name, namekian.Permissions, namekian.Color, namekian.IsHoisted);
            await vegeta.CreateRoleAsync(namekian.Name, namekian.Permissions, namekian.Color, namekian.IsHoisted);

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
        

        DBUser FindDBUser(IUser user)
        {
            int count = 0;
            while (true)
            {
                count++;
                for (int i = 0; i < Bot.sess.Players.Count(); i++) if (Bot.sess.Players[i].User.Id == user.Id) return Bot.sess.Players[i];
                Bot.sess.Players.Add(new DBUser(Context.User));
                if (count == 5 || count == 20 || count == 100) Console.WriteLine("Something is wrong with FindDBuser()!");
            }

        }


    }

}