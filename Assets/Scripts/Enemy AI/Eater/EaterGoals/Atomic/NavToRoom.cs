using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class NavToRoom : Goal
{
    private NavigateToRoom navigateToRoom;

    public NavToRoom(BEnemy _owner) : base(_owner)
    {
        navigateToRoom = owner.GetComponent<NavigateToRoom>();
    }

    public override void Activate()
    {
        status = goalStatus.active;
        if (navigateToRoom != null) navigateToRoom.enabled = true;
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (navigateToRoom != null && navigateToRoom.roomReached)
            status = goalStatus.completed;       

        return status;
    }

    public override void Terminate()
    {
        if (navigateToRoom != null) navigateToRoom.enabled = false;
    }
}

