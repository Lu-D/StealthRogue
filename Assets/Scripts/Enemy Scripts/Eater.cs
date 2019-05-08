using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eater : BEnemy{
    
    public CompositeGoal mainGoal;

    protected override void myAwake()
    {
        mainGoal = new EaterThink(this);
        health = 100;
    }

    private void Update()
    {
        mainGoal.Process();
    }
}
