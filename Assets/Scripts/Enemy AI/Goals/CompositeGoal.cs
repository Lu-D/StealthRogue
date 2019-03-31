using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeGoal : Goal
{
//protected
    LinkedList<Goal> subgoalList;

    protected goalStatus processSubgoals()
    {
        while (subgoalList.Count > 0 &&
              (subgoalList.First.Value.hasFailed() || subgoalList.First.Value.isCompleted()))
        {
            subgoalList.First.Value.Terminate();
            subgoalList.RemoveFirst();
        }

        if (subgoalList.Count != 0)
        {

            goalStatus subgoalStatus = subgoalList.First.Value.Process();

            if (subgoalStatus == goalStatus.completed && subgoalList.Count > 1)
            {
                return goalStatus.active;
            }

            return subgoalStatus;
        }
        else
            return goalStatus.completed;
    }

    //public
    public CompositeGoal(BEnemy _owner) : base(_owner)
    {
        subgoalList = new LinkedList<Goal>();
    }

    public override void addSubgoal(Goal g)
    {
        subgoalList.AddFirst(g);
    }

    public void removeAllSubgoals()
    {
        foreach(Goal goal in subgoalList)
        {
            goal.Terminate();
        }

        subgoalList.Clear();
    }
    
    public abstract override void Activate();
    public abstract override goalStatus Process();
    public abstract override void Terminate();
}
