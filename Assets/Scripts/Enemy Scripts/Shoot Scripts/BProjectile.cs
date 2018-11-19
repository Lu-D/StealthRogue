using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BProjectile
//base class for projectiles
public class BProjectile : MonoBehaviour
{
    public float projSpeed;
    public int damage;
    public GameObject source;

    //initialize fields
    public virtual void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), source.GetComponent<Collider2D>());
    }

    //execute every frame
    public void Update()
    {

    }

    //collision methods
    //despawns bullets which collide with out of camera border
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy projectile when colliding with game border
        if (collision.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }
        //Destroy projectile when colliding with game border
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        //Destroy projectile when colliding with game border
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
        //Destroy projectile when colliding with obstacle
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }
}