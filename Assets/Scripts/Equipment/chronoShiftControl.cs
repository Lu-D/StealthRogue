using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chronoShiftControl : EquipmentController {
    private Queue<Vector3> posQueue;
    private int queueSize;
    public int secondsAgo;
    public GameObject shadow;

    public override void Awake () {
        base.Awake();
        equipType = (int)Equipment.chronoshift;
        queueSize = 0;
        posQueue = new Queue<Vector3>();
        StartCoroutine(positionTracker());
    }

    public override void onKeyDown()
    {
        Debug.Log("onkeydown");
        Debug.Log(posQueue.Peek());
        Debug.Log(player.transform.position);
        player.transform.position = posQueue.Peek();
    }

    public override void onCollide(Collision2D collision)
    {
        
    }

    IEnumerator positionTracker()
    {
        while(true)
        {
            if(queueSize < secondsAgo*(1/0.1))
            {
                ++queueSize;
            }
            else
            {
                posQueue.Dequeue();
            }
            posQueue.Enqueue(player.transform.position);
            shadow.transform.position = posQueue.Peek();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
