﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour {
    public float blastRadius;
    public float enemyAlertRadius;
    public float fuseLength;
    public Message lookAtMe;
	
	void Update () {
        fuseLength -= Time.deltaTime;
        if(fuseLength < 0)
        {
            explode();
        }
	}

    void explode()
    {
        //destroy objects in blastRadius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach(Collider2D hit in hitColliders)
        {
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Obstacle")
            {
                Destroy(hit.gameObject);
            }
        }
        Collider2D[] alertColliders = Physics2D.OverlapCircleAll(transform.position, enemyAlertRadius);

        //alert nearby enemies
        foreach (Collider2D alert in alertColliders)
        {
            if (alert.gameObject.tag == "Enemy")
            {
                lookAtMe = new Message(gameObject.transform.position, "look at me");
                alert.gameObject.GetComponent<EnemyControl>().messageReceiver = lookAtMe;
            }
        }
        Destroy(this.gameObject);
    }
}
