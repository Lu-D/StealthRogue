using System.Collections;
using UnityEngine;


//EnemyControl
//class to control enemy behavior
public class EnemyPatrol : BEnemy {

    //enemy children gameobject
    public GameObject waypointControl;
    private GameObject front;

    //helper variables
    [HideInInspector]
    private Quaternion up; //to keep texture upright

    //animDirection
    //Struct for keeping track of directions for animator
    private struct animDirection
    {
        public static float x;
        public static float y;
    }

    //Awake
    //on start script
    protected override void myAwake()
    {
        up = transform.rotation;
        front = new GameObject("front");
        front.transform.position = Vector3.up + transform.position;
        front.transform.parent = gameObject.transform;

        //initialize state machine and enter first state
        mainFSM.changeState(PatrolEnemyStates.PatrolWaypoint.Instance);
        mainFSM.changeGlobalState(PatrolEnemyStates.PatrolEnemyGlobal.Instance);

        var patrolBehavior = GetComponent<Pathfinding.PatrolBackAndForth>();
        if (patrolBehavior != null) patrolBehavior.setWaypoints(waypointControl);
    }

    //Update
    //called once per frame
    void Update()
    {
        mainFSM.stateUpdate();
    }


}