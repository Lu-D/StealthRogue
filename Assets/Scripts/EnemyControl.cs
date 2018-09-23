using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyControl
//class to control enemy behavior
public class EnemyControl : MonoBehaviour {

    public float detectionAngle;
    public float detectionDistance;
    public GameObject bullet;
    public GameObject waypointControl;
    public Transform[] wayPoints; //waypoints[1] is waypointControl
    public float moveSpeed;
    public float rotateSpeed;
    public float fovResolution;

    private GameObject target;
    private PlayerControl targetControl;
    private AttackPatterns attackPatterns;
    private Animator anim;
    private GameObject gun;
    private IEnumerator attackCoroutine;
    private int nextWayPoint;
    private bool patrolDirection; //false when going backwards
    private bool lookingAtPlayer; //to prevent multiple lookToward coroutines from starting
    private EnemyVision enemyVision;
    public MeshFilter viewMeshFilter;

    //animDirection
    //Struct for keeping track of directions for animator
    private struct animDirection
    {
        public static float x;
        public static float y;
    }

    //Awake
    //on start script
    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        targetControl = target.GetComponent<PlayerControl>();
        gun = transform.GetChild(0).gameObject;
        attackPatterns = new AttackPatterns();
        wayPoints = waypointControl.GetComponentsInChildren<Transform>();
        patrolDirection = true;
        nextWayPoint = 2; //set to two to navigate towards first waypoint that is not an enpoint
        anim = GetComponent<Animator>();
        lookingAtPlayer = false;
        enemyVision = new EnemyVision(target, gameObject.transform, detectionAngle, detectionDistance, fovResolution, viewMeshFilter);
    }

    //Start
	//on initialization
	void Start () {
        StartCoroutine(moveTowardsNext());
	}

    //Update
    //called once per frame
    void Update()
    {

        updateVision();
        if (targetControl.isSpotted)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            if (!lookingAtPlayer)
            {
                StartCoroutine(RotateToFacePlayer(target.transform));
            }
            if (!attackPatterns.getIsAttacking())
            {
                attackStates();
                StartCoroutine(attackCoroutine);
            }
        }
        else
        {
            targetControl.isSpotted = enemyVision.checkVision();
            if (targetControl.isSpotted)
                StopAllCoroutines();
        }
    }

    //LateUpdate
    //draws enemies fieldOfView for player
    private void LateUpdate()
    {
        enemyVision.drawFOV();
    }

    //updateVision
    //update animator with direction state
    void updateVision()
    {
        Vector3 directionVector = gun.transform.position - transform.position;
        if (directionVector.x > 0)
            animDirection.x = 1;
        else if (directionVector.x < 0)
            animDirection.x = -1;
        else
            animDirection.x = 0;

        if (directionVector.y > 0)
            animDirection.y = 1;
        else if (directionVector.y < 0)
            animDirection.y = -1;
        else
            animDirection.y = 0;

        anim.SetFloat("MoveX", animDirection.x);
        anim.SetFloat("MoveY", animDirection.y);

        //determines whether or not to play idle animation
        if(transform.GetComponent<Rigidbody2D>().velocity == new Vector2(0,0))
            anim.SetBool("isMoving", false);
        else
            anim.SetBool("isMoving", true);
    }

    //RotateToFaceWaypoint
    //rotates enemy to face target
    //coroutine stops when enemy is facing target
    IEnumerator RotateToFaceWaypoint(Transform targ)
    {
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized);
        while (transform.rotation != lookDirection && !lookingAtPlayer)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized), rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //RotateToFacePlayer
    //rotates enemy to face target
    //coroutine stops when enemy is facing target
    IEnumerator RotateToFacePlayer(Transform targ)
    {
        lookingAtPlayer = true;
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized);
        while (transform.rotation != lookDirection)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized), rotateSpeed * Time.deltaTime);
            yield return null;
        }
        lookingAtPlayer = false;
    }

    //attackStates
    //randomly choose attackPattern
    void attackStates()
    {
        int randomState = UnityEngine.Random.Range(0, 2);
        if (randomState == 0)
        {
            attackCoroutine = attackPatterns.shootThree(gun, bullet, 5, 2);
        }
        else
        {
            attackCoroutine = attackPatterns.shootStraight(gun, bullet, 5, 2);
        }
    }
    
    //moveTowardsNext
    //move towards next waypoint in wayPoints
    IEnumerator moveTowardsNext()
    {
        float waitTime = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitTime;
        float waitToRotate = wayPoints[nextWayPoint].gameObject.GetComponent<WaypointControl>().waitToRotate;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);
        StartCoroutine(RotateToFaceWaypoint(wayPoints[nextWayPoint]));
        if (waitToRotate > 0)
            yield return new WaitForSeconds(waitToRotate);
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
    }

    //disableWaypoint
    //collects waypoints as enemy progresses to avoid colliding again while rotating
    void disableWaypoint(GameObject waypoint)
    {
        waypoint.SetActive(false);
    }

    //reenableWaypoints
    //reenables all waypoints at end of chain
    void reenableWaypoints()
    {
        foreach(Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(true);
        }
    }

    //OnTriggerEnter2D
    //determines whether waypoint is waypoint or endpoint
    //reverses direction on collision with endpoint
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Waypoint")
        {
            if(GameObject.ReferenceEquals(other.transform.gameObject, wayPoints[nextWayPoint].gameObject))
            {
                disableWaypoint(other.transform.gameObject);
                if (patrolDirection)
                    ++nextWayPoint;
                else
                    --nextWayPoint;
                StartCoroutine(moveTowardsNext());
            }
        }
        else if(other.transform.tag == "Endpoint")
        {

            if (GameObject.ReferenceEquals(other.transform.gameObject, wayPoints[nextWayPoint].gameObject))
            {
                reenableWaypoints();
                patrolDirection = !patrolDirection;
                if (patrolDirection)
                    ++nextWayPoint;
                else
                    --nextWayPoint;
                StartCoroutine(moveTowardsNext());
            }
        }
    }
}