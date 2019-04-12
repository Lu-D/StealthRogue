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

        owner.playerSpotted = false;

        var patrolBehavior = owner.GetComponent<Pathfinding.PatrolBackAndForth>();
        if (patrolBehavior != null)
        {
            patrolBehavior.enabled = true;
        }
    }

    public override void Execute(BEnemy owner)
    {
        //check if player is spotted every udpate
        owner.playerSpotted = owner.enemyVision.checkVision();

        //changes to attack state if enemy spots player
        if (owner.playerSpotted)
            owner.mainFSM.changeState(AttackPlayer.Instance);

        //changes to lookAtMe state when lookatme message is received
        if(owner.messageReceiver.message == message_type.lookAtMe)
            owner.mainFSM.changeState(LookAtMe.Instance);

    }

    public override void Exit(BEnemy owner)
    {
        //owner.waypointNav.stopNav();
        var patrolBehavior = owner.GetComponent<Pathfinding.PatrolBackAndForth>();
        if (patrolBehavior != null) patrolBehavior.enabled = false;
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
        var ai = owner.GetComponent<Pathfinding.IAstarAI>();
        if (ai != null) ai.isStopped = true;

        Vector3 bombPosition = owner.messageReceiver.getMessageContent<Vector3>();
        owner.taskList["LookAtMe"] = new Task(owner.RotateTo(bombPosition, 5f));
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
        if (owner.messageReceiver.message == message_type.lookAtMe)
            owner.mainFSM.reenterState();
        //Reverts back to patrol waypoint state after coroutine is done running
        if (!owner.taskList.Running("LookAtMe"))
            owner.mainFSM.changeState(PatrolWaypoint.Instance);

    }

    public override void Exit(BEnemy owner)
    {
        owner.taskList.Stop("LookAtMe");

        var ai = owner.GetComponent<Pathfinding.IAstarAI>();
        if (ai != null) ai.isStopped = false;
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

public class PatrolEnemyGlobal : State
{
    //singleton of state
    private static PatrolEnemyGlobal instance = null;

    public override void Enter(BEnemy owner)
    {
        if(owner as EnemyPatrol == null)
            Debug.LogError("Incorrect Assignment of EnemyPatrolStates to Enemy");
        }

    public override void Execute(BEnemy owner)
    {
        if (owner.health <= 0 && owner.mainFSM.getCurrentState() != Die.Instance)
            owner.mainFSM.changeState(Die.Instance);
    }

    public override void Exit(BEnemy owner)
    {
    }

    //singleton
    public static PatrolEnemyGlobal Instance
    {
        get
        {
            if (instance == null)
                instance = new PatrolEnemyGlobal();

            return instance;
        }
    }
}
}
