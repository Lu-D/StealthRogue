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
    public bool playerSpotted = false;
    public string mapLocation = "";
    public float health = 1;
    public GameObject itemDrop;

    //Decision making logic
    [HideInInspector]
    public StateMachine mainFSM;
    [HideInInspector]
    public GoalImpl goalImpl;

    [HideInInspector]
    public EnemyVision enemyVision;
    [HideInInspector]
    public Pathfinding.AIPath pathFinder;
    public SoundManager soundManager;
    public StateMachine soundFSM;

    [HideInInspector]
    public PlayerControl player;
    [HideInInspector]
    public TaskList taskList;

    //private stuff
    public void Awake()
    {
        taskList = new TaskList();

        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        enemyVision = GetComponent<EnemyVision>();

        pathFinder = GetComponent<Pathfinding.AIPath>();

        //initialize state machines
        mainFSM = new StateMachine(this);

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        soundFSM = new StateMachine(this);

        myAwake(); //called from derived class
    }
    protected virtual void myAwake(){ }

//public

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
