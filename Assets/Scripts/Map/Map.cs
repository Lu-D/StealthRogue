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

        if (mapBounds.Contains(player.transform.position))
        {
            player.mapLocation = transform.parent.name;
            Debug.Log("happens");
        }
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
