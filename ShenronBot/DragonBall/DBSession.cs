using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Linq;

namespace ShenronBot
{
    public class DBSession
    {
        DateTime StartTime { get; }
        public DragonBall[] Balls { get; set; }
        public List<DBUser> Players { get; set; } = new List<DBUser>();

        public DBSession()
        {
            StartTime = DateTime.Now;
        }

        public async void LaunchBalls()
        {
            IGuild[] guilds = { Bot.client.GetGuild(Constants.Guilds.DBZ_EARTH), Bot.client.GetGuild(Constants.Guilds.DBZ_NAMEK), Bot.client.GetGuild(Constants.Guilds.DBZ_VEGETA) };
            Random rdm = new Random();
            Balls = new DragonBall[7];

            for (int i = 0; i < 7; i++)
            {
                
                var guild = guilds[rdm.Next(guilds.Length)];
                var chans = await guild.GetTextChannelsAsync();
                ITextChannel chan;
                while (true)
                {
                    int chanNum = rdm.Next(chans.Count);
                    chan = chans.ElementAt(chanNum);
                    bool no = false;
                    for (int a = 0; a < Constants.Channels.BLOCKED_CHANNELS.Count(); a++)
                    {
                        if (Constants.Channels.BLOCKED_CHANNELS[a] == (chan.Id)) no = true;
                    }


                    if (!no) break;
                }
                Balls[i] = new DragonBall(chan, i + 1);
            }
        }

        public void End()
        {
            Players.Clear();
            Balls = null;
        }
    }
}
