using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicEnemyAttackState
{
    public class Search : State
    {
        private static Search instance = null;

        public override void Enter(EnemyControl owner)
        {
            owner.pathFinder.canSearch = true;
            owner.pathFinder.canMove = true;
        }

        public override void Execute(EnemyControl owner)
        {
            owner.pathFinder.destination = owner.targetControl.transform.position;

            //check if enemy can hit player, if so change to fire state
            Vector3 targetDir = owner.targetControl.transform.position - owner.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, targetDir, owner.distToFire);
            if (hit.transform != null && hit.transform.tag == "Player")
                owner.attackFSM.changeState(Fire.Instance);

        }

        public override void Exit(EnemyControl owner)
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

        public override void Enter(EnemyControl owner)
        {
            owner.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            owner.attackStates();
            owner.attackOneShot = new Task(owner.attackCoroutine);
            owner.anim.SetTrigger("isShooting");
            owner.playAttackSound();
            --owner.currAmmo;
        }

        public override void Execute(EnemyControl owner)
        {
            if(owner.currAmmo == 0)
                owner.attackFSM.changeState(Reload.Instance);

            if (!owner.attackOneShot.Running)
            {
                owner.attackFSM.changeState(Search.Instance);
            }
        }

        public override void Exit(EnemyControl owner)
        {
            owner.StopAllCoroutines();
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

        public override void Enter(EnemyControl owner)
        {
            owner.reloadOneShot = new Task(owner.Reload(owner.reloadTime));
        }

        public override void Execute(EnemyControl owner)
        {
            if (!owner.reloadOneShot.Running)
                owner.attackFSM.changeState(Search.Instance);
        }

        public override void Exit(EnemyControl owner)
        {
            owner.currAmmo = owner.maxAmmo;
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

        public override void Enter(EnemyControl owner)
        {
            owner.lookingAtPlayerOneShot = new Task(owner.RotateTo(owner.targetControl.transform.position, 0f));
        }

        public override void Execute(EnemyControl owner)
        {
            if (!owner.lookingAtPlayerOneShot.Running)
                owner.lookingAtPlayerOneShot = new Task(owner.RotateTo(owner.targetControl.transform.position, 0f));
        }

        public override void Exit(EnemyControl owner)
        {
            owner.pathFinder.canSearch = false;
            owner.pathFinder.canMove = false;
            owner.lookingAtPlayerOneShot.Stop();
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
