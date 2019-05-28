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
    protected virtual void Awake()
    {
    }

    //execute every frame
    protected virtual void Update()
    {
    }

    //collision methods
    //despawns bullets which collide with out of camera border
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy projectile when colliding with game border
        if (collision.gameObject.tag == "Border" ||
        collision.gameObject.tag == "Player" ||
        collision.gameObject.tag == "Enemy" ||
        collision.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }
}