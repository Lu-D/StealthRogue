using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfinding {
	/** Simple patrol behavior.
	 * This will set the destination on the agent so that it moves through the sequence of objects in the #targets array.
	 * Upon reaching a target it will wait for #delay seconds.
	 *
	 * \see #Pathfinding.AIDestinationSetter
	 * \see #Pathfinding.AIPath
	 * \see #Pathfinding.RichAI
	 * \see #Pathfinding.AILerp
	 */
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_patrol.php")]
	public class BPatrol : MonoBehaviour {
		/** Target points to move to in order */
		protected List<WaypointNode> targets = new List<WaypointNode>();

		/** Time in seconds to wait at each target */
		protected float delay = 0;

		/** Current target index */
		protected int index;

		protected IAstarAI agent;
		protected float switchTime;

        private NodePerimeterGen nodePerimeterGen;

        private void Awake()
        {
            agent = GetComponent<IAstarAI>();
            index = 0;
            nodePerimeterGen = new NodePerimeterGen(this);
            enabled = false;
            switchTime = Mathf.Infinity;
        }

        private void Start()
        {

        }

        public void setWaypoints(GameObject[] waypointList)
        {
            targets.Clear();
            foreach (GameObject i in waypointList)
            {
                targets.Add(new WaypointNode(i));
            }
        }

        public void setWaypoints(GameObject waypointMaster)
        {
            targets.Clear();
            for (int i = 0; i < waypointMaster.transform.childCount; i++)
            {
                targets.Add(new WaypointNode(waypointMaster.transform.GetChild(i).gameObject));
            }
        }

        public void setWaypoints(Vector3[] vector3List)
        {
            targets.Clear();
            foreach (Vector3 i in vector3List)
            {
                targets.Add(new WaypointNode(i));
            }
        }

        public void setWaypoints(Transform[] transformList)
        {
            targets.Clear();
            foreach (Transform i in transformList)
            {
                targets.Add(new WaypointNode(i.position));
            }
        }

        public void setRoomWaypoints(int vertices = 10, float spacing = 1f)
        {
            targets.Clear();
            //Debug.Log(GetComponent<BEnemy>().mapLocation);
            nodePerimeterGen.createWaypoints(vertices, spacing, GetComponent<BEnemy>().mapLocation);
        }

        private class NodePerimeterGen
        {
            private BPatrol waypointNav;
            private List<Vector3> reachablePoints;
            private float desiredArea;
            private int resolution;
            //private MapContainer mapContainer;

            public int Resolution
            {
                set { resolution = value; }
            }


            public NodePerimeterGen(BPatrol _waypointNav)
            {
                reachablePoints = new List<Vector3>();
                waypointNav = _waypointNav;
                resolution = 15;
                //mapContainer = GameObject.Find("Maps").GetComponent<MapContainer>();
            }


            public void createWaypoints(int vertices, float spacing, string mapLocation)
            {
                findReachablePoints(mapLocation);

                for (int i = 0; i < vertices; ++i)
                {
                    Vector3 point = findRandomPoint();
                    waypointNav.targets.Add(new WaypointNode(point));

                    if (!removeWaypointsInProximity(point, spacing))
                        break;
                }
            }

            private Vector3 findRandomPoint()
            {
                int randomNumTracker;

                randomNumTracker = Random.Range(0, reachablePoints.Count);
                Vector3 point = reachablePoints[randomNumTracker];
                reachablePoints.RemoveAt(randomNumTracker);

                return point;
            }

            private bool removeWaypointsInProximity(Vector3 point, float distance)
            {
                reachablePoints.RemoveAll(item => Vector3.Distance(item, point) <= distance);
                if (reachablePoints.Count == 0)
                    return false;

                return true;
            }

            private void findReachablePoints(string mapLocation)
            {
                if (mapLocation == "")
                {
                    Debug.LogError("Invalid Map Location");
                    return;
                }

                //Bounds roomBounds = mapContainer.maps[mapLocation].GetComponent<Map>().mapBounds;
                Bounds roomBounds = GameObject.Find("Maps").transform.Find(mapLocation).Find("MapTrigger").GetComponent<Collider2D>().bounds;

                Vector3 min = roomBounds.min;
                Vector3 max = roomBounds.max;
                float xStep = (max.x - min.x) / resolution;
                float yStep = (max.y - min.y) / resolution;

                for (float i = min.y + yStep; i < max.y; i += yStep)
                {
                    for (float j = min.x + xStep; j < max.x; j += xStep)
                    {
                        reachablePoints.Add(new Vector2(j, i));
                    }
                }
            }
        }
    }
}
