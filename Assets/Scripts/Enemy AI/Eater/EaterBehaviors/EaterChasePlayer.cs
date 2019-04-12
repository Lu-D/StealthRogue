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
        get { return ai.reachedEndOfPath; } 
        private set { targetReached = value; }
    }

    private void Awake()
    {
        player = GetComponent<BEnemy>().player;
        searchableArea = GetComponent<BEnemy>().player.transform.Find("Searchable Area").GetComponent<PlayerSearchableArea>();
        ai = GetComponent<Pathfinding.IAstarAI>();
    }

    private void OnEnable()
    {
        if (ai != null && searchableArea != null)
        {
            searchableArea.gettingHunted = true;
            searchableArea.zeroRadius();

            ai.destination = searchableArea.returnRandomPoint();
        }

    }

    private void OnDisable()
    {
        targetReached = false;
    }
}
