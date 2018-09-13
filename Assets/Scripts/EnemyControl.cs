using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

    public float detectionAngle;
    public float detectionDistance;
    public GameObject bullet;
    public GameObject waypointControl;
    public Transform[] wayPoints;
    public float moveSpeed;
    public float rotateSpeed;

    private GameObject target;
    private PlayerControl targetControl;
    private AttackPatterns attackPatterns;
    private GameObject gun;
    private IEnumerator attackCoroutine;
    public int nextWayPoint;
    private bool patrolDirection; //false when going backwards

    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        targetControl = target.GetComponent<PlayerControl>();
        gun = transform.GetChild(0).gameObject;
        attackPatterns = new AttackPatterns();
        wayPoints = waypointControl.GetComponentsInChildren<Transform>();
        patrolDirection = true;
        nextWayPoint = 2;
    }

	// Use this for initialization
	void Start () {
        moveTowardsNext();
	}

    // Update is called once per frame
    void Update()
    {
        if (targetControl.isSpotted)
        {
            if (!attackPatterns.getIsAttacking())
            {
                attackStates();
                StartCoroutine(attackCoroutine);
            }
        }
        else
        {
            checkVision();
        }
    }

    IEnumerator RotateToFace(Transform targ)
    {
        Quaternion lookDirection = Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized);
        while (transform.rotation != lookDirection)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward, (targ.position - transform.position).normalized), rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

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

    void checkVision()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        if ((Vector3.Angle(targetDir, transform.up) < detectionAngle) && (Vector3.Distance(target.transform.position, transform.position) < detectionDistance))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir);
            if (hit.transform.tag == "Player")
            {
                targetControl.isSpotted = true;
            }
        }
    }

    public void moveTowardsNext()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //updateAnim(transform.position, wayPoints[nextWayPoint].position);
        StartCoroutine(RotateToFace(wayPoints[nextWayPoint]));
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
    }

    public void moveTowardsPrev()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //updateAnim(transform.position, wayPoints[nextWayPoint].position);
        StartCoroutine(RotateToFace(wayPoints[nextWayPoint]));
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
    }

    void disableWaypoint(GameObject waypoint)
    {
        waypoint.SetActive(false);
    }

    void reenableWaypoints()
    {
        foreach(Transform waypoint in wayPoints)
        {
            waypoint.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Waypoint")
        {
            disableWaypoint(other.transform.gameObject);
            if (patrolDirection)
            {
                ++nextWayPoint;
                moveTowardsNext();
            }
            else
            {
                --nextWayPoint;
                moveTowardsPrev();
            }
        }
        else if(other.transform.tag == "Endpoint")
        {
            reenableWaypoints();
            patrolDirection = !patrolDirection;
            if (patrolDirection)
            {
                ++nextWayPoint;
                moveTowardsNext();
            }
            else
            {
                --nextWayPoint;
                moveTowardsPrev();
            }
        }
    }
}