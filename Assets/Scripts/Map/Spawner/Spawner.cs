using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Spawner
{
    private List<Vector3> spawnablePoints;
    private float desiredArea;
    private int resolution = 15;

    private Vector3 min;
    private Vector3 max;

    private List<GameObject[]> spawnableObjects;

    public Spawner(Collider2D boundaries)
    {
        //initialization
        spawnablePoints = new List<Vector3>();

        //set room bounds
        Bounds roomBounds = boundaries.bounds;
        min = roomBounds.center - roomBounds.extents;
        max = roomBounds.center + roomBounds.extents;

        spawnablePoints = new List<Vector3>();
        spawnableObjects = new List<GameObject[]>();

        createGraphPoints();
    }

    //replace spacing with actual spacing for each prefab
    public void spawnObjects(int objectCount, float spacing)
    {

        for (int i = 0; i < objectCount; ++i)
        {
            int decideObjectType = UnityEngine.Random.Range(0, spawnableObjects.Count);
            GameObject[] objectsToCreate = spawnableObjects[decideObjectType];

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

    public void loadObjects(GameObject[] objects)
    {
        spawnableObjects.Add(objects);
    }

    //find random point in spawnablePoints list
    private Vector3 findRandomPoint()
    {
        if(spawnablePoints.Count == 0)
        {
            Debug.LogError("No spawnable points");
            return Vector3.zero;
        }

        int randomNumTracker;

        randomNumTracker = UnityEngine.Random.Range(0, spawnablePoints.Count);
        Vector3 point = spawnablePoints[randomNumTracker];
        spawnablePoints.RemoveAt(randomNumTracker);

        return point;
    }

    //removes all points near given point so objects don't clump
    private bool removeWaypointsInProximity(Vector3 point, float distance)
    {
        spawnablePoints.RemoveAll(item => Vector3.Distance(item, point) <= distance);
        if (spawnablePoints.Count == 0)
            return false;

        return true;
    }

    //Creates graph points based on boundaries and resolution
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

