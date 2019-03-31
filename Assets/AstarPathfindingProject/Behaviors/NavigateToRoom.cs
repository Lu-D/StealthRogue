using System.Collections;
using UnityEngine;

public class NavigateToRoom : MonoBehaviour
{
    private string roomTarget;
    private Pathfinding.IAstarAI ai;
    private Eater eater;

    private bool roomreached = false;
    public bool roomReached{
        get{ return roomreached; }
    }

    private void OnEnable()
    {
        ai = GetComponent<Pathfinding.IAstarAI>();
        if(ai != null) ai.destination = findRandomRoom();
        eater = GetComponent<Eater>();
    }

    private void OnDisable()
    {
        roomreached = false;
    }

    private void Update()
    {
        if(eater != null)
        {
            if (eater.mapLocation == roomTarget)
                roomreached = true;
        }
    }

    public Vector3 findRandomRoom()
    {
        int randomMap = Random.Range(0, GameObject.Find("Maps").transform.childCount);
        roomTarget = GameObject.Find("Maps").transform.GetChild(randomMap).name;
        var roomBounds = GameObject.Find("Maps").transform.GetChild(randomMap).transform.position;
        return roomBounds;
    }
}
