using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Events;

public class HandleLookAtMe : Goal
{
    lookAtMeEvent eventObj;
    Task rotateOneShot;
    
    public HandleLookAtMe(BEnemy _owner, lookAtMeEvent _eventObj) : base(_owner)
    {
        eventObj = _eventObj;
    }

    public override void Activate()
    {
        status = goalStatus.active;
        rotateOneShot = new Task(owner.RotateTo(eventObj.position, 5f));
        
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (!rotateOneShot.Running)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
    }
}

