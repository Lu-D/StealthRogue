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
    private int nextWayPoint;
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
        while(transform.rotation != Quaternion.FromToRotation(transform.position, targ.position))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targ.rotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void attackStates()
    {
        int randomState = Random.Range(0, 2);
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
            Debug.Log("Casting Ray");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir);
            Debug.Log(hit.transform.tag);
            if (hit.transform.tag == "Player")
            {
                targetControl.isSpotted = true;
                Debug.Log("Player Spotted");
            }
        }
    }

    public void moveTowardsNext()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //updateAnim(transform.position, wayPoints[nextWayPoint].position);
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, wayPoints[nextWayPoint].rotation, 1);

    }

    public void moveTowardsPrev()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //updateAnim(transform.position, wayPoints[nextWayPoint].position);
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternioo, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Waypoint")
        {
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
