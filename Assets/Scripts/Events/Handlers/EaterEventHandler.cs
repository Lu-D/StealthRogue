using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class EaterEventHandler : EventHandler
    {
        protected Eater enemy;

        private void Awake()
        {
            enemy = GetComponent<Eater>();
        }

        public override void handleEvent(lookAtMeEvent eventObj)
        {
            //enemy.mainGoal.forwardGoal(new HandleLookAtMe(enemy, eventObj));
        }

        public override void handleEvent(DamageEvent eventObj)
        {
            if (enemy.health > 0)
                --enemy.health;
        }
    }
}
