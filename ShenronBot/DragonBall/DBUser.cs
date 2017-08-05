using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading;
using System.Threading.Tasks;

namespace ShenronBot
{
    public class DBUser
    {
        public IUser User {get;set;}
        public DBNPC NPC { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public List<DragonBall> heldBalls;
        public bool IsDead { get; set; } = false;
        public ulong Location { get; set; }
        public int Energy { get; set; }
        public bool Charging { get; set; } = false;
        public DBUser(IUser user)
        {
            User = user;
            heldBalls = new List<DragonBall>();
            Energy = Convert.ToInt32(DBFuncs.GetAttribute("LEVEL", User)) * 100;
            Health = Convert.ToInt32(DBFuncs.GetAttribute("LEVEL", User)) * 100;
            MaxHealth = Health;
        }

        public DBUser(DBNPC npc)
        {
            Energy = npc.Power_Level * 1000;
            Health = npc.Power_Level * 1000;
            MaxHealth = Health;
            Location = npc.Location;
            NPC = npc;
        }

        public async Task<TimerCallback> Hurt(IMessageChannel hurtLocation, int damage)
        {
            Health-=damage;
            if (Health > MaxHealth) Health = MaxHealth;
            if (Health <= 0) { Health = 0; Die(hurtLocation); }

            await hurtLocation.SendMessageAsync($"{User.Username} takes damage!");
            string msg = "They Collapse to the ground";
            if (BallCount() > 0) msg += " and drop all their Dragon Balls.";
            else msg += ".";
            if (IsDead) await hurtLocation.SendMessageAsync(msg);
            Charging = false;
            return null;
        }

        void Die(IMessageChannel deathLocation)
        {
            foreach (DragonBall ball in heldBalls)
            {
                ball.Held = false;
                ball.Location = deathLocation;
            }
            IsDead = true;

        }

        public int BallCount()
        {
            return heldBalls.Count;
        }
        
    }
}
