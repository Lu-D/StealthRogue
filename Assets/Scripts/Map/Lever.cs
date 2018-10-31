using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    private PlayerControl player;
    private Light sceneLight;
    private AudioSource mainAudio;

   void Start () {

       player = GameObject.Find("Player").GetComponent<PlayerControl>();
       sceneLight = GameObject.Find("Scene Light").GetComponent<Light>();
       mainAudio = GameObject.Find("Player").GetComponent<AudioSource>();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown("f") && player.isSpotted)
            {
                player.isSpotted = false;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                mainAudio.Stop();
                mainAudio.PlayOneShot(player.audioClips[1]);
                sceneLight.intensity = .4f;
            }
        }
    }
}
