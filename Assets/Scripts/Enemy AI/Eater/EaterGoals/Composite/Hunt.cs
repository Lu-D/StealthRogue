using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Hunt : CompositeGoal
{
    private bool startHunt = false;    

    public Hunt(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.active;

        removeAllSubgoals();

        if (!startHunt)
        {
            owner.player.searchableArea.zeroRadius();
            owner.player.searchableArea.gettingHunted = true;
            startHunt = true;
        }

        pushSubgoalBack(new TravelToSearchableArea(owner));
        pushSubgoalBack(new LookAround(owner));
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
        owner.player.searchableArea.resetRadius();
    }
}
