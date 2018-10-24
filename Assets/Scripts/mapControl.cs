using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapControl : MonoBehaviour {
    public int numEnemies;
    public int numMaps; //highest index of maps +1

    void Awake()
    {
        string roomName = "";
        foreach (Transform point in transform)
        {
            //int num = UnityEngine.Random.Range(0, numMaps);
            roomName = "MiniRooms/MinRm" + (UnityEngine.Random.Range(0, numMaps)).ToString();
            GameObject instance = Instantiate(Resources.Load(roomName, typeof(GameObject))) as GameObject;
            instance.transform.position = point.position;
            instance.transform.rotation = point.rotation;
            point.gameObject.GetComponent < spawnPointControl>().spawnedMap = roomName;
        }
    }
}
