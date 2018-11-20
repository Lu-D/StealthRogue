using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovelControl : EquipmentController {

    public override void Awake () {
        base.Awake();
        equipType = (int)Equipment.shovel;
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
