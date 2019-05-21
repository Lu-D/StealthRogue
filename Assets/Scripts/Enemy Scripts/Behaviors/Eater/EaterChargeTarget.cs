using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class EaterChargeTarget : MonoBehaviour
{
    private float startTime;
    public Timer timer;

    private float chargeSpeed = 2.5f;
    private float chargeDistance = 3f;
    private bool charging = true;
    private bool hitPlayer = false;

    public Pathfinding.IAstarAI ai;
    [HideInInspector]
    public Vector3 target;

    private void Awake()
    {
        timer = new Timer();
        ai = GetComponent<Pathfinding.IAstarAI>();
    }

    private void OnEnable()
    {
        if (target == Vector3.negativeInfinity) return;

        chargePlayer();

        ai.isStopped = false;
    }

    public void OnDisable()
    {
        ai.isStopped = true;
        target = Vector3.negativeInfinity;
        ai.maxSpeed /= chargeSpeed;
        timer.endTimer();
        charging = true;
        hitPlayer = false;
    }

    private void Update()
    {
        if (ai.reachedEndOfPath && charging)
        {
            timer.startTimer();
            charging = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (charging && collision.gameObject.tag == "Player" && !hitPlayer)
        {
            var player = collision.gameObject.GetComponent<PlayerControl>();
            --player.health;
            hitPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (charging && collision.gameObject.tag == "Player")
        {
            Rigidbody2D rigidbody = collision.gameObject.transform.GetComponent<Rigidbody2D>();
            rigidbody.transform.Translate((transform.right + transform.position + transform.up).normalized * Time.deltaTime * 6f, Space.World);
        }
    }

    private void chargePlayer()
    {
        LayerMask viewCastLayer = ~(1 << LayerMask.NameToLayer("PlayerSceneColl") |
        1 << LayerMask.NameToLayer("PlayerHurt") |
        1 << LayerMask.NameToLayer("Enemy"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target - transform.position, chargeDistance, viewCastLayer);

        ai.maxSpeed *= chargeSpeed;

        if (hit.collider != null) 
            ai.destination = hit.point;
        else
            ai.destination = transform.position + (target - transform.position).normalized * chargeDistance;

        ai.SearchPath();
    }
}
