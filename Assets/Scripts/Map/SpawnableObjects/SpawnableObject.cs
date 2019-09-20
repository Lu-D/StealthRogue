using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpawnableObject : MonoBehaviour
{
    /*
     * --- Spawnable Object Categories ---
     * enemies
     * obstacles
     * misc
     * */
    public static Dictionary<string, int> objectCount = new Dictionary<string, int>();    

    public virtual void spawn(Vector3 position)
    {
        Instantiate(gameObject, position, transform.rotation);
        //objectCount.Add("misc", 1);
    }
}
