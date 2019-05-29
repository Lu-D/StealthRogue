using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//On actual gun gameObject to fire projectile
public class GunControl : MonoBehaviour
{
    private Transform gunBack;
    private Transform gunFront;
    private Transform shooter;
    public float coolDown;
    private float maxCoolDown;
    public GameObject bullet;


    // Use this for initialization
    void Awake()
    {
        gunBack = gameObject.transform.GetChild(0);
        gunFront = gameObject.transform.GetChild(1);
        shooter = gameObject.transform.parent;
        maxCoolDown = coolDown;
    }

    //Fire()
    //fires projectile in the foward direction
    public void Fire(GameObject proj, float angle, float dmg)
    {
        transform.Rotate(0, 0, angle, Space.Self);

        //fire Projectile
        GameObject projectile = Instantiate(bullet, gunFront.position, transform.rotation);

        projectile.GetComponent<BProjectile>().damage = dmg;

        projectile.GetComponent<Rigidbody2D>().velocity = (gunFront.position - gunBack.position).normalized * projectile.GetComponent<BProjectile>().projSpeed;
        coolDown = maxCoolDown;
        transform.Rotate(0, 0, -angle, Space.Self);

    }
}
