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
        public IUserMessage Msg1 { get; set; }
        public IUserMessage Msg2 { get; set; }

        public DragonBall(IChannel chan,int id)
        {
            Location = chan as IMessageChannel;
            ID = id;
            Console.WriteLine($"Dragonball {ID} has landed in {Location.Name}.");
            SendMessages();
        }

        async void SendMessages()
        {
            Msg1 = await Location.SendMessageAsync("A dragonball lands.");
            Msg2 = await Location.SendFileAsync($@"DragonBall\Images\{ID}.png");
        }
    }
}
