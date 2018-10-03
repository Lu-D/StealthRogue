using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovelControl : EquipmentController {

    void Awake () {
        equipType = (int)Equipment.shovel;
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerControl>();
    }

    public override void onKeyDown()
    {

    }

    public override void onCollide(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(collision.gameObject);
        }
    }
}
