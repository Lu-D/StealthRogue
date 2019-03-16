using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WaypointGameObject : MonoBehaviour
{
    public Vector3 position;
    public float waitTime; //wait before any action upon touching waypoint
    public float waitToRotate; //wait to start moving after rotation begins
    public bool visited = false;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        position = gameObject.transform.position;
        waitTime = 0;
        waitToRotate = 0;
    }
}
