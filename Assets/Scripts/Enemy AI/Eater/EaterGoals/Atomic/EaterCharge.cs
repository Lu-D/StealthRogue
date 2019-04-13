using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterCharge : Goal
{
    private EaterChargeTarget eaterChargeTarget;

    public EaterCharge(BEnemy _owner) : base(_owner)
    {
        eaterChargeTarget = owner.GetComponent<EaterChargeTarget>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        eaterChargeTarget.target = owner.player.transform;
        eaterChargeTarget.enabled = true;
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (eaterChargeTarget.timePassed > 4f)
        {
            status = goalStatus.completed;
        }

        return status;
    }

    public override void Terminate()
    {
       eaterChargeTarget.enabled = false;
    }
}
