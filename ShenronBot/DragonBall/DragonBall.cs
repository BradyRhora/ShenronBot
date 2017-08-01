using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace ShenronBot
{
    public class DragonBall
    {
        public IMessageChannel Location { get; set; }
        public int ID { get; }
        public IUser Holder { get; set; }
        public bool Held { get; set; } = false;

        public DragonBall(IChannel chan,int id)
        {
            Location = chan as IMessageChannel;
            ID = id;
            Random rdm = new Random();
            int time = rdm.Next(1000000);
            for (int i = 0; i < time; i++) { }
            Location.SendMessageAsync("A dragonball lands. `+db`");
            Console.WriteLine($"Dragonball {ID} has landed in {Location.Name}.");
            Location.SendFileAsync($@"DragonBall\Images\{ID}.png");
        }
    }
}
