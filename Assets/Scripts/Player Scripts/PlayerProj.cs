using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProj : BProjectile
{
    GameObject crosshair;
    float range;
    
    // Start is called before the first frame update
    protected new void Awake()
    {
        source = GameObject.Find("Player");

        crosshair = source.transform.Find("Crosshair").transform.gameObject;

        range = (crosshair.transform.position - source.transform.position).magnitude;
    }

    // Update is called once per frame
    protected new void Update()
    {
        if ((transform.position - source.transform.position).magnitude > range)
        {
            Destroy(gameObject);
        }
    }

    //collision method
    protected new void OnCollisionEnter2D(Collision2D collision)
    {       
        if(collision.gameObject.tag == "Enemy")
        {
            BEnemy enemy = collision.gameObject.GetComponent<BEnemy>();
            --enemy.health;

            if(collision.gameObject.name == "Eater Boss")
            {
                var eventObj = new Events.ComeToMeEvent(source.transform.position);
                eventObj.addListener(collision.gameObject);
                Events.EventManager.Instance.addEvent(eventObj, 0f);
            }

            Destroy(gameObject);
        }
    }
}
