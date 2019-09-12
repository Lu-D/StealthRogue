using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
{
    public class PlayerEventHandler : EventHandler
    {
        protected PlayerControl player;

        private void Awake()
        {
            player = GetComponent<PlayerControl>();
        }

        public override void handleEvent(DamageEvent eventObj)
        {
            if (player.health > 0)
                player.health -= eventObj.damage;
        }

        public override void handleEvent(HealEvent eventObj)
        {
            if (player.health < player.maxHealth)
            {
                if (player.health + eventObj.heal > player.maxHealth)
                    player.health = player.maxHealth;
                else
                    player.health += eventObj.heal;
            }
        }
    }
}
