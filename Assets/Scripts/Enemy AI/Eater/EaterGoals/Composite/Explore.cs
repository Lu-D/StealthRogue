using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Explore : CompositeGoal
{
    public Explore(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.active;

        removeAllSubgoals();

        pushSubgoalBack(new NavToRoom(owner));
        pushSubgoalBack(new ExploreRoom(owner));
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        return status;
    }

    public override void Terminate()
    {
        
    }
}