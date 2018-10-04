using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy vision class held by enemy control script
public class EnemyVision {

    GameObject player;
    Transform enemyTransform;
    float detectionAngle;
    float detectionDistance;
    float fovResolution;
    public MeshFilter viewMeshFilter;
    public Mesh viewMesh;

    //Initialize fields
    public EnemyVision(GameObject target, Transform transform, float detectionAngle, float detectionDistance, float resolution, MeshFilter meshFilter)
    {
        player = target;
        enemyTransform = transform;
        this.detectionAngle = detectionAngle;
        this.detectionDistance = detectionDistance;
        fovResolution = resolution;
        viewMeshFilter = meshFilter;

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    //checkVision
    //shoot raycast at player when in detection angle and distance
    //sets player as spotted if raycast hits
    //return if player is spotted or not
    public bool checkVision()
    {
        Vector3 targetDir = player.transform.position - enemyTransform.position;
        if ((Vector3.Angle(targetDir, enemyTransform.up) < (detectionAngle/2) + 5) && (Vector3.Distance(player.transform.position, enemyTransform.position) < detectionDistance))
        {
            RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, targetDir, detectionDistance);
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

    //drawFOV
    //draws enemy vision
    //creates 3d mesh with vertices made by raycast collisions
    public void drawFOV()
    {
        int stepCount = Mathf.RoundToInt(fovResolution * detectionAngle);
        float stepAngle = detectionAngle / stepCount;
        List<Vector2> viewPoints = new List<Vector2>();

        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = enemyTransform.rotation.eulerAngles.z + 90f - detectionAngle/2 + stepAngle * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; ++i)
        {
            vertices[i + 1] = enemyTransform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = i + 2;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = 0;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    //ViewCast()
    //Casts ray in direction angle and returns information about the raycase
    ViewCastInfo ViewCast(float angle)
    {
        Vector3 dir = dirFromAngle(angle);

        RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, dir, detectionDistance);
        if (hit.collider != null){
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else
        {
            return new ViewCastInfo(false, enemyTransform.position + dir * detectionDistance, detectionDistance, angle);
        }
    }

    //dirFromAngle()
    //Returns Vector3 when given angles in degrees
    public Vector3 dirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    //ViewCastInfo
    //contatins information returned by a raycast
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }
}
