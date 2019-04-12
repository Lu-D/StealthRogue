using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ExploreRoom : Goal
{
    private Pathfinding.PatrolLoop pathfinder;

    public ExploreRoom(BEnemy _owner) : base(_owner)
    {
        pathfinder = owner.GetComponent<Pathfinding.PatrolLoop>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        if (pathfinder != null)
        {
            pathfinder.setRoomWaypoints();
            pathfinder.enabled = true;
        }
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (pathfinder != null && pathfinder.timePassed > 20f)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
        if (pathfinder != null) pathfinder.enabled = false;
    }
}
