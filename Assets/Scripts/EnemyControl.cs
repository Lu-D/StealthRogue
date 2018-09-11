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
    

    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        targetControl = target.GetComponent<PlayerControl>();
        gun = transform.GetChild(0).gameObject;
        attackPatterns = new AttackPatterns();
    }

	// Use this for initialization
	void Start () {
		
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
            if (hit.transform.tag == "Player")
            {
                targetControl.isSpotted = true;
                Debug.Log("Player Spotted");
            }
        }
    }
}
