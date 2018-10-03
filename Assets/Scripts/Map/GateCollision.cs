using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollision : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Physics2D.IgnoreLayerCollision(13, 14, true);
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
