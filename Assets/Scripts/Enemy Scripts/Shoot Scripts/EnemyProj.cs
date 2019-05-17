using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from base projectile
public class EnemyProj : BProjectile
{
    private Collider2D projCollider;

    //initialize fields
    protected new void Awake()
    {
        base.Awake();
        projCollider = GetComponent<Collider2D>();
    }

    //executes every frame
    protected new void Update()
    {
        //Deactivate collider when player is invincible
        if (GameObject.Find("Player").GetComponent<PlayerControl>().invincible)
        {
            projCollider.enabled = false;
        }
        else
        {
            projCollider.enabled = true;
        }
    }

    //collision method
    protected new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if(collision.gameObject.tag == "Player")
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            if(player.health != 0)
            {
                --player.health;
            }
        }
    }
}
