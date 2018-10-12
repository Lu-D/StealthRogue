using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BasicEnemyState
{
    public class WaypointState : State
    {
        private static WaypointState instance = null;

        public override void Enter(EnemyControl owner)
        {
            owner.viewMeshFilter.SetActive(true);
            owner.StartCoroutine(owner.moveTowardsNext());
        }

        public override void Execute(EnemyControl owner)
        {
            owner.targetControl.isSpotted = owner.enemyVision.checkVision();

            if (owner.targetControl.isSpotted)
            {
                owner.FSM.changeState(AttackState.Instance);
            }
        }

        public override void Exit(EnemyControl owner)
        {
            owner.StopAllCoroutines();
        }

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
        private static AttackState instance = null;
        private Task attackOneShot;
        private Task lookingAtPlayerOneShot;

        public override void Enter(EnemyControl owner)
        {
            owner.viewMeshFilter.SetActive(false);
            owner.targetControl.gettingCaught = true;
            owner.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }

        public override void Execute(EnemyControl owner)
        {
            owner.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

            if (attackOneShot == null || !attackOneShot.Running) {
                owner.attackStates();
                attackOneShot = new Task(owner.attackCoroutine);
            }

            if(lookingAtPlayerOneShot == null || !lookingAtPlayerOneShot.Running)
            {
                lookingAtPlayerOneShot = new Task(owner.RotateToFacePlayer(owner.targetControl.transform));
            }

            if (!owner.targetControl.isSpotted)
            {
                owner.FSM.changeState(WaypointState.Instance);
            }
        }

        public override void Exit(EnemyControl owner)
        {
            attackOneShot.Stop();
            lookingAtPlayerOneShot.Stop();
        }

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

