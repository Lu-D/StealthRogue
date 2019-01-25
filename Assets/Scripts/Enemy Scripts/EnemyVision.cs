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
    public bool objectInVision;

    //Initialize fields
    public EnemyVision(BEnemy owner)
    {
        player = owner.player.gameObject;
        enemyTransform = owner.transform;
        detectionAngle = owner.detectionAngle;
        detectionDistance = owner.detectionDistance;
        fovResolution = owner.fovResolution;
        viewMeshFilter = owner.viewMeshFilter.GetComponent<MeshFilter>();

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
        int stepCount = Mathf.RoundToInt(fovResolution * detectionAngle);
        float stepAngle = detectionAngle / stepCount;

        if (!objectInVision)
            return false;

        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = enemyTransform.rotation.eulerAngles.z + 90f - detectionAngle / 2 + stepAngle * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            if (newViewCast.hit)
            {
                //if vision sees player
                if (newViewCast.objectHit.transform.tag == "Player")
                {
                    return true;
                }
                //if vision sees enemy that sees player
                if (newViewCast.objectHit.transform.tag == "Enemy" &&
                    newViewCast.objectHit.transform.gameObject.GetComponent<BEnemy>().playerSpotted)
                {
                    return true;
                }
                //if vision sees dead enemy
                if (newViewCast.objectHit.transform.tag == "Enemy" &&
                    newViewCast.objectHit.transform.gameObject.GetComponent<BEnemy>().isDead)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //drawFOV
    //draws enemy vision
    //creates 3d mesh with vertices made by raycast collisions
    public void drawFOV()
    {
        int stepCount = Mathf.RoundToInt(fovResolution * detectionAngle);
        float stepAngle = detectionAngle / stepCount;
        List<Vector2> viewPoints = new List<Vector2>();
        objectInVision = false;

        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = enemyTransform.rotation.eulerAngles.z + 90f - detectionAngle/2 + stepAngle * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
            if (newViewCast.hit)
            {
                objectInVision = true;
            }
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
    //Casts ray in direction angle and returns information about the raycast
    ViewCastInfo ViewCast(float angle)
    {
        Vector3 dir = dirFromAngle(angle);

        //visualization ignores other guards - 11 and projectiles - 12
        LayerMask viewCastLayer = ~((1 << 12));

        RaycastHit2D hit = Physics2D.Raycast(enemyTransform.position, dir, detectionDistance, viewCastLayer);
        if (hit.collider != null){
            return new ViewCastInfo(true, hit.point, hit.distance, angle, hit.collider.gameObject);
        }
        else
        {
            return new ViewCastInfo(false, enemyTransform.position + dir * detectionDistance, detectionDistance, angle, null);
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
        public GameObject objectHit;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dist, float _angle, GameObject _objectHit)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
            objectHit = _objectHit;
        }
    }
}
