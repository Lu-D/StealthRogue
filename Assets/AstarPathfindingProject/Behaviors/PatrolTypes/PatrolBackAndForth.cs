using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfinding {
	/** Simple patrol behavior.
	 * This will set the destination on the agent so that it moves through the sequence of objects in the #targets array.
	 * Upon reaching a target it will wait for #delay seconds.
	 *
	 * \see #Pathfinding.AIDestinationSetter
	 * \see #Pathfinding.AIPath
	 * \see #Pathfinding.RichAI
	 * \see #Pathfinding.AILerp
	 */
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_patrol.php")]
	public class PatrolBackAndForth : BPatrol {
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
            if (index < 0)
                index *= -1;
        }

        /** Update is called once per frame */
        void Update () 
        {
            if (targets.Count == 0) return;

            bool search = false;

            if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
            {
                switchTime = Time.time + targets[Mathf.Abs(index)].waitTime;
            }

            if (Time.time >= switchTime)
            {
                --index;
                search = true;
                switchTime = float.PositiveInfinity;
            }

            if (Mathf.Abs(index) == targets.Count - 1)
                index = targets.Count - 1;

            agent.destination = targets[Mathf.Abs(index)].position;
            if (search) agent.SearchPath();
        }     
    }
}
