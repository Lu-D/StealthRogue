using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision {

    GameObject player;
    Transform enemyTransform;
    float detectionAngle;
    float detectionDistance;
    float fovResolution;

    public EnemyVision(GameObject target, Transform transform, float detectionAngle, float detectionDistance, float resolution)
    {
        player = target;
        enemyTransform = transform;
        this.detectionAngle = detectionAngle;
        this.detectionDistance = detectionDistance;
        fovResolution = resolution;
    }

    public bool checkVision()
    {
        Vector3 targetDir = player.transform.position - enemyTransform.position;
        if ((Vector3.Angle(targetDir, enemyTransform.up) < (detectionAngle/2)) && (Vector3.Distance(player.transform.position, enemyTransform.position) < detectionDistance))
        {
            RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, targetDir);
            if (hit.transform.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void drawFOV()
    {
        int stepCount = Mathf.RoundToInt(fovResolution * detectionAngle);
        float stepAngle = detectionAngle / stepCount;

        for (int i = 0; i <= stepCount; ++i)
        {

        }

    }
}
