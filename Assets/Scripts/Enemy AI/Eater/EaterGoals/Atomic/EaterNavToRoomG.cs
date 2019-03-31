using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterNavToRoomG : Goal
{
    private NavigateToRoom navigateToRoom;

    public EaterNavToRoomG(BEnemy _owner) : base(_owner)
    {
        navigateToRoom = owner.GetComponent<NavigateToRoom>();
    }

    public override void Activate()
    {
        if (navigateToRoom != null) navigateToRoom.enabled = true;
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (navigateToRoom != null && navigateToRoom.roomReached)
            status = goalStatus.completed;
        else if (owner.mapLocation == owner.player.mapLocation)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
        if(navigateToRoom != null) navigateToRoom.enabled = false;
    }
}

