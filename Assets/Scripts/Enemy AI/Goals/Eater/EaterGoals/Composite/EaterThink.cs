using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/*
 * Notes: Arbitrate function is fully responsible for terminating and switching strategies.
 * Only Hunt strategy loops b/c calling Terminate() messes with the player searchable area.
 * 
 * Arbitrate checks at every frame what is the best strategy and then selects it. Can sometimes make
 * dictating a sequence of strategies difficult so leave that for atomic goals
 * 
 * Each strategy needs an active status and a removeAllSubgoals() to ensure the goal structure is properly
 * navigated.
 * 
 * */
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
            SetInactive();
        else if (Arbitrate().GetType() != getCurrentSubgoal().GetType() && !isBuffered())
        {
            SetInactive();
        }

        //owner.enemyVision.logSeen();

        Debug.Log("Goal Status: " + getFrontMostSubgoal());
        return status;
    }

    public override void Terminate()
    {
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
