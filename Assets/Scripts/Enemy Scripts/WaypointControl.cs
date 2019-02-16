using System.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Encapsulation plan
 * Waypoints are set up and have a master, to get an enemy to follow waypoint, simply add enemy to waypoint master
 */

//WaypointControl
//maintains variables for delays at each waypoint
public class WaypointControl : MonoBehaviour {

    private GameObject master;
    private wayPointMaster masterScript;
    public string[] executeOnCollide;
    
    public float waitTime; //wait before any action upon touching waypoint
    public float waitToRotate; //wait to start moving after rotation begins
    public IEnumerator triggerRoutine;
    public string triggerFunction;
    //public Func<Vector3> triggerFunctionVector;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerFunction.Length > 0)
        {
            //other.gameObject.GetComponent<BEnemy>().Invoke(triggerFunction, 0);
            masterScript.invokingEnemy = other.gameObject;
            masterScript.invokingArgs = executeOnCollide;
            masterScript.Invoke("hitWaypoint", 0);
            //clears args after, Invoke is syncronous
            masterScript.invokingArgs = "";
            masterScript.invokingEnemy = "";
        }
        //other.gameObject.GetComponent<EnemyControl>().DoThis();
    }
}
