using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class ObstacleEventHandler : EventHandler
    {
        public override void handleEvent(DamageEvent eventObj)
        {
            Destroy(gameObject);
        }
    }
}
