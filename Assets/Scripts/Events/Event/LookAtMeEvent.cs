using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class lookAtMeEvent : Event
    {
        private Vector3 lookPos;
        public Vector3 position{
            get { return lookPos; }
        }

        public delegate void lookAtMeAction(lookAtMeEvent lookAtMe);
        public event lookAtMeAction onLookAtMe;
        
        public lookAtMeEvent(Vector3 pos)
        {
            lookPos = pos;
            priority = Priority.P1;
        }

        public override void execute()
        {
            if(onLookAtMe != null)
                onLookAtMe(this);
        }

        public override void addListener(GameObject gameObject)
        {
            var eventHandler = gameObject.GetComponent<Events.EventHandler>();
            if (eventHandler != null)
                onLookAtMe += eventHandler.handleEvent;
        }
    }
}
