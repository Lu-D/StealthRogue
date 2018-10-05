using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    //public Sprite leverOff;

    //private PlayerControl player;
    //private Light sceneLight;
    //private AudioSource mainAudio;

	// Use this for initialization
	void Start () {
        //player = GameObject.Find("Player").GetComponent<PlayerControl>();
        //sceneLight = GameObject.Find("Scene Light").GetComponent<Light>();
        //mainAudio = GameObject.Find("Player").GetComponent<AudioSource>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("we good");
            //if (Input.GetKeyDown("f") && player.isSpotted)
            //{
            //    player.isSpotted = false;
            //    gameObject.GetComponent<SpriteRenderer>().sprite = leverOff;
            //    mainAudio.Stop();
            //    sceneLight.intensity = 0f;
            //}
        }
    }
}
