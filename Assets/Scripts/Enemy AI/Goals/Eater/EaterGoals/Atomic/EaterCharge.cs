using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterCharge : Goal
{
    private EaterChargeTarget eaterChargeTarget;

    private float remainingDistance;

    public EaterCharge(BEnemy _owner) : base(_owner)
    {
        eaterChargeTarget = owner.GetComponent<EaterChargeTarget>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        eaterChargeTarget.target = owner.player.transform.position;
        eaterChargeTarget.enabled = true;

        remainingDistance = eaterChargeTarget.ai.remainingDistance;
    }

    public override goalStatus Process()
    {
        ActivateIfInactive();

        if (eaterChargeTarget.timer.getElapsedTime() > 3.5f)
            status = goalStatus.completed;

        return status;
    }

    public override void Terminate()
    {
       eaterChargeTarget.enabled = false;
    }
}
