using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class PatrolEventHandler : EventHandler
    {
        public override void handleLookAtMe(Vector3 lookPosition)
        {
            BEnemy enemy = GetComponent<BEnemy>();
            if (enemy.mainFSM.getCurrentState() == PatrolEnemyStates.PatrolWaypoint.Instance)
            {
                PatrolEnemyStates.LookAtMe.Instance.lookPosition = lookPosition;
                enemy.mainFSM.changeState(PatrolEnemyStates.LookAtMe.Instance);
            }
        }
    }
}
