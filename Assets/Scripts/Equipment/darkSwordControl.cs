using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkSwordControl : EquipmentController {

    void Awake()
    {
        equipType = (int)Equipment.darkSword;
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerControl>();
    }

    //equip
    public override void onKeyDown()
    {
        pControl.gun.GetComponent<GunControl>().Fire(pControl.bullet, 0);
        pControl.capturedBullet = false;
    }

    public override void onCollide(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            pControl.capturedBullet = true;
        }
    }
}
