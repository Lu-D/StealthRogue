using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Attack : CompositeGoal
{
    public Attack(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.noInterrupt;

        removeAllSubgoals();

        owner.player.searchableArea.gettingHunted = true;

        pushSubgoalBack(Arbitrate());

    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        owner.player.searchableArea.zeroRadius();

        return status;
    }

    public override void Terminate()
    {
        owner.player.searchableArea.gettingHunted = false;
    }

    public Goal Arbitrate()
    {
        return new EaterCharge(owner);
    }
}

