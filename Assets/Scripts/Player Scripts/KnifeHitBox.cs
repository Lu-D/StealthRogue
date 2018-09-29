using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitBox : MonoBehaviour {
    private PlayerControl pControl;

    void Awake()
    {
        pControl = transform.parent.gameObject.GetComponent<PlayerControl>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        switch (pControl.equip)
        {
            case 2:
                //laserSword
                if (collision.gameObject.tag == "Projectile")
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = -collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                }
                return;
            case 4:
                //shovel
                if (collision.gameObject.tag == "Obstacle")
                {
                    Destroy(collision.gameObject);
                }
                return;
            case 5:
                //darkSword
                if (collision.gameObject.tag == "Projectile")
                {
                    Destroy(collision.gameObject);
                    pControl.capturedBullet = true;
                }
                return;
        }
    }
}
