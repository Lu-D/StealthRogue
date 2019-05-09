using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EaterChasePlayer : MonoBehaviour
{
    private PlayerControl player;
    private PlayerSearchableArea searchableArea;
    private Pathfinding.IAstarAI ai;

    public bool targetReached
    {
        get; set;
    }

    private void Awake()
    {
        ai = GetComponent<Pathfinding.IAstarAI>();

        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        searchableArea = player.searchableArea;
    }

    private void OnEnable()
    {
        ai.isStopped = false;
        ai.destination = searchableArea.returnRandomPoint();
        ai.SearchPath();
    }

    private void OnDisable()
    {
        targetReached = false;
        ai.isStopped = true;
    }

    private void Update()
    {
        targetReached = ai.reachedEndOfPath;
    }
}
