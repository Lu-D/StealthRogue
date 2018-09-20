using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Struct for keeping track of directions for animator
    private struct animDirection
    {
        public static float x;
        public static float y;
    }

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

	// Use this for initialization
	void Start () {
        StartCoroutine(moveTowardsNext());
	}

    // Update is called once per frame
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

    private void LateUpdate()
    {
        enemyVision.drawFOV();
    }

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

    //rotates enemy to face target
    //coroutine stops when enemy is facing target
    //watch for multiple rotate to faces triggering at same time
    IEnumerator RotateToFaceWaypoint(Transform targ)
    {
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized);
        while (transform.rotation != lookDirection && !lookingAtPlayer)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized), rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    //rotates enemy to face target
    //coroutine stops when enemy is facing target
    //watch for multiple rotate to faces triggering at same time
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

    //move towards next waypoint
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

    //"collects waypoints as enemy progresses to avoid colliding again while rotating
    void disableWaypoint(GameObject waypoint)
    {
        waypoint.SetActive(false);
    }

    //reenables all waypoints at end of chain
    void reenableWaypoints()
    {
        foreach(Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(true);
        }
    }

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