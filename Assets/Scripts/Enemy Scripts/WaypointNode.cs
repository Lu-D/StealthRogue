using System.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Encapsulation plan
 * All waypoint systems inherit from single master waypoint which affects general variables
 * Want static enemies to all behave the same
 * Difficult to have enemies behave in different ways without separate waypoint tree instances, possible completely change waypoint system?
 * 
 * TRY THIS: Have enemies posses their own internal waypoints associated inside the prefab as children. 
 * Have general enemy movement behaviors in a parent enemy script, each enemy gets its own waypoints that are easily duplicatable.
 * All designer has to do to add more is duplicate and change a member number instead of dealing with reassigning arrays
 * Startup cost will be higher
 */

//WaypointControl
//maintains variables for delays at each waypoint
public class WaypointNode : MonoBehaviour {
    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
    
    public float waitTime; //wait before any action upon touching waypoint
    public float waitToRotate; //wait to start moving after rotation begins
    public IEnumerator triggerRoutine;
    public string triggerFunction;
    //public Func<Vector3> triggerFunctionVector;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerFunction.Length > 0)
        {
            other.gameObject.GetComponent<BEnemy>().Invoke(triggerFunction, 0);
        }
        //other.gameObject.GetComponent<EnemyControl>().DoThis();
    }
}
