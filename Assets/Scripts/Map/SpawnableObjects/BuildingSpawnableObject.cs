using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BuildingSpawnableObject : ObstacleSpawnableObject
{
    private GameObject[] barrelsBoxesObjects;
    private GameObject[] columnsObjects;

    private string path1 = "BarrelsBoxes";
    private string path2 = "Columns";

    Spawner buildingSpawner;

    public override void Awake()
    {
        base.Awake();

        barrelsBoxesObjects = Resources.LoadAll<GameObject>(path1);
        columnsObjects = Resources.LoadAll<GameObject>(path2);

        buildingSpawner = new Spawner(GetComponent<Collider2D>());
        buildingSpawner.loadObjects(barrelsBoxesObjects);
        buildingSpawner.loadObjects(columnsObjects);

        buildingSpawner.spawnObjects(5, 0.5f);
    }

    public override void spawn(Vector3 position)
    {
        Instantiate(gameObject, position, transform.rotation);
        ++objectCount["Obstacles"]; 
    }
}
