using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BasicEnemyState
{
    public class WaypointState : State
    {
        //singleton of state
        private static WaypointState instance = null;

        public override void Enter(EnemyControl owner)
        {
            //turn on field of vision visualization
            owner.viewMeshFilter.SetActive(true);

            //fire coroutine first time
            new Task(owner.moveTowardsNext());
        }

        public override void Execute(EnemyControl owner)
        {
            //check if player is spotted every udpate
            owner.targetControl.isSpotted = owner.enemyVision.checkVision();

            //changes to attack state if enemy spots player
            if (owner.targetControl.isSpotted)
            {
                owner.FSM.changeState(AttackState.Instance);
            }
        }

        public override void Exit(EnemyControl owner)
        {
        }

        //singleton
        public static WaypointState Instance
        {
            get
            {
                if (instance == null)
                    instance = new WaypointState();

                return instance;
            }
        }
    }

    public class AttackState : State
    {
        //singleton of state
        private static AttackState instance = null;
        
        //coroutines in execute()
        private Task attackOneShot;
        private Task lookingAtPlayerOneShot;

        public override void Enter(EnemyControl owner)
        {
            //turn off FOV visualization
            owner.viewMeshFilter.SetActive(false);
            //play gettingCaught() scene sequence
            owner.targetControl.gettingCaught = true;
            //have enemy stand in place
            owner.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            //fire coroutines first time
            attackOneShot = new Task(owner.attackCoroutine);
            lookingAtPlayerOneShot = new Task(owner.RotateToFacePlayer(owner.targetControl.transform));
        }

        public override void Execute(EnemyControl owner)
        {
            owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            //only fires coroutines if current one is not running
            if (!attackOneShot.Running) {
                owner.attackStates();
                attackOneShot = new Task(owner.attackCoroutine);
            }
            if(!lookingAtPlayerOneShot.Running)
            {
                lookingAtPlayerOneShot = new Task(owner.RotateToFacePlayer(owner.targetControl.transform));
            }

            //change to waypoint state if player is no longer spotted
            if (!owner.targetControl.isSpotted)
            {
                owner.FSM.changeState(WaypointState.Instance);
            }
        }

        public override void Exit(EnemyControl owner)
        {
            //stop both coroutines
            attackOneShot.Stop();
            lookingAtPlayerOneShot.Stop();
        }

        //singleton
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
}

