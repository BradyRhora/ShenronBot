using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace ShenronBot
{
    public class DBUser
    {
        public IUser User {get;set;}
        public int Health { get; set; } = 3;
        public List<DragonBall> heldBalls;
        public bool IsDead { get; set; } = false;

        public DBUser(IUser user)
        {
            User = user;
            heldBalls = new List<DragonBall>();
        }

        public void Hurt(IMessageChannel hurtLocation)
        {
            Health--;
            if (Health == 0) Die(hurtLocation);
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
