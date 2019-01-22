using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicEnemyAttackState;

namespace BasicEnemyState
{
public class PatrolWaypoint : State
{
    //singleton of state
    private static PatrolWaypoint instance = null;

    public override void Enter(EnemyControl owner)
    {
        //turn on FOV visualization
        owner.viewMeshFilter.SetActive(true);

        //turn on all waypoints
        owner.reenableWaypoints();

        if (owner.movingPatrol)
            new Task(owner.moveTowardsNext());
        else
            new Task(owner.rotateTowardsNext());

        owner.playerSpotted = false;
    }

    public override void Execute(EnemyControl owner)
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

    public override void Exit(EnemyControl owner)
    {
        owner.StopAllCoroutines();
        owner.locationBeforeAttack = owner.transform.position;
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

    public override void Enter(EnemyControl owner)
    {
        //turn off FOV visualization
        owner.viewMeshFilter.SetActive(false);
        //turn off all waypoints
        owner.disableWaypoints();

        owner.attackFSM.changeState(Search.Instance);
        owner.attackFSM.changeGlobalState(BasicEnemyAttackGlobal.Instance);
    }

    public override void Execute(EnemyControl owner)
    {
        owner.attackFSM.stateUpdate();
    }

    public override void Exit(EnemyControl owner)
    {
        owner.attackFSM.globalState.Exit(owner);
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
        

    public override void Enter(EnemyControl owner)
    {
        Vector3 bombPosition = owner.messageReceiver.senderPosition;
        owner.lookAtMeOneShot = new Task(owner.RotateTo(bombPosition, 5f));
    }

    public override void Execute(EnemyControl owner)
    {
        owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        //check if player is spotted every udpate
        owner.playerSpotted = owner.enemyVision.checkVision();
        if (owner.playerSpotted)
        {
            owner.targetControl.isSpotted = true;
            //play gettingCaught() scene sequence
            owner.targetControl.gettingCaught = true;
        }

            //Change to attack state if player is spotted
            if (owner.targetControl.isSpotted)
            owner.mainFSM.changeState(AttackPlayer.Instance);
        //overrides current state for a new lookatme
        if (owner.messageReceiver.message == "look at me")
            owner.mainFSM.reenterState();
        //Reverts back to patrol waypoint state after coroutine is done running
        if (!owner.lookAtMeOneShot.Running)
            owner.mainFSM.changeState(PatrolWaypoint.Instance);

    }

    public override void Exit(EnemyControl owner)
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

    public override void Enter(EnemyControl owner)
    {
        owner.isDead = true;

        //turn off FOV visualization
        owner.viewMeshFilter.SetActive(false);
        //turn off all waypoints
        owner.disableWaypoints();

        //set velocity to zero
        owner.myRigidbody.velocity = Vector3.zero;

        //drop item
        GameObject.Instantiate(owner.itemDrop, owner.transform.position, Quaternion.identity);

        //play animation
        owner.anim.SetTrigger("isDead");
    }

    public override void Execute(EnemyControl owner)
    {
            
    }

    public override void Exit(EnemyControl owner)
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

    public override void Enter(EnemyControl owner)
    {
    }

    public override void Execute(EnemyControl owner)
    {
        if(owner.mainFSM.currentState != Die.Instance)
            owner.updateAnim();

        if (owner.health <= 0 && owner.mainFSM.currentState != Die.Instance)
            owner.mainFSM.changeState(Die.Instance);
    }

    public override void Exit(EnemyControl owner)
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

