using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour {
    public float blastRadius;
    public float enemyAlertRadius;
    public float fuseLength;
	
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
        var dieEvent = new Events.DamageEvent(1);
        foreach(Collider2D hit in hitColliders)
        {
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Obstacle")
            {
                dieEvent.addListener(hit.gameObject);
            }
        }

        Collider2D[] alertColliders = Physics2D.OverlapCircleAll(transform.position, enemyAlertRadius);
        var lookEvent = new Events.lookAtMeEvent(transform.position);
        //alert nearby enemies
        foreach (Collider2D alert in alertColliders)
        {
            if(alert.gameObject.tag == "Enemy")
            {
                lookEvent.addListener(alert.gameObject);
            }
        }

        Events.EventManager.Instance.addEvent(lookEvent, 0);
        Events.EventManager.Instance.addEvent(dieEvent, 0);

        Destroy(this.gameObject);
    }
}
