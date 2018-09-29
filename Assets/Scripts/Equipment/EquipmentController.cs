using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentController : MonoBehaviour {
    //variables Equipments modify
    public float playerSpeedBonus = 0;
    protected GameObject player;
    protected PlayerControl pControl;

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

    public abstract void OnTriggerEnter2D(Collider2D collision);


}
