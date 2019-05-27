using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundStates
{
    public class EnvironmentalCrickets : State
    {
        private static EnvironmentalCrickets instance = null;
        
        public override void Enter(BEnemy owner)
        {
            owner.soundManager.FadeIn("Environment_Ambience", .2f, .001f, true);
        }

        public override void Execute(BEnemy owner)
        {
            if ((owner.player.transform.position - owner.transform.position).magnitude < 3.5f)
                owner.soundFSM.changeState(EnemyNear.Instance);
        }

        public override void Exit(BEnemy owner)
        {
            owner.soundManager.Pause("Environment_Ambience");
        }

        public static EnvironmentalCrickets Instance
        {
            get{
                if (instance == null)
                    instance = new EnvironmentalCrickets();

                return instance;
            }
        }
    }

    public class EnemyNear : State
    {
        private static EnemyNear instance = null;
        
        public override void Enter(BEnemy owner)
        {
            owner.soundManager.FadeIn("Chase_Theme_Loop", 1f, .005f, true);
        }

        public override void Execute(BEnemy owner)
        {
            if ((owner.player.transform.position - owner.transform.position).magnitude > 3.5f)
                owner.soundFSM.changeState(EnvironmentalCrickets.Instance);
        }

        public override void Exit(BEnemy owner)
        {
            owner.soundManager.FadeOut("Chase_Theme_Loop");
        }

        public static EnemyNear Instance{
            get{
                if (instance == null)
                    instance = new EnemyNear();

                return instance;
            }
        }
    }
}
