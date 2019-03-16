using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatePlayer : MonoBehaviour {

    private PlayerControl player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.mapLocation = transform.parent.name;
            player.changingLocation = false;
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<BEnemy>().mapLocation = transform.parent.name;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.mapLocation = "";
            player.changingLocation = true;
        }
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<BEnemy>().mapLocation = "";
        }
    }
}
