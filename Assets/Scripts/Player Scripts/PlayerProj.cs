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
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);    

        if(collision.gameObject.tag == "Enemy")
        {
            BEnemy enemy = collision.gameObject.GetComponent<BEnemy>();
            enemy.health -= damage;

            GameObject.Find("Sound Manager").GetComponent<SoundManager>().PlayOneShot("Arrow_Hit");

            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            enemyRigidbody.transform.Translate((transform.position - source.transform.position).normalized * .2f, Space.World);

            if (collision.gameObject.name == "Boss")
            {
                var eventObj = new Events.ComeToMeEvent(source.transform.position);
                eventObj.addListener(collision.gameObject);
                Events.EventManager.Instance.addEvent(eventObj, 0f);
            }
        }
    }
}
