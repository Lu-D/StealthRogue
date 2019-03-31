using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterPatrolRoomG : CompositeGoal
{
    private Pathfinding.PatrolLoop pathfinder;

    public EaterPatrolRoomG(BEnemy _owner) : base(_owner)
    {
        pathfinder = owner.GetComponent<Pathfinding.PatrolLoop>();
    }

    public override void Activate()
    {
        if (checkCanChargePlayer())
            addSubgoal(new EaterChargeG(owner));
        else
            addSubgoal(new EaterSetRoomWaypointsG(owner));
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        if (owner.mapLocation != owner.player.mapLocation)
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