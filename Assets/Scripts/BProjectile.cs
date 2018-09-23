using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BProjectile
//base class for projectiles
public class BProjectile : MonoBehaviour
{
    public float projSpeed;
    public int damage;

    //OnCollisionEnter2D 
    //despawns bullets which collide with out of camera border
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }
}