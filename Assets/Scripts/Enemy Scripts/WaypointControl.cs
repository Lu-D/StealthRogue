using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WaypointControl
//maintains variables for delays at each waypoint
public class WaypointControl : MonoBehaviour {
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public float waitTime; //wait before any action upon touching waypoint
    public float waitToRotate; //wait to start moving after rotation begins
}
