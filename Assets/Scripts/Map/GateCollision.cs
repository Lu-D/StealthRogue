using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollision : MonoBehaviour {

    Collider2D player;

    void Awake()
    {
        player = GameObject.Find("SceneCollider").GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.enabled = false;
            player.enabled = true;
            Physics2D.IgnoreLayerCollision(13, 14, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Physics2D.IgnoreLayerCollision(13, 14, false);
        }
    }
}
