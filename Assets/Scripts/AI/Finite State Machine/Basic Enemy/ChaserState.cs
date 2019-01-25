using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChaserState
{
public class FindRoom : State
{
    //singleton of state
    private static FindRoom instance = null;

    public override void Enter(BEnemy owner)
    {
        owner.pathFinder.canSearch = true;
        owner.pathFinder.canMove = true;
    }

    public override void Execute(BEnemy owner)
    {
        
        if (owner.mapLocation == owner.player.mapLocation)
            owner.mainFSM.changeState(SearchRoom.Instance);
        if (owner.playerSpotted)
            owner.mainFSM.changeState(AttackPlayer.Instance);
    }

    public override void Exit(BEnemy owner)
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
