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

    public virtual void Awake()
    {
        if (!objectCount.ContainsKey("Enemies"))
            objectCount.Add("Enemies", 0);
        if (!objectCount.ContainsKey("Obstacles"))
            objectCount.Add("Obstacles", 0);
        if (!objectCount.ContainsKey("Misc"))
            objectCount.Add("Misc", 0);
    }

    public virtual void spawn(Vector3 position)
    {
        Instantiate(gameObject, position, transform.rotation);
        ++objectCount["Misc"];
    }
}
