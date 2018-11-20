using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombControl : EquipmentController {
    public int bombCount;
    public GameObject bomb;

    public override void Awake()
    {
        base.Awake();
        equipType = (int)Equipment.bomb;
        bombCount = 3;
    }

    //equip
    public override void onKeyDown()
    {
        if(bombCount > 0)
        {
            GameObject projectile = (GameObject)Instantiate(bomb, player.transform.position, player.transform.rotation);
            --bombCount;
        }
    }

    public override void onCollide(Collision2D collision)
    {

    }
}
