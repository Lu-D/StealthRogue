using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour {
    public float blastRadius;
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach(Collider2D hit in hitColliders)
        {
            if(hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Obstacle")
            {
                Destroy(hit.gameObject);
            }
        }
        Destroy(this.gameObject);
    }
}
