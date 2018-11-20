using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attack Patterns class
//List of projectile attacks that gameobject can execute
public class AttackPatterns
{

    //shootgun()
    //Takes gun thats shooting, type of projectile being shot, number of bullets being shot, and cd for attack
    //Fire projectiles in 3 directions: -45 deg, 0 deg, and 45 deg
    //returns IEnumerator for coroutines
    public IEnumerator shootThree(GameObject gun, GameObject bullet, int bulletNum, float CD)
    {
        for (int i = 1; i <= bulletNum; ++i)
        {
            gun.GetComponent<GunControl>().Fire(bullet, -30);
            gun.GetComponent<GunControl>().Fire(bullet, 0);
            gun.GetComponent<GunControl>().Fire(bullet, 30);
            yield return new WaitForSeconds(.2f);
        }
        yield return new WaitForSeconds(CD);
    }

    //shootStraight()
    //Takes gun thats shooting, type of projectile being shot, number of bullets being shot, and cd for attack
    //Fires projectiles in a straight line
    //returns IEnumerator for coroutines
    public IEnumerator shootStraight(GameObject gun, GameObject bullet, int bulletNum, float CD)
    {
        for (int i = 0; i < bulletNum; ++i)
        {
            gun.GetComponent<GunControl>().Fire(bullet, 0);
            yield return new WaitForSeconds(.05f);
        }
        yield return new WaitForSeconds(CD);
    }

    //shootWave()
    //Takes gun thats shooting, type of projectile being shot, number of bullets being shot, and cd for attack
    //Shoots sinusoidal wave of projectiles
    //returns IEnumerator for coroutines
    public IEnumerator shootWave(GameObject gun, GameObject bullet, int bulletNum, float CD)
    {
        for (int j = -(5 * bulletNum); j <= 5 * bulletNum; j += 5)
        {
            gun.GetComponent<GunControl>().Fire(bullet, j);
            yield return new WaitForSeconds(.03f);
        }
        

        yield return new WaitForSeconds(CD);
    }
}
