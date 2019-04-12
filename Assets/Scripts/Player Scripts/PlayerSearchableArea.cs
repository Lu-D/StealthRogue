using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerSearchableArea : MonoBehaviour
{
    private CircleCollider2D circle;
    private float originalRadius = 13f;
    private float radius;
    private float hintsUntilPlayerFound = 6;
    private float decreaseStep;
    public bool gettingHunted = true;

    private void Awake()
    {
        radius = originalRadius;
        decreaseStep = radius / hintsUntilPlayerFound;
        circle = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if(!gettingHunted)
        {
            DebugUtility.DrawCircle(transform.position, radius, Color.green);
            if (circle != null) circle.radius = radius;
        }
        else
        {
            radius += Time.deltaTime;
            DebugUtility.DrawCircle(transform.position, radius, Color.red);

            if (radius > 10f) 
            {
                gettingHunted = false;
                resetRadius();
            }

        }
    }

    public void decreaseSearchArea()
    {
        radius -= decreaseStep;
    }

    public void resetRadius()
    {
        radius = originalRadius;
    }

    public void zeroRadius()
    {
        radius = 0;
    }

    public Vector2 returnRandomPoint()
    {
        float R = radius * Mathf.Sqrt(Random.Range(0f, 1f));
        float theta = Random.Range(0f, 1f) * 2 * Mathf.PI;

        return new Vector2(transform.position.x + R * Mathf.Cos(theta), transform.position.y + R * Mathf.Sin(theta));
    }

    public GameObject returnRandomRoom()
    {
        LayerMask layerMask = (1 << LayerMask.NameToLayer("Room"));
        Physics2D.queriesHitTriggers = true;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        Physics2D.queriesHitTriggers = false;

        int randomIndex = Random.Range(0, hitColliders.Length);

        return hitColliders[randomIndex].transform.gameObject;
    }
}

