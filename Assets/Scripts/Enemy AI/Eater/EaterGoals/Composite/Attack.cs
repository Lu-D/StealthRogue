using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Attack : CompositeGoal
{
    public Attack(BEnemy _owner) : base(_owner)
    {
    }

    public override void Activate()
    {
        status = goalStatus.active;

        removeAllSubgoals();

        throw new NotImplementedException();
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        return status;
    }

    public override void Terminate()
    {
        throw new NotImplementedException();
    }
}

