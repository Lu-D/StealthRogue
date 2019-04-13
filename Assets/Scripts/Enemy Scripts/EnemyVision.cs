using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enemy vision class held by enemy control script
public class EnemyVision : MonoBehaviour
{
    public float detectionAngle = 90;
    public float detectionDistance = 2;

    private BEnemy enemy;
    private float fovResolution = 1;
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    private GameObject visualObject;

    //holds tags and number of object seen
    private Dictionary<string, int> watchList;

    //Initialize fields
    private void Awake()
    {
        enemy = GetComponent<BEnemy>();
        visualObject = transform.Find("FOV Visual").transform.gameObject;
        viewMeshFilter = visualObject.GetComponent<MeshFilter>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        watchList = new Dictionary<string, int>();
    }

    private void OnEnable()
    {
        visualObject.SetActive(true);
    }

    private void OnDisable()
    {
        visualObject.SetActive(false);
    }

    private void Update()
    {
        createFOV();
    }

    private void LateUpdate()
    {
        watchList.Clear();
    }

    public bool hasSeen(string gameObjectTag)
    {
        return watchList.ContainsKey(gameObjectTag);
    }

    public int numTimesSeen(string gameObjectTag)
    {
        if (hasSeen(gameObjectTag))
            return watchList[gameObjectTag];
        else
            return 0;
    }
    
    //Debugging purposes
    public void logSeen()
    {
        foreach(var x in watchList)
        {
            Debug.Log(x.Key + ": " + x.Value);
        }
    }

    //createFOV
    //draws enemy vision
    //creates 3d mesh with vertices made by raycast collisions
    private void createFOV()
    {
        int stepCount = Mathf.RoundToInt(fovResolution * detectionAngle);
        float stepAngle = detectionAngle / stepCount;
        List<Vector2> viewPoints = new List<Vector2>();

        for (int i = 0; i <= stepCount; ++i)
        {
            float angle = enemy.transform.rotation.eulerAngles.z + 90f - detectionAngle/2 + stepAngle * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            updateVision(newViewCast);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; ++i)
        {
            vertices[i + 1] = enemy.transform.InverseTransformPoint(viewPoints[i]);
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

    //updateVision
    //updates list of objects that is in enemy vision
    private void updateVision(ViewCastInfo info)
    {
        if (info.hit && info.objectHit.tag != "Untagged")
        {
            if (watchList.ContainsKey(info.objectHit.tag))
                ++watchList[info.objectHit.tag];
            else
                watchList.Add(info.objectHit.tag, 0);
        }
    }

    //ViewCast()
    //Casts ray in direction angle and returns information about the raycast
    private ViewCastInfo ViewCast(float angle)
    {
        Vector3 dir = dirFromAngle(angle);

        //visualization ignores projectiles and enemies
        LayerMask viewCastLayer = ~(1 << LayerMask.NameToLayer("Projectile") | 1 << LayerMask.NameToLayer("Enemy"));


        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, dir, detectionDistance, viewCastLayer);
        if (hit.collider != null){
            return new ViewCastInfo(true, hit.point, hit.distance, angle, hit.collider.gameObject);
        }
        else
        {
            return new ViewCastInfo(false, enemy.transform.position + dir * detectionDistance, detectionDistance, angle, null);
        }
    }

    //dirFromAngle()
    //Returns Vector3 when given angles in degrees
    private Vector3 dirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    //ViewCastInfo
    //contatins information returned by a raycast
    private struct ViewCastInfo
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
