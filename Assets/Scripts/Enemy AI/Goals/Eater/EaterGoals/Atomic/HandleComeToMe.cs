using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;
using UnityEngine;

public class HandleComeToMe : Goal
{
    ComeToMeEvent eventObj;

    Pathfinding.IAstarAI ai;

    public HandleComeToMe(BEnemy _owner, ComeToMeEvent _eventObj) : base(_owner)
    {
        eventObj = _eventObj;
        ai = owner.GetComponent<Pathfinding.IAstarAI>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        ai.isStopped = false;
        ai.destination = eventObj.position;
        ai.SearchPath();
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (ai.reachedEndOfPath)
            status = goalStatus.completed; 

        return status;
    }

    public override void Terminate()
    {
        ai.isStopped = true;
    }
}
