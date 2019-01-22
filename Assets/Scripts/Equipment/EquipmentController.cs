using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentController : MonoBehaviour {
    //variables Equipments modify
    public float playerSpeedBonus = 0;
    protected GameObject player;
    protected PlayerControl pControl;
    public int equipType;

    public virtual void Awake()
    {
        player = GameObject.Find("Player");
        pControl = player.GetComponent<PlayerControl>();

        //ignores collisions coming from player
        Collider2D sceneColl = player.transform.Find("SceneCollider").GetComponent<Collider2D>();
        Collider2D hurtbox = player.transform.Find("Hurtbox").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), sceneColl);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), hurtbox);
    }

    public enum Equipment
    {
        //0 for no equip
        chronoshift = 1,
        laserSword,
        bomb,
        shovel,
        darkSword,
        map,
        pistol,
        shotgun,
        rifle
    };

    public abstract void onKeyDown();

    public abstract void onCollide(Collision2D collision);

    public void OnTriggerStay2D(Collider2D collision)
    {
        //only picks up item if player doesnt have one
        if (collision.gameObject.tag == "Player" 
            && pControl.equip == 0 
            && GetComponent<Rigidbody2D>().velocity.magnitude < .3f)
        {
            Debug.Log("picking i[");
            pControl.equip = equipType;
            pControl.equipment = gameObject;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        if (GetComponent<Rigidbody2D>().velocity.magnitude > 1f 
            && collision.gameObject.tag == "Enemy")
        {
            Debug.Log("success");

            collision.gameObject.GetComponent<EnemyControl>().health = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
    }

    public void throwEquip(int equipType)
    {
        pControl.equip = 0;

        GameObject equipment = pControl.equipment;

        //turns on renderer for equipment
        equipment.GetComponent<SpriteRenderer>().enabled = true;

        GetComponent<Collider2D>().enabled = true;

        //Throws equipment towards mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(player.transform.position);
        mousePos = mousePos - objectPos;

        equipment.transform.position = player.transform.position;
        equipment.GetComponent<Rigidbody2D>().AddForce(mousePos.normalized * 450);

        //detatches equipment from player
        pControl.equipment = null;
    }

}
