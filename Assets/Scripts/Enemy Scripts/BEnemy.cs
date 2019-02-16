using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class BEnemy : MonoBehaviour
{
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
    public bool patrolDirection; //false when going backwards
    public GameObject itemDrop;

    //Finite State Machines
    public StateMachine mainFSM;
    public StateMachine attackFSM;

    public EnemyVision enemyVision;
    public AttackPatterns attackPatterns;
    [HideInInspector]
    public AIPath pathFinder;

    //Receives messages
    public MessageReceiver messageReceiver = new MessageReceiver();

    [HideInInspector]
    public GameObject viewMeshFilter;
    [HideInInspector]
    public PlayerControl player;

    public TaskList taskList;

//private stuff
    private void Awake()
    {
        //variable initalization
        patrolDirection = true;
        playerSpotted = false;
        isDead = false;
        health = 1;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        //taskManager = new TaskManager.TaskState();

        //class and component initialization
        attackPatterns = new AttackPatterns();
        taskList = new TaskList();
        enemyVision = new EnemyVision(this);
        pathFinder = GetComponent<AIPath>();

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
