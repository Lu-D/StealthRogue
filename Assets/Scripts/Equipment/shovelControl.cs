using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shovelControl : EquipmentController {

	void Awake () {
        player = GameObject.FindWithTag("Player");
        pControl = player.GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onKeyDown();
        }
    }

    //equip
    public override void onKeyDown()
    {

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            pControl.equip = (int)Equipment.laserSword;
            transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            transform.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
