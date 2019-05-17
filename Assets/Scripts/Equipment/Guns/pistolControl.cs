using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistolControl : EquipmentController
{
    public int bullets;
    public Task fireOneShot;

    public override void Awake()
    {
        base.Awake();
        equipType = (int)Equipment.pistol;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //equip
    public override void onKeyDown()
    {
        if (bullets > 0 && (fireOneShot == null || !fireOneShot.Running))
        {
            fireOneShot = new Task(pControl.attackPatterns.shootStraight(pControl.gun.gameObject, pControl.bullet, 1, 1f));
            --bullets;
            pControl.myAudioSource.PlayOneShot(pControl.audioClips[3]);
        }
    }

    public override void onCollide(Collision2D collision)
    {
    }
}
