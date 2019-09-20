using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<Vector3> spawnablePoints;
    private float desiredArea;
    private int resolution;

    private Vector3 min;
    private Vector3 max;

    private string path1 = "Enemies";
    private string path2 = "Obstacles";

    public GameObject[] enemyObjects;
    public GameObject[] obstacleObjects;

    public void Awake()
    {
        spawnablePoints = new List<Vector3>(); ;
        resolution = 15;

        Bounds roomBounds = GetComponent<Collider2D>().bounds;
        min = roomBounds.min;
        max = roomBounds.max;

        enemyObjects = Resources.LoadAll<GameObject>(path1);
        obstacleObjects = Resources.LoadAll<GameObject>(path2);

        createGraphPoints();

        spawnObjects(10, 5, 2, 8);
        //Debug.Log(SpawnableObject.objectCount["Enemies"]);
        //Debug.Log(SpawnableObject.objectCount["Obstacles"]);
    }

    public int Resolution
    {
        set { resolution = value; }
    }

    //replace spacing with actual spacing for each prefab
    public void spawnObjects(int objectCount, float spacing, int maxEnemies, int maxObstacles)
    {
        createGraphPoints();

        for (int i = 0; i < objectCount; ++i)
        {
            GameObject[] objectsToCreate = null;
            if (SpawnableObject.objectCount["Enemies"] > maxEnemies &&
                SpawnableObject.objectCount["Obstacles"] < maxObstacles)
                objectsToCreate = obstacleObjects;
            else if (SpawnableObject.objectCount["Obstacles"] > maxObstacles &&
                     SpawnableObject.objectCount["Enemies"] < maxEnemies)
                objectsToCreate = enemyObjects;
            else
            {
                int decideObjectType = UnityEngine.Random.Range(0, 2);
                if (decideObjectType == 0)
                    objectsToCreate = enemyObjects;
                else
                    objectsToCreate = obstacleObjects;
            }

            int decideObject = UnityEngine.Random.Range(0, objectsToCreate.Length);
            GameObject objectToCreate = objectsToCreate[decideObject];

            Bounds objectBounds = objectToCreate.GetComponent<Collider2D>().bounds;
            Vector3 objectExtents = objectBounds.extents;

            bool validPoint = false;
            Vector3 point = Vector3.zero;
            while (!validPoint)
            {
                point = findRandomPoint();
                if (point.x - objectExtents.x > min.x &&
                   point.y - objectExtents.y > min.y &&
                   point.x + objectExtents.x < max.x &&
                   point.y + objectExtents.y < max.y)
                    validPoint = true;
            }
            objectToCreate.GetComponent<SpawnableObject>().spawn(point);

            if (!removeWaypointsInProximity(point, spacing))
                break;
        }
    }

    private Vector3 findRandomPoint()
    {
        int randomNumTracker;

        randomNumTracker = UnityEngine.Random.Range(0, spawnablePoints.Count);
        Vector3 point = spawnablePoints[randomNumTracker];
        spawnablePoints.RemoveAt(randomNumTracker);

        return point;
    }

    private bool removeWaypointsInProximity(Vector3 point, float distance)
    {
        spawnablePoints.RemoveAll(item => Vector3.Distance(item, point) <= distance);
        if (spawnablePoints.Count == 0)
            return false;

        return true;
    }

    private void createGraphPoints()
    {
        float xStep = (max.x - min.x) / resolution;
        float yStep = (max.y - min.y) / resolution;

        for (float i = min.y + yStep; i < max.y; i += yStep)
        {
            for (float j = min.x + xStep; j < max.x; j += xStep)
            {
                spawnablePoints.Add(new Vector2(j, i));
            }
        }
    }
}

