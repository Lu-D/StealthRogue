using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NavigateRoom : CompositeGoal
{
    public NavigateRoom(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.active;
        owner.waypointNav.setRoomWaypoints();
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();
        owner.waypointNav.navLoop();
        owner.BupdateAnim();
        return status;
    }

    public override void Terminate()
    {
        
    }
}