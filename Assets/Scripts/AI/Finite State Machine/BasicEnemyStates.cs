using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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

            //fire coroutine first time
            new Task(owner.moveTowardsNext());

            //turn on all waypoints
            owner.reenableWaypoints();
        }

        public override void Execute(EnemyControl owner)
        {
            //check if player is spotted every udpate
            owner.targetControl.isSpotted = owner.enemyVision.checkVision();

            //changes to attack state if enemy spots player
            if (owner.targetControl.isSpotted)
                owner.FSM.changeState(AttackPlayer.Instance);

            //changes to new state when enemy receives message
            if (owner.messageReceiver.newState != null)
                owner.FSM.changeState(owner.messageReceiver.newState);

        }

        public override void Exit(EnemyControl owner)
        {
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
        
        //coroutines in execute()
        private Task attackOneShot;
        private Task lookingAtPlayerOneShot;

        public override void Enter(EnemyControl owner)
        {
            //play gettingCaught() scene sequence
            owner.targetControl.gettingCaught = true;
            //have enemy stand in place
            owner.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            //turn off FOV visualization
            owner.viewMeshFilter.SetActive(false);
            //turn off all waypoints
            owner.disableWaypoints();

            //fire coroutines first time
            attackOneShot = new Task(owner.attackCoroutine);
            lookingAtPlayerOneShot = new Task(owner.RotateTo(owner.targetControl.transform.position, 0f));
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
                lookingAtPlayerOneShot = new Task(owner.RotateTo(owner.targetControl.transform.position, 0f));
            }

            //change to waypoint state if player is no longer spotted
            if (!owner.targetControl.isSpotted)
            {
                owner.FSM.changeState(PatrolWaypoint.Instance);
            }
        }

        public override void Exit(EnemyControl owner)
        {
            //stop both coroutines
            attackOneShot.Stop();
            lookingAtPlayerOneShot.Stop();
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

        //coroutines in execute()
        private Task lookAtBombOneShot;

        public override void Enter(EnemyControl owner)
        {
            //take contents of message and look at sender
            Vector3 lookAtPosition = owner.messageReceiver.sender.transform.position;
            lookAtBombOneShot = new Task(owner.RotateTo(lookAtPosition, 5f));

            //clear message 
            owner.messageReceiver = new Message(null, null);
        }

        public override void Execute(EnemyControl owner)
        {
            owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            //check if player is spotted every udpate
            owner.targetControl.isSpotted = owner.enemyVision.checkVision();

            //Change to attack state if player is spotted
            if (owner.targetControl.isSpotted)
                owner.FSM.changeState(AttackPlayer.Instance);
            //Reverts back to previous state after coroutine is done running
            if (!lookAtBombOneShot.Running)
                owner.FSM.changeState(PatrolWaypoint.Instance);
            //new msg overrides lookAtMe state
            if (owner.messageReceiver.newState != null)
                owner.FSM.changeState(owner.messageReceiver.newState);

        }

        public override void Exit(EnemyControl owner)
        {
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
}

