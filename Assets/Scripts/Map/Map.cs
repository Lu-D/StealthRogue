using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    private PlayerControl player;

    [HideInInspector]
    public Bounds mapBounds;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();

        mapBounds = GetComponent<Collider2D>().bounds;

        if (mapBounds.min.x <= player.transform.position.x &&
            mapBounds.max.x >= player.transform.position.x &&
            mapBounds.min.y <= player.transform.position.y &&
            mapBounds.max.y >= player.transform.position.y)
            player.mapLocation = transform.parent.name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.mapLocation = transform.parent.name;
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<BEnemy>().mapLocation = transform.parent.name;
        }
    }
}
