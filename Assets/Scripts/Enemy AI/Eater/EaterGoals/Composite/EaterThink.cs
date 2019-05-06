using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterThink : CompositeGoal
{ 
    public EaterThink(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.active;

        removeAllSubgoals();

        pushSubgoalBack(Arbitrate());
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();
        status = processSubgoals();

        if (status == goalStatus.completed)
            Activate();
        else if (Arbitrate().GetType() != getCurrentSubgoal().GetType() && !isBuffered())
            Activate();

        //owner.enemyVision.logSeen();

        Debug.Log("Goal Status: " + getFrontMostSubgoal());
        return status;
    }

    public override void Terminate()
    {
        removeAllSubgoals();
    }

    public Goal Arbitrate() 
    {
        if (owner.enemyVision.hasSeen("Player"))
            return new Attack(owner);
        else if (owner.player.searchableArea.radius > 6f)
            return new Explore(owner);
        else
            return new Hunt(owner);
    }

}
