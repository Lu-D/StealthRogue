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

    }

    public override void Execute(BEnemy owner)
    {


    }

    public override void Exit(BEnemy owner)
    {

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
    }

    public override void Execute(BEnemy owner)
    {

    }

    public override void Exit(BEnemy owner)
    {

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

    }

    public override void Execute(BEnemy owner)
    {

    }

    public override void Exit(BEnemy owner)
    {

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

    }

    public override void Execute(BEnemy owner)
    {

    }

    public override void Exit(BEnemy owner)
    {

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
