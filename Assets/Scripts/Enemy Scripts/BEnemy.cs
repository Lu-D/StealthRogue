using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEnemy : MonoBehaviour
{
    //enemy properties
    public float detectionAngle;
    public float detectionDistance;
    public float moveSpeed;
    public float rotateSpeed;
    public float fovResolution;
    public float distToFire;
    public bool playerSpotted = false;
    public int currAmmo;
    public int maxAmmo;
    public float reloadTime;
    public string mapLocation;
    public bool movingPatrol;
    public bool patrolLoop;
    public int health = 1;
    public bool isDead = false;
    public bool patrolDirection = true; //false when going backwards
    public GameObject itemDrop;

    //Finite State Machines
    [HideInInspector]
    public StateMachine mainFSM;
    [HideInInspector]
    public StateMachine attackFSM;

    [HideInInspector]
    public EnemyVision enemyVision;
    [HideInInspector]
    public AttackPatterns attackPatterns = new AttackPatterns();
    [HideInInspector]
    public Pathfinding.AIPath pathFinder;

    //Receives messages
    [HideInInspector]
    public MessageReceiver messageReceiver = new MessageReceiver();

    [HideInInspector]
    public GameObject viewMeshFilter;
    [HideInInspector]
    public PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    [HideInInspector]
    public TaskList taskList = new TaskList();

//private stuff
    private void Awake()
    {
        enemyVision = new EnemyVision(this);
        pathFinder = GetComponent<Pathfinding.AIPath>();

        //initialize state machines
        mainFSM = new StateMachine(this);
        attackFSM = new StateMachine(this);

        myAwake(); //called from derived class
    }
    protected virtual void myAwake(){ }

//public stuff
    public void BupdateAnim(){ updateAnim(); }
    protected virtual void updateAnim(){ }

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

    public IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public T castTo<T>() where T : BEnemy
    {
        return (T)this;
    }
}
