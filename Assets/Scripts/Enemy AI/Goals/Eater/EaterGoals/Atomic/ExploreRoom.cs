using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ExploreRoom : Goal
{
    private Pathfinding.PatrolLoop pathfinder;
    private Timer timer;

    public ExploreRoom(BEnemy _owner) : base(_owner)
    {
        pathfinder = owner.GetComponent<Pathfinding.PatrolLoop>();
        timer = new Timer();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        if (pathfinder != null)
        {
            pathfinder.setRoomWaypoints();
            pathfinder.enabled = true;
        }

        timer.startTimer();
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (timer.getElapsedTime() > 20f)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
        timer.endTimer();
        if (pathfinder != null) pathfinder.enabled = false;
    }
}
