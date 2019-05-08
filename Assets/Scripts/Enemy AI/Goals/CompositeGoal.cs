using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeGoal : Goal
{
//protected
    protected LinkedList<Goal> subgoalList;

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
                subgoalStatus = goalStatus.active;

            if (subgoalStatus == goalStatus.active && isBuffered())
                subgoalStatus = goalStatus.noInterrupt;

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

    public override void pushSubgoalFront(Goal g)
    {
        subgoalList.AddFirst(g);
    }

    public override void pushSubgoalBack(Goal g)
    {
        subgoalList.AddLast(g);
    }

    public void removeAllSubgoals()
    {
        foreach (Goal goal in subgoalList)
        {
            if (goal is CompositeGoal)
                (goal as CompositeGoal).removeAllSubgoals();

            goal.Terminate();
        }

        subgoalList.Clear();
    }

    public void forwardGoal(Goal passGoal)
    {
        if (subgoalList.First.Value is CompositeGoal)
        {
            CompositeGoal firstGoal = subgoalList.First.Value as CompositeGoal;
            firstGoal.forwardGoal(passGoal);
        }
        else
        {
            subgoalList.First.Value.Terminate();
            subgoalList.First.Value.SetInactive();
            pushSubgoalFront(passGoal);
        }
    }

    //for debugging purposes
    public Goal getFrontMostSubgoal()
    {
        if (subgoalList.First.Value is CompositeGoal)
        {
            CompositeGoal firstGoal = subgoalList.First.Value as CompositeGoal;
            return firstGoal.getFrontMostSubgoal();
        }
        else
            return subgoalList.First.Value;
    }

    public Goal getCurrentSubgoal()
    {
        if (subgoalList.Count != 0)
            return subgoalList.First.Value;
        else
            throw new System.Exception("Empty Subgoal List access attempted");
    }
    
    public abstract override void Activate();
    public abstract override goalStatus Process();
    public abstract override void Terminate();
}
