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

    }

    //execute every frame
    public void Update()
    {

    }

    //collision methods
    //despawns bullets which collide with out of camera border
    //public virtual void OnCollisionEnter2D(Collision2D collision)
    //{
    //    //Destroy projectile when colliding with game border
    //    if (collision.gameObject.tag == "Border")
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}