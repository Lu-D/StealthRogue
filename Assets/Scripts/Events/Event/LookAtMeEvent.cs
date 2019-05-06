using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class lookAtMe : Event
    {
        private Vector3 lookPos;

        public delegate void lookAtMeAction(Vector3 lookPosition);
        public event lookAtMeAction onLookAtMe;
        
        public lookAtMe(Vector3 pos)
        {
            lookPos = pos;
        }

        public override void execute()
        {
            onLookAtMe(lookPos);
        }
    }
}
