using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

    public int healthNumber;
    private int playerHealth;
    private PlayerControl player;
    private Image image;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        image = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        playerHealth = player.health;
        if (playerHealth < healthNumber)
        {
            image.enabled = false;
        }
        else 
        {
            image.enabled = true;
        }
	}
}
