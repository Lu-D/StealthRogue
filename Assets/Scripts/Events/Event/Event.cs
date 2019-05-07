using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
 * Abstract interface that allows for events to be treated uniformly in EventManager
 * Makes use of Unity delegates and events
 * 
 * Use: Object interested in broadcasting an event is responsible for creating concrete 
 * event, adding listeners, and adding event to queue in Event Manager. See example in bomb.
 * 
 * Any object that is a listener is expected to have a concrete event handler. See event 
 * handler on how to implement.
 **/
namespace Events
{
    public abstract class Event
    {
        public Priority priority;
        public abstract void execute();

        //ensure event handler is not null
        public abstract void addListener(GameObject gameObject);
    }
}
