using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class WaypointNav : MonoBehaviour
{
    private BEnemy owner;
    public Transform[] waypoints; //waypoints[1] is waypointControl
    public int nextWayPoint = 2; //set to two to navigate towards first waypoint that is not an enpoint
    public bool patrolDirection = true; //false when going backwards

    public WaypointNav(BEnemy _owner)
    {
        owner = _owner;
    }

    //moveTowardsNext
    //move towards next waypoint in wayPoints
    public IEnumerator moveTowardsNext()
    {
        Rigidbody2D myRigidbody = transform.GetComponent<Rigidbody2D>();
        float waitTime = waypoints[nextWayPoint].gameObject.GetComponent<WaypointNode>().waitTime;
        float waitToRotate = waypoints[nextWayPoint].gameObject.GetComponent<WaypointNode>().waitToRotate;
        myRigidbody.velocity = new Vector3(0, 0, 0);

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        yield return owner.RotateTo(waypoints[nextWayPoint].transform.position, 0);
        if (waitToRotate > 0)
            yield return new WaitForSeconds(waitToRotate);
        myRigidbody.velocity = ((waypoints[nextWayPoint].position - transform.position).normalized * owner.moveSpeed);
    }

    //rotateTowardsNext
    //rotates towards next waypoint if enemy patrol is stationary
    public IEnumerator rotateTowardsNext()
    {
        float waitTime = waypoints[nextWayPoint].gameObject.GetComponent<WaypointNode>().waitTime;
        float waitToRotate = waypoints[nextWayPoint].gameObject.GetComponent<WaypointNode>().waitToRotate;
        yield return owner.RotateTo(waypoints[nextWayPoint].transform.position, 0);

        if (nextWayPoint == waypoints.Length || nextWayPoint == 1)
        {
            patrolDirection = !patrolDirection;
        }

        if (patrolDirection)
            ++nextWayPoint;
        else
            --nextWayPoint;

        yield return new WaitForSeconds(waitToRotate);
        //messageReceiver = new Message<Vector3>(message_type.nextWaypoint);
    }

    //disableWaypoints
    //disables all waypoints
    public void disableWaypoints()
    {
        foreach (Transform waypoint in waypoints)
        {
            waypoint.gameObject.SetActive(false);
        }
    }

    //reenableWaypoints
    //reenables all waypoints at end of chain
    public void reenableWaypoints()
    {
        foreach (Transform waypoint in waypoints)
        {
            waypoint.gameObject.SetActive(true);
        }
    }
}
