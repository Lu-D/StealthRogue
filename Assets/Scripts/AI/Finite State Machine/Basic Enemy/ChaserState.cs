using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaserState
{
public class FindRoom : State
{
    //singleton of state
    private static FindRoom instance = null;

    public override void Enter(EnemyControl owner)
    {
        owner.pathFinder.canSearch = true;
        owner.pathFinder.canMove = true;
    }

    public override void Execute(EnemyControl owner)
    {

    }

    public override void Exit(EnemyControl owner)
    {
    }

    //singleton
    public static FindRoom Instance
    {
        get
        {
            if (instance == null)
                instance = new FindRoom();

            return instance;
        }
    }
}
public class SearchRoom : State
{
    //singleton of state
    private static SearchRoom instance = null;

    public override void Enter(EnemyControl owner)
    {
    }

    public override void Execute(EnemyControl owner)
    {

    }

    public override void Exit(EnemyControl owner)
    {
    }

    //singleton
    public static SearchRoom Instance
    {
        get
        {
            if (instance == null)
                instance = new SearchRoom();

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
    }

    public override void Execute(EnemyControl owner)
    {

    }

    public override void Exit(EnemyControl owner)
    {
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
public class ChaserStateGlobal : State
{
    //singleton of state
    private static ChaserStateGlobal instance = null;

    public override void Enter(EnemyControl owner)
    {
    }

    public override void Execute(EnemyControl owner)
    {
        
    }

    public override void Exit(EnemyControl owner)
    {
    }

    //singleton
    public static ChaserStateGlobal Instance
    {
        get
        {
            if (instance == null)
                instance = new ChaserStateGlobal();

            return instance;
        }
    }
}
}
