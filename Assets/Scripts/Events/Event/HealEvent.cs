using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Events
{
public class HealEvent : Event
{
    public delegate void HealAction(HealEvent obj);
    public event HealAction onHeal;

    public int heal;

    public HealEvent(int healNumber)
    {
        heal = healNumber;
    }

    public override void execute()
    {
        if (onHeal != null)
            onHeal(this);
    }

    public override void addListener(GameObject gameObject)
    {
        var eventHandler = gameObject.GetComponent<Events.EventHandler>();
        if (eventHandler != null)
            onHeal += eventHandler.handleEvent;
    }
}
}

