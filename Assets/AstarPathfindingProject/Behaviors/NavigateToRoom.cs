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
        ai.isStopped = false;
        ai.destination = findRandomRoom();
        ai.SearchPath();
    }

    private void OnDisable()
    {
        ai.isStopped = true;
        roomreached = false;
        roomTarget = "empty";
    }

    private void Update()
    {
        if (enemy.mapLocation == roomTarget)
            roomreached = true;
    }

    public Vector3 findRandomRoom()
    {
        PlayerSearchableArea searchableMaps;
        searchableMaps = enemy.player.transform.Find("Searchable Area").GetComponent<PlayerSearchableArea>();

        GameObject selectedRoom = searchableMaps.returnRandomRoom();
        roomTarget = selectedRoom.transform.parent.name;
        return selectedRoom.transform.position;
    }
}
