using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class DieEvent : Event
    {

        public delegate void DieAction(DieEvent die);
        public event DieAction onDie;

        public DieEvent()
        {
            priority = Priority.P100;
        }

        public override void execute()
        {
            if(onDie != null)
                onDie(this);
        }

        public override void addListener(GameObject gameObject)
        {
            var eventHandler = gameObject.GetComponent<Events.EventHandler>();
            if (eventHandler != null)
                onDie += eventHandler.handleEvent;
        }
    }
}
