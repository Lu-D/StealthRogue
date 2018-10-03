using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chronoShiftControl : EquipmentController {
    private Queue<Vector3> posQueue;
    private int queueSize;
    public int secondsAgo;

    void Awake () {
        equipType = (int)Equipment.chronoshift;
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerControl>();
        queueSize = 0;
        StartCoroutine(positionTracker());
    }

    public override void onKeyDown()
    {
        player.transform.position = posQueue.Peek();
    }

    public override void onCollide(Collision2D collision)
    {
        
    }

    IEnumerator positionTracker()
    {
        while(true)
        {
            if(queueSize < secondsAgo*(1/0.2))
            {
                ++queueSize;
            }
            else
            {
                posQueue.Dequeue();
            }
            posQueue.Enqueue(player.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
