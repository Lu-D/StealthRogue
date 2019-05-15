using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerSearchableArea : MonoBehaviour
{
    private CircleCollider2D circle;
    private float originalRadius = 13f;
    private float changeRate = 0;

    public mode radiusMode = mode.same;
    public float radius;

    public enum mode
    {
        increasing = 0,
        decreasing,
        same
    }

    private void Awake()
    {
        radius = originalRadius;
        circle = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        switch(radiusMode){
            case mode.increasing:
                radius += Time.deltaTime * changeRate;
                DebugUtility.DrawCircle(transform.position, radius, Color.red);
                circle.radius = radius * (1 / transform.parent.gameObject.transform.localScale.x);
                return;
            case mode.decreasing:
                radius -= Time.deltaTime * changeRate;
                DebugUtility.DrawCircle(transform.position, radius, Color.yellow);
                circle.radius = radius * (1 / transform.parent.gameObject.transform.localScale.x);
                return;
            case mode.same:
                DebugUtility.DrawCircle(transform.position, radius, Color.green);
                return;
        }
    }

    public bool isIncreasing()
    {
        return radiusMode == mode.increasing;
    }

    public bool isDecreasing()
    {
        return radiusMode == mode.decreasing;
    }

    public bool isSame()
    {
        return radiusMode == mode.same;
    }

    public void setIncreasing(float rate = .1f)
    {
        changeRate = rate;

        radiusMode = mode.increasing;
    }

    public void setDecreasing(float rate = .1f)
    {
        changeRate = rate;

        radiusMode = mode.decreasing;
    }

    public void setSame()
    {
        changeRate = 0;

        radiusMode = mode.same;
    }

    public void decreaseSearchArea()
    {
        radius -= 2f;
    }

    public void reset()
    {
        radius = originalRadius;
        radiusMode = mode.decreasing;
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

