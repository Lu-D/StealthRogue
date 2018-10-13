using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelBuilder : MonoBehaviour {

    public uint levelCount;
    public uint minRooms;
    public uint maxRooms;
    public uint maxLevel;
    private string path = "Assets/Resources/Maps/";

    void clearMaps()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
            if (go.activeInHierarchy)
            {
                if(go.tag != "Player" || go.tag != "levelBuilder")
                {
                    Destroy(go);
                }
            }
    }

    void determineMaps()
    {
        uint numRooms = findNumRooms();
        if(levelCount > levelCount/2)
        {
            //spawnMoreDifficultLevels
        }
        Object[] mapsToBeSpawned = new Object[5];
        for (uint i = 0; i < numRooms - 2; ++i)
        {
            string mapNumber = (UnityEngine.Random.Range(0, 8).ToString());
            mapsToBeSpawned[i] = Resources.Load(path + "map" + mapNumber);
        }
        Object startMap = Resources.Load(path + "map" + 9.ToString());
        Object endMap = Resources.Load(path + "map" + 10.ToString());
        buildLevel(mapsToBeSpawned, startMap, endMap);
    }

    uint findNumRooms()
    {
        //float roomProb = UnityEngine.Random.Range((float)minRooms, (float)maxRoooms);
        //if (roomProb < 0.5f)
        //    return minRooms;
        //else if (roomProb < 0.75)
        //    return ((minRooms + maxRooms) / 2);
        //else
        //    retunr maxRooms;
        return maxRooms;
    }


    void buildLevel(Object[] maps, Object startMap, Object endMap)
    {

    }
}
