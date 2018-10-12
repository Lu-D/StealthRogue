using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaypointState : State
{
    private static WaypointState instance = null;

    public override void Enter(EnemyControl owner)
    {
        owner.viewMeshFilter.SetActive(true);
        owner.StartCoroutine(owner.moveTowardsNext());
    }

    public override void Execute(EnemyControl owner)
    {
        owner.targetControl.isSpotted = owner.enemyVision.checkVision();

        if (owner.targetControl.isSpotted)
        {
            owner.FSM.changeState(AttackState.Instance);
        }
    }

    public override void Exit(EnemyControl owner)
    {
        owner.StopAllCoroutines();
    }

    public static WaypointState Instance
    {
        get
        {
            if(instance == null)
                instance = new WaypointState();

            return instance;
        }
    }
}

public class AttackState : State
{
    private static AttackState instance = null;

    public override void Enter(EnemyControl owner)
    {
        owner.viewMeshFilter.SetActive(false);
        owner.targetControl.gettingCaught = true;
        owner.lookingAtPlayer = false;
        owner.attackPatterns.isAttacking = false;
        owner.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
    }

    public override void Execute(EnemyControl owner)
    {
        owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        if (!owner.attackPatterns.isAttacking)
        {
            owner.attackStates();
            owner.StartCoroutine(owner.attackCoroutine);
        }

        if (!owner.lookingAtPlayer)
            owner.StartCoroutine(owner.RotateToFacePlayer(owner.targetControl.transform));

        if (!owner.targetControl.isSpotted)
        {
            owner.FSM.changeState(WaypointState.Instance);
        }
    }

    public override void Exit(EnemyControl owner)
    {
        owner.lookingAtPlayer = true;
        owner.StopAllCoroutines();
    }

    public static AttackState Instance
    {
        get
        {
            if (instance == null)
                instance = new AttackState();

            return instance;
        }
    }
}

