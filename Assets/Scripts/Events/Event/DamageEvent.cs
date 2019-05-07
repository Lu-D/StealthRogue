using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
    public class DamageEvent : Event
    {

        public delegate void DamageAction(DamageEvent die);
        public event DamageAction onDamage;

        public DamageEvent()
        {
            priority = Priority.P100;
        }

        public override void execute()
        {
            if(onDamage != null)
                onDamage(this);
        }

        public override void addListener(GameObject gameObject)
        {
            var eventHandler = gameObject.GetComponent<Events.EventHandler>();
            if (eventHandler != null)
                onDamage += eventHandler.handleEvent;
        }
    }
}
