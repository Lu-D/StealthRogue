using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LookAround : Goal
{
    private LookSideToSide behavior;
    private Timer timer;
    
    public LookAround(BEnemy _owner) : base(_owner)
    {
        behavior = owner.GetComponent<LookSideToSide>();
        timer = new Timer();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        if (behavior != null) behavior.enabled = true;

        timer.startTimer();
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (timer.getElapsedTime() > 2f)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
        behavior.enabled = false;
        timer.endTimer();
    }
}

