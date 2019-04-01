using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterChargeG : Goal
{
    private EaterChargeTarget eaterChargeTarget;

    public EaterChargeG(BEnemy _owner) : base(_owner)
    {
        eaterChargeTarget = owner.GetComponent<EaterChargeTarget>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        if (eaterChargeTarget != null)
        {
            eaterChargeTarget.target = owner.player.transform;
            eaterChargeTarget.enabled = true;
        }
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (eaterChargeTarget != null &&
        eaterChargeTarget.timePassed > 4f)
        {
            if (eaterChargeTarget.canHitTarget)
            {
                Reactivate();
            }
            else
                status = goalStatus.completed;
        }

        return status;
    }

    public override void Terminate()
    {
        if(eaterChargeTarget != null) eaterChargeTarget.enabled = false;
    }
}
