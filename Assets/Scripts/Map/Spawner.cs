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

    public void Awake()
    {
        spawnablePoints = new List<Vector3>(); ;
        resolution = 15;

        Debug.Log(spawnablePoints.Count);
        createGraphPoints();
        Debug.Log(spawnablePoints.Count);
    }

    public int Resolution
    {
        set { resolution = value; }
    }

    //replace spacing with actual spacing for each prefab
    public void spawnObjects(int vertices, float spacing, string mapLocation)
    {
        createGraphPoints();

        for (int i = 0; i < vertices; ++i)
        {
            Vector3 point = findRandomPoint();
            //Spawn something

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
        //Bounds roomBounds = mapContainer.maps[mapLocation].GetComponent<Map>().mapBounds;
        Bounds roomBounds = GetComponent<Collider2D>().bounds;

        Vector3 min = roomBounds.min;
        Vector3 max = roomBounds.max;
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

