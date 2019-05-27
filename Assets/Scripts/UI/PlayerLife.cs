using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    public int healthNumber;
    private int playerHealth;
    private PlayerControl player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }
	
	// Update is called once per frame
	void Update () {
        playerHealth = player.health;
        if (playerHealth < healthNumber)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
	}
}
