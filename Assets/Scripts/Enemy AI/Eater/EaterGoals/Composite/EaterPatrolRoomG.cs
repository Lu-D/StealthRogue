using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterPatrolRoomG : CompositeGoal
{
    private Pathfinding.PatrolLoop pathfinder;

    public EaterPatrolRoomG(BEnemy _owner) : base(_owner)
    {
        pathfinder = owner.GetComponent<Pathfinding.PatrolLoop>();
    }

    public override void Activate()
    {
        addSubgoal(new EaterChargeG(owner));
        addSubgoal(new EaterSetRoomWaypointsG(owner));
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        if (owner.mapLocation != owner.player.mapLocation)
        {
            removeAllSubgoals();
            status = goalStatus.completed;
        } 
        else if(isCompleted())
        {
            removeAllSubgoals();
            status = goalStatus.inactive;
        }

        return status;
    }

    public override void Terminate()
    {
        if (pathfinder != null) pathfinder.enabled = false;
    }
}