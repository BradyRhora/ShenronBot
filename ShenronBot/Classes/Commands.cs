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

            foreach (CommandInfo command in Bot.commands.Commands)
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

        [Command("adminhelp"), Summary("Displays admin commands and descriptions. [Admin Only]")]
        public async Task AdminHelp()
        {
            if (Funcs.IsAdmin(Context.User))
            {
                JEmbed emb = new JEmbed();
                emb.Author.Name = "Shenron Admin Commands";
                emb.ThumbnailUrl = Context.User.AvatarId;
                emb.ColorStripe = Constants.Colours.SHENRON_GREEN;

                foreach (CommandInfo command in Bot.commands.Commands)
                {
                    if (command.Summary == null)
                    {
                        emb.Fields.Add(new JEmbedField(x =>
                        {
                            string header = "db!" + command.Name;
                            foreach (ParameterInfo parameter in command.Parameters)
                            {
                                header += " [" + parameter.Name + "]";
                            }
                            x.Header = header;
                            x.Text = "";
                        }));
                    }
                }

                await Context.Channel.SendMessageAsync("", embed: emb.Build());
            }
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


                string[] toWrite = new string[9];
                toWrite[0] = Context.User.Username;
                toWrite[1] = $"RACE: {wRace}";
                toWrite[2] = "POWER_LVL: 10";
                toWrite[3] = "MULTIPLIER: 1";
                toWrite[4] = "ENERGY: 100";
                toWrite[5] = "LEVEL: 1";
                toWrite[6] = "EXP: 0";
                toWrite[7] = "LOCATION: null";
                toWrite[8] = "SKILLS:";

                File.WriteAllLines($@"Players\{Context.User.Id}.txt", toWrite);

                DBFuncs.LoadRoles(Context.User);


                await Context.Channel.SendMessageAsync($"{Context.User.Mention} has successfully registered as a {wRace}. Go forth to Planet {planet}.");
            }
            else await Context.Channel.SendMessageAsync("Please choose either Human, Saiyan, or Namekian as your race. `db!register [race]`");
        }

        [Command("register")]
        public async Task Register() { await Context.Channel.SendMessageAsync("Please specify a race. (Human, Saiyan, or Namekian)"); }

        [Command("profile"), Summary("View your or another users profile.")]
        public async Task Profile(IUser user)
        {
            if (DBFuncs.PlayerRegistered(user))
            {
                var emb = new JEmbed()
                {
                    ThumbnailUrl = user.GetAvatarUrl(),
                    ColorStripe = Funcs.GetColour(user, Context.Guild)
                };
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

        [Command("profile")]
        public async Task Profile() { await Profile(Context.User); }

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

        [Command("held"), Summary("Displays the Dragon Balls that you are currently holding.")]
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

        [Command("scouter"), Summary("WHAT DOES THE SCOUTER SAY ABOUT HIS POWER LEVEL?")]
        public async Task Scouter(IUser user)
        {
            var JEmb = new JEmbed();
            JEmb.Author.Name = $"{user.Username}'s Power Level";
            JEmb.ColorStripe = Funcs.GetColour(user, Context.Guild);
            JEmb.Description = Convert.ToString(DBFuncs.GetPowerLVL(user));

            await Context.Channel.SendMessageAsync("", embed: JEmb.Build());
        }

        [Command("scouter")]
        public async Task Scouter() { await Scouter(Context.User); }

        [Command("attack"), Summary("Attacks another player. Kill them and they'll drop their Dragon Balls.")]
        public async Task Attack(IUser user)
        {
            if (DBFuncs.PlayerRegistered(user))
            {
                DBUser attacker = DBFuncs.FindDBUser(Context.User);
                DBUser target = DBFuncs.FindDBUser(user);
                Random rdm = new Random();

                int attackerRole = rdm.Next(10) + 1 + DBFuncs.GetPowerLVL(Context.User);
                int targetRole = rdm.Next(10) + 1 + DBFuncs.GetPowerLVL(user);

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

        [Command("wish"), Summary("Use the power of the Dragon Balls to make a wish to the Eternal Dragon! Input no name to give yourself the wish.")]
        public async Task Wish(string wish, IUser user)
        {
            if (DBFuncs.FindDBUser(user).BallCount() == 7)
            {
                if (wish == "list")
                {
                    JEmbed jemb = new JEmbed();
                    jemb.Author.Name = "Wish List";
                    jemb.Author.IconUrl = Bot.client.CurrentUser.GetAvatarUrl();
                    jemb.ColorStripe = Constants.Colours.SHENRON_GREEN;

                    int counter = 1;
                    foreach (string item in File.ReadAllLines(@"Files\wishes.txt"))
                    {
                        jemb.Fields.Add(new JEmbedField(x =>
                        {
                            x.Header = $"[{counter}]";
                            x.Text = item;
                        }));
                        counter++;
                    }

                    await Context.Channel.SendMessageAsync("", embed: jemb.Build());
                }
                else if (wish == "1")
                {
                    int currentLVL = Convert.ToInt32(DBFuncs.GetAttribute("LEVEL", user));
                    int newLVL = currentLVL + 10;
                    DBFuncs.SetAttribute("LEVEL", user, Convert.ToString(newLVL));

                    await Context.Channel.SendMessageAsync($"Your wish has been granted... {user.Username}! You are now level {newLVL}.");
                }
                else if (wish == "2") { }//increase currency
                else if (wish == "3") { }//give skill
                else if (wish == "4") { }//custom
                else { await Context.Channel.SendMessageAsync($"Wish '{wish}' does not exist. Refer to the wish by it's ID number."); return; }

                for (int i = 0; i < 7; i++)
                {
                    if (Bot.sess.Balls[i].Location == null && Bot.sess.Balls[i].Held == false)
                    {
                        var dbUser = DBFuncs.FindDBUser(Context.User);
                        dbUser.heldBalls.Clear();
                        Bot.sess.Balls[i].Holder = null;
                        Bot.sess.Balls[i].Held = false;
                        Bot.sess.End();
                    }
                }
            }
            else await Context.Channel.SendMessageAsync("Shenron cannot be summoned without all seven **Dragon Balls**!");
        }

        [Command("wish")]
        public async Task Wish(string wish) { await Wish(wish, Context.User); }

        [Command("wish")]
        public async Task Wish() { await Wish("list"); }

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

            JEmbed embed = new JEmbed()
            {
                Title = "Messages deleted.",
                Description = $"{amount} messages deleted.",
                ColorStripe = Constants.Colours.SHENRON_GREEN
            };
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
        public async Task Power(string direction) { await Power(direction, 1); }

        [Command("power")]
        public async Task Power(string direction, int amount)
        {
            var guilds = Constants.Guilds.PLANETS;
            if (direction == "up")
            {
                int lvl = Convert.ToInt32(DBFuncs.GetAttribute("LEVEL", Context.User));
                string multiplier = "1";
                string race = DBFuncs.GetAttribute("RACE", Context.User);

                ulong[] roles = new ulong[3];
<<<<<<< HEAD
=======
                if (lvl >= 100 && race == "Saiyan") { roles = Constants.Roles.SSGSS; multiplier = "1000"; }
                else if (lvl >= 50 && race == "Saiyan") { roles = Constants.Roles.SUPER; multiplier = "100"; }
                else if (lvl >= 10) { roles = Constants.Roles.KAIOKEN; multiplier = "10"; }
                else { await Context.Channel.SendMessageAsync("You attempt to increase your power, but nothing happens."); return; }
>>>>>>> origin/master

                DBFuncs.SetAttribute("MULTIPLIER", Context.User, multiplier);


                
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
                DBFuncs.SetAttribute("MULTIPLIER", Context.User, "1");
            }
        }
        
        [Command("giveexp")]
        public async Task GiveEXP(int amount, IUser user)
        {
            DBFuncs.AddEXP(user, amount);
            await Context.Channel.SendMessageAsync($"{user.Mention} has been given {amount} EXP.");
        }

        [Command("moveplanet"), Summary("Currently used to switch planets, will not remain this way.")]
        public async Task MovePlanet(string planet)
        {
            string msg = await DBFuncs.MoveToPlanet(Context.User, planet);
            await Context.Channel.SendMessageAsync(msg);
        }

        [Command("train")]
        public async Task Train(string npcChoice)
        {
            bool npcfound = false;
            foreach (DBNPC npc in NPCList.NPCs)
            {
                if (Context.Channel.Id == npc.Location)
                {
                    if (npc.Name.ToLower().Contains(npcChoice.ToLower()))
                    {
                        npcfound = true;
                        JEmbed JEmb = new JEmbed();
                        JEmb.ThumbnailUrl = $@"DragonBall\Images\{npc.Name}.jpg";
                        JEmb.Author.Name = npc.Name;
                        JEmb.Description = npc.Dialogue;

                        JEmb.Fields.Add(new JEmbedField(x =>
                        {
                            x.Header = "Race:";
                            x.Text = $"{npc.Race}";
                            x.Inline = true;
                        }));

                        JEmb.Fields.Add(new JEmbedField(x =>
                        {
                            x.Header = "Power Level:";
                            x.Text = $"~{npc.Power_Level}";
                            x.Inline = true;
                        }));

                        string name = npc.Name;
                        if (npc.Name.Contains(" ")) name = $"\"{npc.Name}\"";

                        JEmb.Fields.Add(new JEmbedField(x =>
                        {
                            x.Header = $"db!train {name} 1";
                            x.Text = "Train me!";
                        }));

                        JEmb.Fields.Add(new JEmbedField(x =>
                        {
                            x.Header = $"db!train {name} 2";
                            x.Text = "Nevermind.";
                        }));

                        JEmb.ColorStripe = Funcs.GetColour(Context.User, Context.Guild);
                        
                        await Context.Channel.SendMessageAsync("", embed: JEmb.Build());
                        break;
                    }
                }
            }
            if (!npcfound) await Context.Channel.SendMessageAsync($"Cannot find NPC with name {npcChoice}.");

        }

        [Command("train")]
        public async Task Train(string npcChoice, int choice)
        {
            foreach (DBNPC npc in NPCList.NPCs)
            {
                if (Context.Channel.Id == npc.Location)
                {
                    if (npc.Name.ToLower().Contains(npcChoice.ToLower()))
                    {

                    }
                }
            }
        }

        [Command("train"), Summary("Increase your strength and learn new techniques.")]
        public async Task Train()
        {
            List<DBNPC> inLocation = new List<DBNPC>();
            List<DBUser> here = new List<DBUser>();

            foreach (DBNPC npc in NPCList.NPCs)
            {
                if (Context.Channel.Id == npc.Location)
                {
                    inLocation.Add(npc);
                }
            }

            foreach (DBUser user in Bot.sess.Players)
            {
                if (Context.Channel.Id == Convert.ToUInt64(DBFuncs.GetAttribute("LOCATION",user.User)) && user.User.Id != Context.User.Id)
                {
                    here.Add(user);
                }
            }

            var JEmb = new JEmbed();

            JEmb.Description = "People in area:";

            if (inLocation.Count > 0 || here.Count > 0)
            {
                foreach (DBNPC npc in inLocation)
                {
                    JEmb.Fields.Add(new JEmbedField(x =>
                    {
                        x.Header = npc.Name + " [NPC]";
                        x.Text = $"Power Level: ~{npc.Power_Level}";
                    }));
                }

                foreach (DBUser user in here)
                {
                    JEmb.Fields.Add(new JEmbedField(x =>
                    {
                        x.Header = user.User.Username + " [PLAYER]";
                        x.Text = $"Power Level: ~{DBFuncs.GetPowerLVL(user.User)}";
                    }));
                }

                JEmb.ColorStripe = Funcs.GetColour(Context.User, Context.Guild);
                JEmb.Footer.Text = "Type 'db!train [name]' to request to train with them!";
            }
            else
            {
                JEmb.Fields.Add(new JEmbedField(x =>
                {
                    x.Header = "None";
                    x.Text = "There seems to be no one here.. Check another area.";
                }));
            }

            await Context.Channel.SendMessageAsync("", embed: JEmb.Build());
        }
    }

}