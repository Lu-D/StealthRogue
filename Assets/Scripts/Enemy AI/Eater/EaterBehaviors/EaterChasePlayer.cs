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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        searchableArea = player.transform.Find("Searchable Area").GetComponent<PlayerSearchableArea>();
    }

    private void OnEnable()
    {
        if (ai != null && searchableArea != null)
            ai.destination = searchableArea.returnRandomPoint();
    }

    private void OnDisable()
    {
        targetReached = false;
    }

    private void Update()
    {
        targetReached = ai.reachedEndOfPath;
    }
}
