using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemyAttackState
{
public class Search : State
{
    private static Search instance = null;

    public override void Enter(BEnemy owner)
    {
        owner.pathFinder.canSearch = true;
        owner.pathFinder.canMove = true;
    }

    public override void Execute(BEnemy owner)
    {
        owner.pathFinder.destination = owner.player.transform.position;

        //check if enemy can hit player, if so change to fire state
        Vector3 targetDir = owner.player.transform.position - owner.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, targetDir, owner.distToFire);
        if (hit.transform != null && hit.transform.tag == "Player")
            owner.attackFSM.changeState(Fire.Instance);

    }

    public override void Exit(BEnemy owner)
    {
        owner.pathFinder.canSearch = false;
        owner.pathFinder.canMove = false;
    }

    //singleton
    public static Search Instance
    {
        get
        {
            if (instance == null)
                instance = new Search();

            return instance;
        }
    }
}

public class Fire : State
{
    private static Fire instance = null;

    public override void Enter(BEnemy owner)
    {
        owner.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        IEnumerator attackCoroutine = owner.attackPatterns.shootStraight(owner.transform.Find("Gun").gameObject, ((EnemyPatrol)owner).bullet, 3, .5f);
        owner.taskList["Shoot"] = new Task(attackCoroutine);
        owner.transform.Find("Texture").GetComponent<Animator>().SetTrigger("isShooting");
        ((EnemyPatrol)owner).playAttackSound();
        --owner.currAmmo;
    }

    public override void Execute(BEnemy owner)
    {
        if(owner.currAmmo == 0)
            owner.attackFSM.changeState(Reload.Instance);

        if (!owner.taskList.Running("Shoot"))
        {
            owner.attackFSM.changeState(Search.Instance);
        }
    }

    public override void Exit(BEnemy owner)
    {
        owner.taskList.Stop("Shoot");
    }

    //singleton
    public static Fire Instance
    {
        get
        {
            if (instance == null)
                instance = new Fire();

            return instance;
        }
    }
}

public class Reload : State
{
    private static Reload instance = null;

    public override void Enter(BEnemy owner)
    {
        owner.taskList["Reload"] = new Task(owner.Wait(owner.reloadTime));
    }

    public override void Execute(BEnemy owner)
    {
        if (!owner.taskList.Running("Reload"))
            owner.attackFSM.changeState(Search.Instance);
    }

    public override void Exit(BEnemy owner)
    {
        owner.currAmmo = owner.maxAmmo;
        owner.taskList.Stop("Reload");
    }

    //singleton
    public static Reload Instance
    {
        get
        {
            if (instance == null)
                instance = new Reload();

            return instance;
        }
    }
}

    

public class BasicEnemyAttackGlobal : State
{
    private static BasicEnemyAttackGlobal instance = null;

    public override void Enter(BEnemy owner)
    {
        owner.taskList["FacePlayer"] = new Task(owner.RotateTo(owner.player.transform.position, 0f));
    }

    public override void Execute(BEnemy owner)
    {
        if (!owner.taskList.Running("FacePlayer"))
            owner.taskList["FacePlayer"] = new Task(owner.RotateTo(owner.player.transform.position, 0f));
    }

    public override void Exit(BEnemy owner)
    {
        owner.pathFinder.canSearch = false;
        owner.pathFinder.canMove = false;
        owner.taskList.Stop("FacePlayer");
    }

    //singleton
    public static BasicEnemyAttackGlobal Instance
    {
        get
        {
            if (instance == null)
                instance = new BasicEnemyAttackGlobal();

            return instance;
        }
    }
}
}
