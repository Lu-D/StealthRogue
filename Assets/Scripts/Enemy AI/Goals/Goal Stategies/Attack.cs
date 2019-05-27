using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack : CompositeGoal
{
    public Attack(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.buffered;

        removeAllSubgoals();

        owner.player.searchableArea.zeroRadius();

        owner.player.searchableArea.setIncreasing();

        owner.goalImpl.addSubgoals(this);
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

