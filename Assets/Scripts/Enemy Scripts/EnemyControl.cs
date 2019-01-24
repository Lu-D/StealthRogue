using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicEnemyState;
using BasicEnemyAttackState;
using Pathfinding;


//EnemyControl
//class to control enemy behavior
public class EnemyControl : MonoBehaviour {

    //enemy properties
    public float detectionAngle;
    public float detectionDistance;
    public float moveSpeed;
    public float rotateSpeed;
    public float fovResolution;
    public float distToFire;
    public bool playerSpotted;
    public int currAmmo;
    public int maxAmmo;
    public float reloadTime;
    public string mapLocation;
    public bool movingPatrol;
    public bool patrolLoop;
    public int health;
    public bool isDead;

    //enemy children gameobjects
    public GameObject bullet;
    public GameObject waypointControl;
    public GameObject texture;
    public GameObject viewMeshFilter;
    public GameObject itemDrop;
    [HideInInspector]
    public PlayerControl targetControl;
    [HideInInspector]
    public GameObject gun;
    [HideInInspector]
    public Transform[] wayPoints; //waypoints[1] is waypointControl

    public EnemyVision enemyVision;
    public AttackPatterns attackPatterns;
    [HideInInspector]
    public AIPath pathFinder;

    //Finite State Machines
    public StateMachine mainFSM;
    public StateMachine attackFSM;

    //coroutines in attackPlayer state
    public Task attackOneShot;
    public Task lookingAtPlayerOneShot;
    public Task reloadOneShot;
    public Task lookAtMeOneShot;

    //Receives messages
    public Message messageReceiver = new Message(Vector3.zero, null);

    //helper variables
    public int nextWayPoint;
    public IEnumerator attackCoroutine;
    public bool patrolDirection; //false when going backwards
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D myRigidbody;
    [HideInInspector]
    public Quaternion up; //to keep texture upright
    [HideInInspector]
    public Vector3 locationBeforeAttack;

    //animDirection
    //Struct for keeping track of directions for animator
    public struct animDirection
    {
        public static float x;
        public static float y;
    }

    //Awake
    //on start script
    void Awake()
    {
        //variable initalization
        patrolDirection = true;
        playerSpotted = false;
        isDead = false;
        health = 1;
        up = transform.rotation;
        nextWayPoint = 2; //set to two to navigate towards first waypoint that is not an enpoint

        //class and component initialization
        target = GameObject.FindWithTag("Player");
        targetControl = target.GetComponent<PlayerControl>();
        gun = transform.Find("Gun").gameObject;
        attackPatterns = new AttackPatterns();
        wayPoints = waypointControl.GetComponentsInChildren<Transform>();
        enemyVision = new EnemyVision(this);
        myRigidbody = transform.GetComponent<Rigidbody2D>();
        pathFinder = GetComponent<AIPath>();

        //initialize state machine and enter first state
        mainFSM = new StateMachine(this);
        mainFSM.changeState(PatrolWaypoint.Instance);
        mainFSM.changeGlobalState(BasicEnemyGlobal.Instance);

        attackFSM = new StateMachine(this);
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

    //Moves enemy back to position before attack
    public void revertPositionBeforeAttack(State newState)
    {

        pathFinder.canSearch = true;
        pathFinder.canMove = true;
        pathFinder.destination = locationBeforeAttack;

        if (pathFinder.reachedEndOfPath)
        {
            pathFinder.canSearch = false;
            pathFinder.canMove = false;
            mainFSM.changeState(newState);
        }
    }

    //updateVision
    //update animator with direction state
    public void updateAnim()
    {
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
        if(texture.transform.rotation != up)
            texture.transform.rotation = up;
        

        //determines whether or not to play idle animation
        if (myRigidbody.velocity != new Vector2(0,0) || pathFinder.canMove)
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);
    }

    public IEnumerator Reload(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    //RotateTo
    //rotates enemy to face target
    //coroutine stops when enemy is facing target
    public IEnumerator RotateTo(Vector3 targ, float delayAfter)
    {
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, (targ - transform.position).normalized);
        while (Quaternion.Angle(transform.rotation, lookDirection) > .01f)
        {
            Debug.DrawRay(transform.position, (targ - transform.position), Color.red);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDirection, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(delayAfter);
    }

    //attackStates
    //chosses shootStraight attackPattern
    public void attackStates()
    {
        attackCoroutine = attackPatterns.shootStraight(gun, bullet, 3, .5f);
    }
    
    //moveTowardsNext
    //move towards next waypoint in wayPoints
    public IEnumerator moveTowardsNext()
    {
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitToRotate;
        myRigidbody.velocity = new Vector3(0, 0, 0);
        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        StartCoroutine(RotateTo(wayPoints[nextWayPoint].transform.position, 0));
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
        StartCoroutine(RotateTo(wayPoints[nextWayPoint].transform.position, 0));

        if (nextWayPoint == waypointControl.transform.childCount || nextWayPoint == 1)
        {
            patrolDirection = !patrolDirection;
        }

        if (patrolDirection)
            ++nextWayPoint;
        else
            --nextWayPoint;

        yield return new WaitForSeconds(waitToRotate);
        messageReceiver = new Message("next waypoint");
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
                messageReceiver = new Message("next waypoint");
            }
        }
    }

    public void playAttackSound()
    {
        targetControl.myAudioSource.PlayOneShot(targetControl.audioClips[3], .5f);
    }
}