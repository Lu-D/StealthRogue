using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class ComeToMeEvent : Event
    {
        private Vector3 comePos;
        public Vector3 position
        {
            get { return comePos; }
        }

        public delegate void comeToMeAction(ComeToMeEvent comeToMe);
        public event comeToMeAction onComeToMe;

        public ComeToMeEvent(Vector3 pos)
        {
            comePos = pos;
        }

        public override void addListener(GameObject gameObject)
        {
            var eventHandler = gameObject.GetComponent<Events.EventHandler>();
            if (eventHandler != null)
                onComeToMe += eventHandler.handleEvent;
        }

        public override void execute()
        {
            if (onComeToMe != null)
                onComeToMe(this);
        }
    }
}
