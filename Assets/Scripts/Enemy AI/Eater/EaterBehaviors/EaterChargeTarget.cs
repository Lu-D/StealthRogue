using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EaterChargeTarget : MonoBehaviour
{
    private Pathfinding.IAstarAI ai;
    private float startTime;
    public Timer timer;

    private float chargeSpeed = 2.5f;
    private float chargeDistance = 3f;
    private bool oneshot = true;
    public Transform target;

    private void Awake()
    {
        timer = new Timer();
    }

    private void OnEnable()
    {
        if (target == null) return;

        ai = GetComponent<Pathfinding.IAstarAI>();

        chargePlayer();
    }

    public void OnDisable()
    {
        target = null;
        ai.maxSpeed /= chargeSpeed;
        timer.endTimer();
        oneshot = true;
    }

    private void Update()
    {
        if (ai.reachedEndOfPath && oneshot)
        {
            timer.startTimer();
            oneshot = false;
        }
    }

    private void chargePlayer()
    {
        LayerMask viewCastLayer = ~(1 << LayerMask.NameToLayer("PlayerSceneColl") |
        1 << LayerMask.NameToLayer("PlayerHurt") |
        1 << LayerMask.NameToLayer("Enemy"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, chargeDistance, viewCastLayer);

        ai.maxSpeed *= chargeSpeed;

        if (hit.collider != null) ai.destination = hit.point;
        else
        {
            ai.destination = transform.position + (target.position - transform.position).normalized * chargeDistance;
            ai.SearchPath();
        }
    }
}
