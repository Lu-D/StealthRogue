using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD:Assets/Scripts/Enemy Scripts/Shoot Scripts/BProjectile.cs
//Base projectile class 
=======
//BProjectile
//base class for projectiles
>>>>>>> 90293a96f79c3b943260d56de07427b1dc5a209c:Assets/Scripts/BProjectile.cs
public class BProjectile : MonoBehaviour
{
    public float projSpeed;
    public int damage;

<<<<<<< HEAD:Assets/Scripts/Enemy Scripts/Shoot Scripts/BProjectile.cs
    //initialize fields
    public virtual void Start()
    {

    }

    //execute every frame
    public void Update()
    {

    }

    //collision methods
=======
    //OnCollisionEnter2D 
    //despawns bullets which collide with out of camera border
>>>>>>> 90293a96f79c3b943260d56de07427b1dc5a209c:Assets/Scripts/BProjectile.cs
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy projectile when colliding with game border
        if (collision.gameObject.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }
}