using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GeneralSpawner : MonoBehaviour
{
    public GameObject[] enemyObjects;
    public GameObject[] obstacleObjects;

    private string path1 = "Enemies";
    private string path2 = "Obstacles";

    private Spawner mainSpawner;

    public void Awake()
    {
        enemyObjects = Resources.LoadAll<GameObject>(path1);
        obstacleObjects = Resources.LoadAll<GameObject>(path2);

        mainSpawner = new Spawner(GetComponent<Collider2D>());

        mainSpawner.loadObjects(enemyObjects);
        mainSpawner.loadObjects(obstacleObjects);
        mainSpawner.spawnObjects(10, 5);
    }
}

