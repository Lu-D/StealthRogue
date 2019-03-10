using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour {

    public GameObject waypointControl;
    public Transform[] wayPoints; //waypoints[1] is waypointControl
    public int nextWayPoint;
    public bool patrolDirection; //false when going backwards
    public bool patrolLoop;
    public float moveSpeed;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //moveTowardsNext
    //move towards next waypoint in wayPoints
    public IEnumerator moveTowardsNext()
    {
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointNode>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointNode>().waitToRotate;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        if (waitToRotate > 0)
            yield return new WaitForSeconds(waitToRotate);
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
    }

    //disableWaypoints
    //disables all waypoints
    public void disableWaypoints()
    {
        foreach (Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(false);
        }
    }

    //reenableWaypoints
    //reenables all waypoints at end of chain
    public void reenableWaypoints()
    {
        foreach (Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(true);
        }
    }

    //OnTriggerEnter2D
    //determines whether waypoint is waypoint or endpoint
    //reverses direction on collision with endpoint
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Waypoint")
        {
            if (GameObject.ReferenceEquals(other.transform.gameObject, wayPoints[nextWayPoint].gameObject))
            {
                if ((nextWayPoint == waypointControl.transform.childCount || nextWayPoint == 1) && !patrolLoop)
                {
                    reenableWaypoints();
                    patrolDirection = !patrolDirection;
                }
                else if (nextWayPoint == waypointControl.transform.childCount && patrolLoop)
                {
                    Debug.Log("succesful loop");
                    reenableWaypoints();
                    nextWayPoint = 0;
                }
                else
                    other.transform.gameObject.SetActive(false);

                if (patrolDirection)
                    ++nextWayPoint;
                else
                    --nextWayPoint;
            }
        }
    }
}
