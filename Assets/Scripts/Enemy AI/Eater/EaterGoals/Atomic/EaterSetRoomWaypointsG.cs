using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterSetRoomWaypointsG : Goal
{
    private Pathfinding.PatrolLoop pathfinder;

    public EaterSetRoomWaypointsG(BEnemy _owner) : base(_owner)
    {
        pathfinder = owner.GetComponent<Pathfinding.PatrolLoop>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        if (pathfinder != null)
        {
            pathfinder.setRoomWaypoints();
            if (!pathfinder.enabled) pathfinder.enabled = true;
        }
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (checkCanChargePlayer())
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
        if (pathfinder != null) pathfinder.enabled = false;
    }

    private bool checkCanChargePlayer()
    {
        LayerMask viewCastLayer = ~(1 << LayerMask.NameToLayer("Enemy"));
        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, owner.player.transform.position - owner.transform.position, 4, viewCastLayer);

        if (hit.collider != null && hit.collider.tag == "Player") return true;
        else return false;
    }
}
