using System;
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
            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            
            if (message.HasStringPrefix("db!", ref argPos))
            //changed prefix to db! because even though bot is called shenron, its more DB in general.
            if (message.HasStringPrefix("db!", ref argPos))
            {

                var context = new CommandContext(client, message);
                var result = await commands.ExecuteAsync(context, argPos);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
            else return;
        }
    }

    ///Late Night THOTS:
    /*
     * On death, ability to communicate and interact in DBZ Other World?
     * Can no longer communicate in other DBZ servers?
     * DBUser compatible with Player\ files?
     * Change server names from DBZ to DB.
     * UPLOAD TO GITHUB?
     * Worry about the fact that GitHub will be public.
     * Cry?
     * ???
     * Profit.
     * Powering up. EXP System? Levels? Just Power level?
     * Saiyans:
     *      Saiyan
     *      Super Saiyan
     *      Super Saiyan 2
     *      Super Saiyan 3
     *      Super Saiyan 4? (Research saiyan levels)
     *      ???
     *      Super Saiyan God
     *      Super Saiyan God Super Saiyan (Ridiculous)
     * Humans:
     *      how tf do humans work..?
     *      fuck
     * Namekians:
     *      why the fuck would anyone want to be one of these.
     * All:
     *      Learn skills and abilities?
     *      Kaoken? (Learn 2 spell)
     * Basically fuckin Xenoverse TBH but more dragon ball focused.
     * Are items necessary? We'll see. def not first priority.
     * Obv current fighting system is NOT final.
     * 
     * Alright, time to sleep. Gotta get up in ~6 hours. Godspeed, future Brady.
     * (((TIME TRAVEL?????)))
     * Anywa- FUCK MORE IDEAS.
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
     */
}