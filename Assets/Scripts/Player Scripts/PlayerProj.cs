using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProj : BProjectile
{
    // Start is called before the first frame update
    private new void Start()
    {
        //ignores collisions coming from player
        Collider2D sceneColl = source.transform.Find("SceneCollider").GetComponent<Collider2D>();
        Collider2D hurtbox = source.transform.Find("Hurtbox").GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), sceneColl);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), hurtbox);
    }

    // Update is called once per frame
    private new void Update()
    {

    }

    //collision method
    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        if(collision.gameObject.tag == "Enemy")
        {
            BEnemy enemy = collision.gameObject.GetComponent<BEnemy>();
            --enemy.health;
        }
    }
}
