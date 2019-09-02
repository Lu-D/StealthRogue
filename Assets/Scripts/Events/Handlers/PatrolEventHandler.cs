using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class PatrolEventHandler : EventHandler
    {
        protected BEnemy enemy;

        private void Awake()
        {
            enemy = GetComponent<BEnemy>();
        }

        public override void handleEvent(lookAtMeEvent eventObj)
        {
            if (enemy.mainFSM.getCurrentState() == PatrolEnemyStates.PatrolWaypoint.Instance ||
            enemy.mainFSM.getCurrentState() == PatrolEnemyStates.LookAtMe.Instance)
            {
                PatrolEnemyStates.LookAtMe.Instance.lookPosition = eventObj.position;
                enemy.mainFSM.changeState(PatrolEnemyStates.LookAtMe.Instance);
            }
        }

        public override void handleEvent(DamageEvent eventObj)
        {
            if(enemy.mainFSM.getCurrentState() != PatrolEnemyStates.Die.Instance)
            {
                enemy.health -= eventObj.damage;
            }
        }
    }
}
