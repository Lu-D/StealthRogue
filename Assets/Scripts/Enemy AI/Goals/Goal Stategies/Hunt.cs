using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Hunt : CompositeGoal
{
    private SoundManager soundManager;
    
    public Hunt(BEnemy _owner) : base(_owner)
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    public override void Activate()
    {
        status = goalStatus.active;

        removeAllSubgoals();

        if (!owner.player.searchableArea.isIncreasing())
        {
            owner.player.searchableArea.zeroRadius();
            owner.player.searchableArea.setIncreasing();
        }

        soundManager.Pause("Environment_Ambience");

        if(!soundManager.isPlaying("Chase_Theme_Loop"))
            soundManager.FadeIn("Chase_Theme_Loop", 1f, true);

        owner.goalImpl.addSubgoals(this);
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        if (status == goalStatus.completed)
            Activate();

        return status;
    }

    public override void Terminate()
    {
        owner.player.searchableArea.reset();

        soundManager.Play("Environment_Ambience");
    }
}
