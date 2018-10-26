using System.Management;
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
    public IEnumerator triggerRoutine;
    public string triggerFunction;
    //public Func<Vector3> triggerFunctionVector;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerFunction.Length > 0)
        {
            other.gameObject.GetComponent<EnemyControl>().Invoke(triggerFunction, 0);
        }
        //other.gameObject.GetComponent<EnemyControl>().DoThis();
    }
}
