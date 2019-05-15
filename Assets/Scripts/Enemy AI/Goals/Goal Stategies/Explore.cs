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

        owner.goalImpl.addSubgoals(this);

        owner.player.searchableArea.setDecreasing();
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