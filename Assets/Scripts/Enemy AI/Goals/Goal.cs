using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Goal
{
    public enum goalStatus
    {
        inactive = 0,
        active,
        completed,
        failed
    }

//protected
    protected goalStatus status;
    protected BEnemy owner;

    protected void ActivateIfInactive()
    {
        if (isInactive())
            Activate();
    }

    protected void ReactivateIfFailed()
    {
        if (hasFailed())
            status = goalStatus.inactive; 

    }

    public virtual bool HandleMessage()
    {
        return false;
    }
//public
    public Goal(BEnemy _owner){
        owner = _owner;
        status = goalStatus.inactive;
    }

    public virtual void addSubgoal(Goal g){
        throw new MethodAccessException("Cannot add goals to atomic goals");
    }

    public bool isCompleted(){return status == goalStatus.completed;}
    public bool isActive(){return status == goalStatus.active;}
    public bool isInactive(){return status == goalStatus.inactive;}
    public bool hasFailed(){return status == goalStatus.failed;}

    public void Reactivate(){
        Terminate();
        Activate();
    }

    public abstract void Activate();
    public abstract goalStatus Process();
    public abstract void Terminate();
}
