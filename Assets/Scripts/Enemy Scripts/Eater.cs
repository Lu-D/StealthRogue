using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eater : BEnemy{
    
    Goal testGoal;

    protected override void myAwake()
    {
        testGoal = new EaterThink(this);
    }

    public void Update()
    {
        testGoal.Process();
    }
}
