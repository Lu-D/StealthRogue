using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class GoalImpl
{
    public abstract void addSubgoals(Attack goal);
    public abstract void addSubgoals(Explore goal);
    public abstract void addSubgoals(Flee goal);
    public abstract void addSubgoals(Hunt goal);
}

