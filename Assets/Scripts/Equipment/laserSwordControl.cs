﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserSwordControl : EquipmentController {

    public override void Awake()
    {
        base.Awake();
        equipType = (int)Equipment.laserSword;
    }

    //equip
    public override void onKeyDown()
    {

    }

    public override void onCollide(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            //equipment.GetComponent<
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            BProjectile bulletControl = collision.gameObject.GetComponent<BProjectile>();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = (bulletControl.source.transform.position - transform.position).normalized * bulletControl.projSpeed;
            bulletControl.source = transform.gameObject;
        }
    }
}
                                                   