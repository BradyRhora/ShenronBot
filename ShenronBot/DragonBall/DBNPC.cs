using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Discord;
using Discord.Commands;
using System.Linq;

namespace ShenronBot
{
    public class DBNPC
    {
        public string Name { get; set; }
        public string Race { get; set; } = "?";
        public int Power_Level { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public ulong Location { get; set; }
        public IMessageChannel Place { get; set; }
        public string Dialogue { get; set; }
        public string Response1 { get; set; }
        public string Response2 { get; set; }
        public string DefeatResponse { get; set; }
        public DBSkill Skill { get; set; } = null;
        public DBUser Target { get; set; }

        public DBNPC()
        {
            Health = Power_Level * 10;
            MaxHealth = Health;
            Place = Bot.client.GetChannel(Location) as IMessageChannel;
        }

        public void Fight(DBUser target)
        {
            Health = Power_Level * 10;
            MaxHealth = Health;
            Place = Bot.client.GetChannel(Location) as IMessageChannel;

            Timer battle = new Timer(Timer(target), null, 10000, 0);
        }

        public TimerCallback Timer(DBUser target)
        {
            Random rdm = new Random();

            if (rdm.Next(100) > 30)
            {
                var emb = DBFuncs.Dialogue(this, $"{Name} attacks {target.User.Username}, doing {Power_Level} damage.");
                Place.SendMessageAsync("", embed: emb);
            }
            else
            {
                if (Skill != null)
                {
                    UseSkill(Skill, target);
                }
            }


            return null;
        }

        public async void UseSkill(DBSkill skill, DBUser user)
        {
            int power = skill.Power * Power_Level;
            await Place.SendMessageAsync($"{Name} uses {skill.Name} on {user.User.Username} with a power of {power}");
            Target.Charging = true;
            Timer timer = new Timer(await user.Hurt(Place, power), null, skill.Charge * 1000, Timeout.Infinite);
        }

        public void Hurt(int damage)
        {
            Health -= damage;
            if (Health > MaxHealth) Health = MaxHealth;
            if (Health <= 0) { Health = 0; Die(); }
        }

        public void Die()
        {
            Place.SendMessageAsync("", embed: DBFuncs.Dialogue(this, DefeatResponse));
        }

    }
}
