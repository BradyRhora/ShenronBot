﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ShenronBot
{
    public class Bot
    {
        static void Main(string[] args) => new Bot().Run().GetAwaiter().GetResult();

        #region Vars
        public static DiscordSocketClient client;
        public static CommandService commands;
        public static DBSession sess = new DBSession();
        public static List<DBUser> Players = new List<DBUser>();
        #endregion

        public async Task Run()
        {
            Start:
            try
            {
                Console.WriteLine("Welcome, Brady. Initializing Shenron...");
                client = new DiscordSocketClient();
                Console.WriteLine("Client Initialized.");
                commands = new CommandService();
                Console.WriteLine("Command Service Initialized.");
                string token = File.ReadAllLines(@"Constants\Token")[0];
                await InstallCommands();
                Console.WriteLine("Commands Installed, logging in.");
                await client.LoginAsync(TokenType.Bot, token);
                Console.WriteLine("Successfully logged in!");
                // Connect the client to Discord's gateway
                await client.StartAsync();
                Console.WriteLine("Shenron successfully intialized");
                // Block this task until the program is exited.
                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n==========================================================================");
                Console.WriteLine("                                  ERROR                        ");
                Console.WriteLine("==========================================================================\n");
                Console.WriteLine($"Error occured in {e.Source}");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);

                Again:

                Console.WriteLine("Would you like to try reconnecting? [Y/N]");
                var input = Console.Read();

                if (input == 121) { Console.Clear(); goto Start; }
                else if (input == 110) Environment.Exit(0);

                Console.WriteLine("Invalid input.");
                goto Again;
            }
        }

        public async Task InstallCommands()
        {
            // Hook the MessageReceived Event into our Command Handler
            client.MessageReceived += HandleCommand;
            client.UserJoined += HandleJoin;
            client.Ready += HandleReady;
            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            
            if (!Constants.Channels.BLOCKED_CHANNELS.Contains(message.Channel.Id))
            {
                DBUser user = DBFuncs.FindDBUser(message.Author);
                user.Location = message.Channel.Id;
                DBFuncs.SetAttribute("LOCATION", message.Author, Convert.ToString(message.Channel.Id));
            }

            //changed prefix to db! because even though bot is called shenron, its more DB in general.
            if (message.HasStringPrefix("db!", ref argPos))
            {
                if (DBFuncs.PlayerRegistered(message.Author) || message.Content.StartsWith("db!register") || message.Content.StartsWith("db!help"))
                {
                    var context = new CommandContext(client, message);
                    var result = await commands.ExecuteAsync(context, argPos);
                    if (!result.IsSuccess)
                        Console.WriteLine(result.ErrorReason);
                }
                else await message.Channel.SendMessageAsync($"{message.Author.Mention}, you must register with `db!register` before using commands.");
            }
            else return;
        }

        public async Task HandleJoin(SocketGuildUser user)
        {
            ulong ID = user.Guild.Id;

            IChannel chan = user.Guild.GetChannel(Constants.Channels.EARTH_GEN);
            if (ID == Constants.Guilds.DBZ_EARTH) chan = user.Guild.GetChannel(Constants.Channels.EARTH_GEN);
            else if (ID == Constants.Guilds.DBZ_NAMEK) chan = user.Guild.GetChannel(Constants.Channels.NAMEK_GEN);
            else if (ID == Constants.Guilds.DBZ_VEGETA) chan = user.Guild.GetChannel(Constants.Channels.VEGETA_GEN);
            var mChan = chan as IMessageChannel;
            string reg = "";
            if (DBFuncs.PlayerRegistered(user)) DBFuncs.LoadRoles(user);
            else reg = " Use `db!register [race]` to register as either a Human, Namekian, or Saiyan.";
            await mChan.SendMessageAsync($"{user.Mention} has entered {user.Guild.Name.Replace("DBZ ", "")}.{reg}");
        }

        public async Task HandleReady()
        {
            try
            {
                foreach (IUser user in client.GetGuild(Constants.Guilds.DBZ_EARTH).Users)
                {
                    if (DBFuncs.PlayerRegistered(user))
                    {
                        if (DBFuncs.GetAttribute("LOCATION", user) != "null")
                        {
                            DBFuncs.FindDBUser(user).Location = Convert.ToUInt64(DBFuncs.GetAttribute("LOCATION", user));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    ///Late Night THOTS:
    /*
     * Change server names from DBZ to DB.
     * Basically fuckin Xenoverse TBH but more dragon ball focused.
     * Are items necessary? We'll see. def not first priority.
     * Obv current fighting system is NOT final.
     * 
     * Enemies? (Vegeta, Freiza, Cell, Buu, Goku??, etc.)
     * While we're on the thought of fighting Goku... NPCs?
     * 
     * NPCs? (Goku, Piccolo, Krillin, Yamcha, Tien, Roshi, Bulma, etc.)
     * 
     * NPCs:
     *      Only appear in certain locations
     *      Can be trained with
     *      Teams??? We Pokemon Go boyz?
     * How would NPCs be done?
     * Surely having their own bots would be pointless...
     * Or would it?
     * If not, then now?
     * Mention obv. places for NPCs 
     * NPCs can move? Bot informs players when npc has arrived in location?
     * NPCObject?
     * 
     * I'm liking this idea more and more.
     * 
     *          (8/1/2017 11:43AM) On the bus to Jace.
     * 
     * This morning I was thinking that maybe players of certain races would start on different planets,
     * and they could only be on one planet at a time. While on a planet they would have a role stating so 
     * that the bot would give them on all servers. When trying to access another planet their role would 
     * make it so that they cannot read or send messages in the alternate server. Players cannot freely
     * travel between servers. They require tools (space ships) or skills (Instant transmission).
     * 
     * What else? On the 102 now. Wish I could test without internet.
     * 
     * Battle Ideas
     *      Anyone can attack anyone. Battle does not have to be initiated. This will be easier for RP and
     *      even puts less work on me, so win-win. Maybe have a command such as d!attack then input what you
     *      use to attack as the parameter.
     * 
     * 
     *          (4:28 PM) On the bus home.
     * 
     * No thoughts currently. Will review notes and attempt to implement at home.
     * When I have an internet connection, I will make the server "ready for people", so that Moon and Aku can 
     * join and help me test. Entering subway station now.
     * 
     * At subway. 15 minutes until bus approximately. No WiFi available. Let's see if there's anything I can do.
     * 
     *          (8/2/2017 12:42 AM) In Andrew's Bed.
     *          
     * Josh is on top of me. Going to implement some ideas.
     * 
     * 
     */
}