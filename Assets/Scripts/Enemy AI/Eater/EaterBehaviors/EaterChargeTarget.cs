using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EaterChargeTarget : MonoBehaviour
{
    private Pathfinding.IAstarAI ai;
    private float startTime;

    private float timepassed = 0;
    public float timePassed
    {
        get{ return timepassed; }
    }
    private bool canhittarget = false;
    public bool canHitTarget
    {
        get{ return canhittarget; }
    }
    public float chargeSpeed = 2.5f;
    public float chargeDistance = 3f;
    public Transform target;

    private void OnEnable()
    {
        if (target == null) return;

        ai = GetComponent<Pathfinding.IAstarAI>();

        startTime = Time.time;
        timepassed = 0;

        chargePlayer();
    }

    private void OnDisable()
    {
        target = null;
        ai.maxSpeed /= chargeSpeed;
    }

    private void Update()
    {
        if (ai.reachedEndOfPath) timepassed = Time.time - startTime;
    }

    private void chargePlayer()
    {
        if (ai != null)
        {
            LayerMask viewCastLayer = ~(1 << LayerMask.NameToLayer("PlayerSceneColl") |
            1 << LayerMask.NameToLayer("PlayerHurt") |
            1 << LayerMask.NameToLayer("Enemy"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position, chargeDistance, viewCastLayer);
            ai.maxSpeed *= chargeSpeed;
            if (hit.collider != null) ai.destination = hit.point;
            else ai.destination = transform.position + (target.position - transform.position).normalized * chargeDistance;
        }
    }
}
