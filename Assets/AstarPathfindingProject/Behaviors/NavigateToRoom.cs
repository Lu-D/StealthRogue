using System.Collections;
using UnityEngine;

public class NavigateToRoom : MonoBehaviour
{
    private string roomTarget = "empty";
    private Pathfinding.IAstarAI ai;
    private Eater enemy;

    private bool roomreached = false;
    public bool roomReached{
        get{ return roomreached; }
    }

    private void Awake()
    {
        ai = GetComponent<Pathfinding.IAstarAI>();
        enemy = GetComponent<Eater>();
    }

    private void OnEnable()
    {
        if (ai != null) ai.destination = findRandomRoom();
    }

    private void OnDisable()
    {
        roomreached = false;
        roomTarget = "empty";
    }

    private void Update()
    {
        if(enemy != null)
        {
            if (enemy.mapLocation == roomTarget)
                roomreached = true;
        }
    }

    public Vector3 findRandomRoom()
    {
        //int randomMap = Random.Range(0, GameObject.Find("Maps").transform.childCount);
        //roomTarget = GameObject.Find("Maps").transform.GetChild(randomMap).name;
        //var roomBounds = GameObject.Find("Maps").transform.GetChild(randomMap).transform.position;
        //return roomBounds;

        PlayerSearchableArea searchableMaps;
        if (enemy != null) searchableMaps = enemy.player.transform.Find("Searchable Area").GetComponent<PlayerSearchableArea>();
        else return Vector3.positiveInfinity;

        if (searchableMaps != null)
        {
            GameObject selectedRoom = searchableMaps.returnRandomRoom();
            roomTarget = selectedRoom.transform.parent.name;
            return selectedRoom.transform.position;
        }
        else return Vector3.positiveInfinity;
    }
}
