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
        Debug.Log("reactivated");
        status = goalStatus.active;
        removeAllSubgoals();

        Goal bestGoal = Arbitrate();

        pushSubgoalFront(bestGoal);
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        if (status == goalStatus.completed)
            status = goalStatus.inactive;

        Debug.Log("Goal Status: " + getFrontMostSubgoal());
        return status;
    }

    public override void Terminate()
    {
        removeAllSubgoals();
    }

    public Goal Arbitrate() 
    {
        return new Explore(owner);
    }

}
