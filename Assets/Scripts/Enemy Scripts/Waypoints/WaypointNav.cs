//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class WaypointNav
//{

//    private BEnemy owner;
//    private List<WaypointNode> waypoints; //waypoints[1] is waypointControl
//    private bool patrolDirection = true; //false when going backwards
//    private TaskList taskList;
//    private NodePerimeterGen nodePerimeterGen;

//    private bool navigating;
//    public bool isNavigating{
//        get { return navigating; }
//    }

//    //moveTowardsNext
//    //move towards next waypoint in wayPoints
//    private IEnumerator moveTowardsNext(int nextWayPoint)
//    {
//        Rigidbody2D myRigidbody = owner.transform.GetComponent<Rigidbody2D>();
//        float waitTime = waypoints[nextWayPoint].waitTime;
//        float waitToRotate = waypoints[nextWayPoint].waitToRotate;
//        myRigidbody.velocity = new Vector3(0, 0, 0);

//        if (waitTime > 0)
//            yield return new WaitForSeconds(waitTime);
//        yield return owner.RotateTo(waypoints[nextWayPoint].position, 0);
//        if (waitToRotate > 0)
//            yield return new WaitForSeconds(waitToRotate);
//        myRigidbody.velocity = ((waypoints[nextWayPoint].position - owner.transform.position).normalized * owner.moveSpeed);
//    }

//    private void clearWaypoints()
//    {
//        waypoints.Clear();
//    }


//    public WaypointNav(BEnemy _owner)
//    {
//        owner = _owner;
//        waypoints = new List<WaypointNode>();
//        taskList = new TaskList();
//        nodePerimeterGen = new NodePerimeterGen(this);
//        navigating = false;
//    }

//    public void setWaypoints(GameObject[] waypointList)
//    {
//        clearWaypoints();
//        foreach(GameObject i in waypointList)
//        {
//            waypoints.Add(new WaypointNode(i));
//        }
//    }

//    public void setWaypoints(GameObject waypointMaster)
//    {
//        clearWaypoints();
//        for (int i = 0; i < waypointMaster.transform.childCount; i++)
//        {
//            waypoints.Add(new WaypointNode(waypointMaster.transform.GetChild(i).gameObject));
//        }
//    }

//    public void setWaypoints(Vector3[] vector3List)
//    {
//        clearWaypoints();
//        foreach(Vector3 i in vector3List)
//        {
//            waypoints.Add(new WaypointNode(i));
//        }
//    }

//    public void setWaypoints(Transform[] transformList)
//    {
//        clearWaypoints();
//        foreach (Transform i in transformList)
//        {
//            waypoints.Add(new WaypointNode(i.position));
//        }
//    }

//    public void setRoomWaypoints()
//    {
//        clearWaypoints();
//        nodePerimeterGen.createWaypoints(10, 1f);
//    }

//    public void stopNav()
//    {
//        taskList.StopAllTasks();
//        navigating = false;
//    }

//    private int nextWayPoint;
//    public void navForwardBack()
//    {
//        if (!navigating)
//        {
//            navigating = true;
//            nextWayPoint = 1;
//            taskList["Waypoint Nav"] = new Task(moveTowardsNext(nextWayPoint));
//        }
//        else if(Vector3.Distance(owner.transform.position, waypoints[nextWayPoint].position) < .01)
//        {
//            waypoints[nextWayPoint].visited = true;
//            if (nextWayPoint == waypoints.Count - 1 || nextWayPoint == 0)
//            {
//                resetWaypoints();
//                patrolDirection = !patrolDirection;
//            }

//            if (patrolDirection)
//                ++nextWayPoint;
//            else
//                --nextWayPoint;
//            taskList["Waypoint Nav"] = new Task(moveTowardsNext(nextWayPoint));
//        }
//    }

//    public void navLoop()
//    {
//        if (!navigating)
//        {
//            navigating = true;
//            nextWayPoint = 0;
//            taskList["Waypoint Nav"] = new Task(moveTowardsNext(nextWayPoint));
//        }
//        else if (Vector3.Distance(owner.transform.position, waypoints[nextWayPoint].position) < .01)
//        {
//            waypoints[nextWayPoint].visited = true;
//            ++nextWayPoint;
//            nextWayPoint = nextWayPoint % waypoints.Count;
//            taskList["Waypoint Nav"] = new Task(moveTowardsNext(nextWayPoint));
//        }
//    }

//    public void resetWaypoints()
//    {
//        foreach(WaypointNode i in waypoints)
//        {
//            i.visited = false;
//        }
//    }

//    private class NodePerimeterGen
//    {
//        private WaypointNav waypointNav;
//        private List<Vector3> reachablePoints;
//        private float desiredArea;
//        private int resolution;
//        public int Resolution
//        {
//            set{ resolution = value; }
//        }


//        public NodePerimeterGen(WaypointNav _waypointNav)
//        {
//            reachablePoints = new List<Vector3>();
//            waypointNav = _waypointNav;
//            resolution = 15;
//        }


//        public void createWaypoints(int vertices, float spacing)
//        {
//            findReachablePoints();

//            for (int i = 0; i < vertices; ++i)
//            {
//                Vector3 point = findRandomPoint();
//                waypointNav.waypoints.Add(new WaypointNode(point));

//                if (!removeWaypointsInProximity(point, spacing))
//                    break;
//            }
//        }

//        private Vector3 findRandomPoint()
//        {
//            int randomNumTracker;

//            randomNumTracker = Random.Range(0, reachablePoints.Count);
//            Vector3 point = reachablePoints[randomNumTracker];
//            reachablePoints.RemoveAt(randomNumTracker);

//            return point;
//        }

//        private bool removeWaypointsInProximity(Vector3 point, float distance)
//        {
//            reachablePoints.RemoveAll(item => Vector3.Distance(item, point) <= distance);
//            if (reachablePoints.Count == 0)
//                return false;

//            return true;
//        }

//        private void findReachablePoints()
//        {
//            var roomBounds = GameObject.Find("Maps").transform.Find(waypointNav.owner.mapLocation).transform.Find("PlayerLocator").GetComponent<Collider2D>().bounds;

//            Vector3 min = roomBounds.min;
//            Vector3 max = roomBounds.max;
//            float xStep = (max.x - min.x) / resolution;
//            float yStep = (max.y - min.y) / resolution;
//            LayerMask viewCastLayer = ((1 << LayerMask.NameToLayer("Border")) | (1 << LayerMask.NameToLayer("Obstacle")));

//            for (float i = min.y + yStep; i < max.y; i += yStep)
//            {
//                for (float j = min.x + xStep; j < max.x; j += xStep)
//                {
//                    Vector2 testPoint = new Vector2(j, i);
//                    if (Physics2D.OverlapPoint(testPoint, viewCastLayer) == null)
//                    {
//                        reachablePoints.Add(testPoint);
//                    }
//                }
//            }
//        }
//    }
//}
