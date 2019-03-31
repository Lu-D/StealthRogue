using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterThinkG : CompositeGoal
{
    public EaterThinkG(BEnemy _owner) : base(_owner)
    {
        
    }

    public override void Activate()
    {   
        status = goalStatus.active;

        if (owner.mapLocation != owner.player.mapLocation)
            addSubgoal(new EaterNavToRoomG(owner));
        else if(owner.mapLocation != "")
            addSubgoal(new EaterPatrolRoomG(owner));
    }


    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        owner.BupdateAnim();

        if (isCompleted())
            status = goalStatus.inactive;

        return status;
    }

    public override void Terminate()
    {
        throw new NotImplementedException();
    }
}
