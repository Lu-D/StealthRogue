using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicEnemyAttackState;

namespace PatrolEnemyStates
{
public class PatrolWaypoint : State
{
    //singleton of state
    private static PatrolWaypoint instance = null;

    public override void Enter(BEnemy owner)
    {
        //turn on FOV visualization
        owner.viewMeshFilter.SetActive(true);

        //turn on all waypoints
        ((EnemyPatrol)owner).reenableWaypoints();

        if (owner.movingPatrol)
            new Task(((EnemyPatrol)owner).moveTowardsNext());
        else
            new Task(((EnemyPatrol)owner).rotateTowardsNext());

        owner.playerSpotted = false;
    }

    public override void Execute(BEnemy owner)
    {
        //check if player is spotted every udpate
        owner.playerSpotted = owner.enemyVision.checkVision();

        //changes to attack state if enemy spots player
        if (owner.playerSpotted)
            owner.mainFSM.changeState(AttackPlayer.Instance);

        //reenters state if it hits a waypoint
        if (owner.messageReceiver.message == "next waypoint")
            owner.mainFSM.reenterState();

        //changes to lookAtMe state when lookatme message is received
        if(owner.messageReceiver.message == "look at me")
            owner.mainFSM.changeState(LookAtMe.Instance);

    }

    public override void Exit(BEnemy owner)
    {
        owner.StopAllCoroutines();
    }

    //singleton
    public static PatrolWaypoint Instance
    {
        get
        {
            if (instance == null)
                instance = new PatrolWaypoint();

            return instance;
        }
    }
}

public class AttackPlayer : State
{
    //singleton of state
    private static AttackPlayer instance = null;

    public override void Enter(BEnemy owner)
    {
        //turn off FOV visualization
        owner.viewMeshFilter.SetActive(false);
        //turn off all waypoints
        ((EnemyPatrol)owner).disableWaypoints();

        owner.attackFSM.changeState(Search.Instance);
        owner.attackFSM.changeGlobalState(BasicEnemyAttackGlobal.Instance);
    }

    public override void Execute(BEnemy owner)
    {
        owner.attackFSM.stateUpdate();
    }

    public override void Exit(BEnemy owner)
    {
        owner.attackFSM.getGlobalState().Exit(owner);
    }

    //singleton
    public static AttackPlayer Instance
    {
        get
        {
            if (instance == null)
                instance = new AttackPlayer();

            return instance;
        }
    }
}

public class LookAtMe : State
{
    //singleton of state
    private static LookAtMe instance = null;
        

    public override void Enter(BEnemy owner)
    {
        Vector3 bombPosition = owner.messageReceiver.senderPosition;
        owner.oneShot1 = new Task(owner.RotateTo(bombPosition, 5f));
    }

    public override void Execute(BEnemy owner)
    {
        owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        //check if player is spotted every udpate
        owner.playerSpotted = owner.enemyVision.checkVision();

        //Change to attack state if player is spotted
        if (owner.playerSpotted)
            owner.mainFSM.changeState(AttackPlayer.Instance);
        //overrides current state for a new lookatme
        if (owner.messageReceiver.message == "look at me")
            owner.mainFSM.reenterState();
        //Reverts back to patrol waypoint state after coroutine is done running
        if (!owner.oneShot1.Running)
            owner.mainFSM.changeState(PatrolWaypoint.Instance);

    }

    public override void Exit(BEnemy owner)
    {
        owner.StopAllCoroutines();
    }

    //singleton
    public static LookAtMe Instance
    {
        get
        {
            if (instance == null)
                instance = new LookAtMe();

            return instance;
        }
    }
}

public class Die : State
{
    //singleton of state
    private static Die instance = null;

    public override void Enter(BEnemy owner)
    {
        owner.isDead = true;

        //turn off FOV visualization
        owner.viewMeshFilter.SetActive(false);
        //turn off all waypoints
        ((EnemyPatrol)owner).disableWaypoints();

        //set velocity to zero
        owner.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        //drop item
        GameObject.Instantiate(owner.itemDrop, owner.transform.position, Quaternion.identity);

        //play animation
        owner.transform.Find("Texture").GetComponent<Animator>().SetTrigger("isDead");
    }

    public override void Execute(BEnemy owner)
    {
            
    }

    public override void Exit(BEnemy owner)
    {
    }

    //singleton
    public static Die Instance
    {
        get
        {
            if (instance == null)
                instance = new Die();

            return instance;
        }
    }
}

public class BasicEnemyGlobal : State
{
    //singleton of state
    private static BasicEnemyGlobal instance = null;

    public override void Enter(BEnemy owner)
    {
        EnemyPatrol test = owner as EnemyPatrol;
        if(test == null)
            throw new System.ArgumentException("Only enemy Patrol can use the Patrol Enemy States");
        }

    public override void Execute(BEnemy owner)
    {
        if(owner.mainFSM.getCurrentState() != Die.Instance)
            owner.BupdateAnim();

        if (owner.health <= 0 && owner.mainFSM.getCurrentState() != Die.Instance)
            owner.mainFSM.changeState(Die.Instance);
    }

    public override void Exit(BEnemy owner)
    {
    }

    //singleton
    public static BasicEnemyGlobal Instance
    {
        get
        {
            if (instance == null)
                instance = new BasicEnemyGlobal();

            return instance;
        }
    }
}
}
