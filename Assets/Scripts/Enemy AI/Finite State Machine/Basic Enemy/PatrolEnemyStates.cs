using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

namespace PatrolEnemyStates
{
public class PatrolWaypoint : State
{
    //singleton of state
    private static PatrolWaypoint instance = null;

    public override void Enter(BEnemy owner)
    {
        //turn on FOV visualization
        owner.GetComponent<EnemyVision>().enabled = true;

        owner.playerSpotted = false;

        var patrolBehavior = owner.GetComponent<Pathfinding.PatrolBackAndForth>();
        patrolBehavior.setRoomWaypoints(4, 1f);
        patrolBehavior.enabled = true;
    }

    public override void Execute(BEnemy owner)
    {
        //check if player is spotted every udpate
        owner.playerSpotted = owner.enemyVision.hasSeen("Player");

        //changes to attack state if enemy spots player
        if (owner.playerSpotted)
            owner.mainFSM.changeState(AlertBoss.Instance);
    }

    public override void Exit(BEnemy owner)
    {
        var patrolBehavior = owner.GetComponent<Pathfinding.PatrolBackAndForth>();
        patrolBehavior.enabled = false;
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

public class AlertBoss : State
{
    //singleton of state
    private static AlertBoss instance = null;

    public override void Enter(BEnemy owner)
    {
        //turn off FOV visualization
        owner.GetComponent<EnemyVision>().enabled = false;

        owner.pathFinder.destination = owner.player.transform.position;
        owner.pathFinder.isStopped = false;
        owner.soundManager.Play("Ghost_Screaming");

        //Alert boss player's whereabouts
        var alertBoss = new ComeToMeEvent(owner.transform.position);
        alertBoss.addListener(GameObject.Find("Boss"));
            EventManager.Instance.addEvent(alertBoss, 0);
    }

    public override void Execute(BEnemy owner)
    {
        owner.pathFinder.destination = owner.player.transform.position;
        if (!owner.soundManager.isPlaying("Ghost_Screaming"))
            owner.mainFSM.reenterState();
    }

    public override void Exit(BEnemy owner)
    {
        
        owner.pathFinder.isStopped = true;
    }

    //singleton
    public static AlertBoss Instance
    {
        get
        {
            if (instance == null)
                instance = new AlertBoss();

            return instance;
        }
    }
}

public class LookAtMe : State
{
    //singleton of state
    private static LookAtMe instance = null;

    public Vector3 lookPosition;

    public override void Enter(BEnemy owner)
    {
        var ai = owner.GetComponent<Pathfinding.IAstarAI>();
        if (ai != null) ai.isStopped = true;

        owner.taskList["LookAtMe"] = new Task(owner.RotateTo(lookPosition, 5f));
    }

    public override void Execute(BEnemy owner)
    {        
        owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        //check if player is spotted every udpate
        owner.playerSpotted = owner.enemyVision.hasSeen("Player");

        //Change to attack state if player is spotted
        if (owner.playerSpotted)
            owner.mainFSM.changeState(AlertBoss.Instance);
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
        //turn off FOV visualization
        owner.GetComponent<EnemyVision>().enabled = false;

        owner.pathFinder.isStopped = true;

        //drop item
        if(owner.itemDrop != null)
            GameObject.Instantiate(owner.itemDrop, owner.transform.position, Quaternion.identity);

        //play animation
        owner.transform.Find("Texture").GetComponent<SpriteRenderer>().enabled = false;

        //Give 1 health to player
        var giveHealth = new HealEvent(1);
        giveHealth.addListener(GameObject.Find("Player"));
        EventManager.Instance.addEvent(giveHealth);
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
        Debug.Log(owner.mainFSM.getCurrentState());
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
