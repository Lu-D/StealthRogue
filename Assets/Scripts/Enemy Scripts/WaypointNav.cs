using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointNav
{
    private BEnemy owner;
    private List<WaypointNode> waypoints; //waypoints[1] is waypointControl
    private bool patrolDirection = true; //false when going backwards

    private bool navigating;
    public bool isNavigating{
        get { return navigating; }
    }

    public WaypointNav(BEnemy _owner)
    {
        owner = _owner;
        waypoints = new List<WaypointNode>();
        navigating = false;
    }

    public void setWaypoints(GameObject[] waypointList)
    {
        foreach(GameObject i in waypointList)
        {
            waypoints.Add(new WaypointNode(i));
        }
    }

    public void setWaypoints(GameObject waypointMaster)
    {
        for (int i = 0; i < waypointMaster.transform.childCount; i++)
        {
            waypoints.Add(new WaypointNode(waypointMaster.transform.GetChild(i).gameObject));
        }
    }

    public void setWaypoints(Vector3[] waypointList)
    {
        clearWaypoints();
        foreach(Vector3 i in waypointList)
        {
            waypoints.Add(new WaypointNode(i));
        }
    }

    public void setWaypoints(Transform[] waypointList)
    {
        clearWaypoints();
        foreach (Transform i in waypointList)
        {
            waypoints.Add(new WaypointNode(i.position));
        }
    }

    public void stopNav()
    {
        owner.taskList.Stop("Waypoint Nav");
        navigating = false;
    }

    private void clearWaypoints()
    {
        waypoints.Clear();
    }

    private int nextWayPoint = 1;
    public void navForwardBack()
    {
        if (!navigating)
        {
            navigating = true;
            owner.taskList["Waypoint Nav"] = new Task(moveTowardsNext(nextWayPoint));
        }
        else if(Vector3.Distance(owner.transform.position, waypoints[nextWayPoint].position) < .01)
        {
            waypoints[nextWayPoint].visited = true;
            if (nextWayPoint == waypoints.Count - 1 || nextWayPoint == 0)
            {
                resetWaypoints();
                patrolDirection = !patrolDirection;
            }

            if (patrolDirection)
                ++nextWayPoint;
            else
                --nextWayPoint;
            owner.taskList["Waypoint Nav"] = new Task(moveTowardsNext(nextWayPoint));
        }
    }

    public void resetWaypoints()
    {
        foreach(WaypointNode i in waypoints)
        {
            i.visited = false;
        }
    }

    //moveTowardsNext
    //move towards next waypoint in wayPoints
    private IEnumerator moveTowardsNext(int nextWayPoint)
    {
        Rigidbody2D myRigidbody = owner.transform.GetComponent<Rigidbody2D>();
        float waitTime = waypoints[nextWayPoint].waitTime;
        float waitToRotate = waypoints[nextWayPoint].waitToRotate;
        myRigidbody.velocity = new Vector3(0, 0, 0);

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        yield return owner.RotateTo(waypoints[nextWayPoint].position, 0);
        if (waitToRotate > 0)
            yield return new WaitForSeconds(waitToRotate);
        myRigidbody.velocity = ((waypoints[nextWayPoint].position - owner.transform.position).normalized * owner.moveSpeed);
    }

}
