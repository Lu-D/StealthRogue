using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EaterGoalImpl : GoalImpl
{
    Eater owner;

    public EaterGoalImpl(Eater _owner)
    {
        owner = _owner;
    }
    
    public override void addSubgoals(Attack goal)
    {
        goal.pushSubgoalBack(new EaterCharge(owner));
    }

    public override void addSubgoals(Explore goal)
    {
        goal.pushSubgoalBack(new NavToRoom(owner));
        goal.pushSubgoalBack(new ExploreRoom(owner));
    }

    public override void addSubgoals(Flee goal)
    {
        throw new NotImplementedException();
    }

    public override void addSubgoals(Hunt goal)
    {
        goal.pushSubgoalBack(new TravelToSearchableArea(owner));
        goal.pushSubgoalBack(new LookAround(owner));
    }
}

