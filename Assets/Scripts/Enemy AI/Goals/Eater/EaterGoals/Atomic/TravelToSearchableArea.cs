using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TravelToSearchableArea : Goal
{
    private EaterChasePlayer chasePlayer;

    public TravelToSearchableArea(BEnemy _owner) : base(_owner)
    {
        chasePlayer = owner.GetComponent<EaterChasePlayer>();
    }

    public override void Activate()
    {
        status = goalStatus.active;
        chasePlayer.enabled = true;
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (chasePlayer.targetReached)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
        chasePlayer.enabled = false;
    }
}

