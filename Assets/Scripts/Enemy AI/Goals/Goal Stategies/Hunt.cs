using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Hunt : CompositeGoal
{
    
    public Hunt(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.active;

        removeAllSubgoals();

        if (!owner.player.searchableArea.isIncreasing())
        {
            owner.player.searchableArea.zeroRadius();
            owner.player.searchableArea.setIncreasing();
        }

        owner.goalImpl.addSubgoals(this);
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        if (status == goalStatus.completed)
            Activate();

        return status;
    }

    public override void Terminate()
    {
        owner.player.searchableArea.reset();
    }
}
