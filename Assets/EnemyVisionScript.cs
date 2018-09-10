using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisionScript : MonoBehaviour {

    private GameObject target;
    public float detectionAngle;
    public float detectionDistance;
    private PlayerControl targetControl;
    

    void Awake()
    {
        target = GameObject.FindWithTag("Player");
        targetControl = target.GetComponent<PlayerControl>();
    }

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (targetControl.isSpotted)
            fire();
        else
            checkVision();
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

    void fire()
    {
        Debug.Log("Firing for effect");
    }
}
