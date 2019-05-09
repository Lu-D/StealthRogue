using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LookSideToSide : MonoBehaviour
{
    private Pathfinding.IAstarAI ai;
    private EnemyPatrol enemy;

    private Vector3 turnLocationRight;
    private Vector3 turnLocationLeft;
    int dir = 0;

    public void Awake()
    {
        ai = GetComponent<Pathfinding.IAstarAI>();
        enemy = GetComponent<EnemyPatrol>();
    }

    public void OnEnable()
    {
        ai.isStopped = false;
    }

    public void OnDisable()
    {
        ai.isStopped = true;
    }

    public void Update()
    {
        if(ai.reachedEndOfPath && dir == 0) 
        {
            ai.destination = transform.position - transform.right + transform.up;
            ai.SearchPath();
            ++dir;
        }
        else if(ai.reachedEndOfPath && dir == 1)
        {
            ai.destination = transform.position + transform.right + transform.up;
            ai.SearchPath();
            --dir;
        }
    }
}

