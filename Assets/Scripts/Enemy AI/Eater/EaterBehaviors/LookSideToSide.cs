using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LookSideToSide : MonoBehaviour
{
    private Pathfinding.IAstarAI ai;
    private EnemyPatrol enemy;
    
    public void Awake()
    {
        ai = GetComponent<Pathfinding.IAstarAI>();
        enemy = GetComponent<EnemyPatrol>();
    }

    public void OnEnable()
    {
        if(ai != null) ai.isStopped = true;

        Vector3 turnLocationRight = transform.forward + transform.right;
        Vector3 turnLocationLeft = transform.forward + transform.right;

        enemy.RotateTo(turnLocationRight, 1f);
        //enemy.RotateTo(turnLocationLeft, 1f);
    }

    public void OnDisable()
    {
        if(ai != null) ai.isStopped = false;
    }

    public void Update()
    {
    }
}

