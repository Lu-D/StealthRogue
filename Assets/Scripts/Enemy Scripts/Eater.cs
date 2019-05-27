using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SoundStates;

public class Eater : BEnemy{
    
    public CompositeGoal mainGoal;

    protected override void myAwake()
    {
        mainGoal = new EaterThink(this);
        goalImpl = new EaterGoalImpl(this);
        health = 50;
    }

    private void Start()
    {
        soundFSM.changeState(EnvironmentalCrickets.Instance);
    }

    private void Update()
    {
        mainGoal.Process();

        soundFSM.stateUpdate();
    }
}
