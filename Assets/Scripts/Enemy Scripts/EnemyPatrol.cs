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
        enemyVision = new EnemyVision(this);

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

    //LateUpdate
    //draws enemies fieldOfView for player
    private void LateUpdate()
    {
        enemyVision.drawFOV();
    }

    //updateVision
    //update animator with direction state
    protected override void updateAnim()
    {
        Animator anim = transform.Find("Texture").GetComponent<Animator>();
        Vector3 directionVector = front.transform.position - transform.position;

        if (directionVector.x > 0.01f)
        {
            animDirection.x = 1;
        }
        else if (directionVector.x < -.01f)
        {
            animDirection.x = -1;
        }
        else
            animDirection.x = 0;


        if (directionVector.y > .01f)
        {
            animDirection.y = 1;
        }
        else if (directionVector.y < -.01f)
        {
            animDirection.y = -1;
        }
        else
        {
            animDirection.y = 0;
        }

        anim.SetFloat("MoveX", animDirection.x);
        anim.SetFloat("MoveY", animDirection.y);

        //keeps animation texture upright
        Transform texture = transform.Find("Texture");
        if(texture.rotation != up)
            texture.rotation = up;
        

        //determines whether or not to play idle animation
        if (transform.GetComponent<Rigidbody2D>().velocity != new Vector2(0,0) || pathFinder.canMove)
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);
    }
}