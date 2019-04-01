using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PlayerSearchableArea : MonoBehaviour
{
    private CircleCollider2D circle;
    private float radius = 26f;
    private float hintsUntilPlayerFound = 6;
    private float decreaseStep;

    private void Awake()
    {
        decreaseStep = radius / hintsUntilPlayerFound;
        circle = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        
        DebugUtility.DrawCircle(transform.position, radius, Color.yellow);
        if (circle != null) circle.radius = radius;
    }

    public void decreaseSearchArea()
    {
        radius -= decreaseStep;
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

