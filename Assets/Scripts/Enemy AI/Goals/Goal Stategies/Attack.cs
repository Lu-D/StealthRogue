using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Attack : CompositeGoal
{
    private SoundManager soundManager;

    public Attack(BEnemy _owner) : base(_owner)
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    public override void Activate()
    {
        status = goalStatus.buffered;

        removeAllSubgoals();

        owner.player.searchableArea.zeroRadius();

        owner.player.searchableArea.setIncreasing();

        owner.goalImpl.addSubgoals(this);

        soundManager.Pause("Environment_Ambience");

        if (!soundManager.isPlaying("Chase_Theme_Loop"))
            soundManager.FadeIn("Chase_Theme_Loop", 1f, true);
    }
    public override goalStatus Process()
    {
        ActivateIfInactive();

        status = processSubgoals();

        return status;
    }

    public override void Terminate()
    {
    }
}

