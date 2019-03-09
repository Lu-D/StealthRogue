using System.Collections;
using UnityEngine;
using PatrolEnemyStates;
using Pathfinding;


//EnemyControl
//class to control enemy behavior
public class EnemyPatrol : BEnemy {

    //enemy children gameobjects
    public GameObject bullet;
    public GameObject waypointControl;
    [HideInInspector]
    public Transform[] wayPoints; //waypoints[1] is waypointControl

    //helper variables
    public int nextWayPoint;
    public IEnumerator attackCoroutine;
    [HideInInspector]
    private Quaternion up; //to keep texture upright

    //animDirection
    //Struct for keeping track of directions for animator
    public struct animDirection
    {
        public static float x;
        public static float y;
    }

    //Awake
    //on start script
    protected override void myAwake()
    {
        up = transform.rotation;
        nextWayPoint = 2; //set to two to navigate towards first waypoint that is not an enpoint

        //class and component initialization
        wayPoints = waypointControl.GetComponentsInChildren<Transform>();

        //initialize state machine and enter first state
        mainFSM.changeState(PatrolWaypoint.Instance);
        mainFSM.changeGlobalState(BasicEnemyGlobal.Instance);
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
        GameObject gun = transform.Find("Gun").gameObject;
        Vector3 directionVector = gun.transform.position - transform.position;

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
    
    //moveTowardsNext
    //move towards next waypoint in wayPoints
    public IEnumerator moveTowardsNext()
    {
        Rigidbody2D myRigidbody = transform.GetComponent<Rigidbody2D>();
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitToRotate;
        myRigidbody.velocity = new Vector3(0, 0, 0);

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        yield return RotateTo(wayPoints[nextWayPoint].transform.position, 0);
        if (waitToRotate > 0)
            yield return new WaitForSeconds(waitToRotate);
        myRigidbody.velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
    }

    //rotateTowardsNext
    //rotates towards next waypoint if enemy patrol is stationary
    public IEnumerator rotateTowardsNext()
    {
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitToRotate;
        yield return RotateTo(wayPoints[nextWayPoint].transform.position, 0);

        if (nextWayPoint == waypointControl.transform.childCount || nextWayPoint == 1)
        {
            patrolDirection = !patrolDirection;
        }

        if (patrolDirection)
            ++nextWayPoint;
        else
            --nextWayPoint;

        yield return new WaitForSeconds(waitToRotate);
        messageReceiver = new Message<Vector3>(message_type.nextWaypoint);
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
        foreach(Transform waypoint in wayPoints)
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
            if(GameObject.ReferenceEquals(other.transform.gameObject, wayPoints[nextWayPoint].gameObject))
            {
                if ((nextWayPoint == waypointControl.transform.childCount || nextWayPoint == 1) && !patrolLoop)
                {
                    reenableWaypoints();
                    patrolDirection = !patrolDirection;
                }
                else if(nextWayPoint == waypointControl.transform.childCount && patrolLoop)
                {
                    reenableWaypoints();
                    nextWayPoint = 0;
                }
                else
                    other.transform.gameObject.SetActive(false);

                if (patrolDirection)
                    ++nextWayPoint;
                else
                    --nextWayPoint;
                messageReceiver = new Message<int>(message_type.nextWaypoint);
            }
        }
    }

    public void playAttackSound()
    {
        PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        player.myAudioSource.PlayOneShot(player.audioClips[3], .5f);
    }
}