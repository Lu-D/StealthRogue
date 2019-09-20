using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class EnemySpawnableObject : SpawnableObject
{
    public override void spawn(Vector3 position)
    {
        Instantiate(gameObject, position, transform.rotation);
        ++objectCount["Enemies"];
    }
}

