using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentController : MonoBehaviour {
    //variables Equipments modify
    public float playerSpeedBonus = 0;
    protected GameObject player;
    protected PlayerControl pControl;
    public int equipType;

    public enum Equipment
    {
        //0 for no equip
        chronoshift = 1,
        laserSword,
        bomb,
        shovel,
        darkSword,
        map,
        box,
        trap
        //disguise
    };

    public abstract void onKeyDown();

    public abstract void onCollide(Collision2D collision);

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (pControl.equip != null && pControl.equip > 0)
            {
                spawnPrevEquip(pControl.equip);
            }
            pControl.equip = equipType;
            pControl.equipment = gameObject;
            transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            transform.gameObject.GetComponent<Collider2D>().enabled = false;
            transform.parent = player.transform;
        }
    }

    public void spawnPrevEquip(int equipType)
    {
        pControl.equip = 0;
        pControl.equipment.GetComponent<SpriteRenderer>().enabled = true;
       // pControl.equipment.transform.position = pControl.equipment.transform.position + new Vector3(0, (float)-0.2, 0);
        pControl.equipment.GetComponent<Collider2D>().enabled = true;
        pControl.equipment.transform.parent = null;
        pControl.equipment = null;
    }

}
