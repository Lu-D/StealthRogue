using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapControl : MonoBehaviour {
    public int numEnemies;

    void Awake()
    {
        Transform[] spawnPoints;
        spawnPoints = GetComponentsInChildren<Transform>();

        foreach(Transform point in spawnPoints)
        {
            GameObject instance = Instantiate(Resources.Load("Red Dot", typeof(GameObject))) as GameObject;
            instance.transform.position = point.position;
            instance.transform.rotation = point.rotation;
        }
    }
}
