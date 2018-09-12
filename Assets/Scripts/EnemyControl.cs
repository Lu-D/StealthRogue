using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

    private GameObject target;
    public float detectionAngle;
    public float detectionDistance;
    private PlayerControl targetControl;
    private AttackPatterns attackPatterns;
    private GameObject gun;
    private IEnumerator attackCoroutine;
    public GameObject bullet;
    private int nextWayPoint;
    public GameObject waypointControl;
    public Transform[] wayPoints;
    private bool patrolDirection; //false when going backwards
    public float moveSpeed;

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
        //moveTowardsNext();
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
    }

    public void moveTowardsPrev()
    {
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        //updateAnim(transform.position, wayPoints[nextWayPoint].position);
        transform.GetComponent<Rigidbody2D>().velocity = ((wayPoints[nextWayPoint].position - transform.position).normalized * moveSpeed);
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
