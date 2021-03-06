﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*
 * Responsible for implementation of how events should be handled. Every event possible should
 * have its own overloaded virtual function that throws if called and not implemented. These functions are added
 * the event delegate. See bomb for example.
 * 
 * Concrete handler implements all functions that correspond to the events it should be listening to.
 * */
namespace Events
{
    public class EventHandler : MonoBehaviour
    {
        public virtual void handleEvent(lookAtMeEvent eventObj) { throw new NotImplementedException();  }
        public virtual void handleEvent(DamageEvent eventObj) { throw new NotImplementedException(); }
        public virtual void handleEvent(ComeToMeEvent eventObj) { throw new NotImplementedException(); }
        public virtual void handleEvent(HealEvent eventObj) { throw new NotImplementedException(); }
    }
}
