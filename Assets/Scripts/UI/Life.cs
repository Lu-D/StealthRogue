using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour {

    PlayerControl playerHealth;

	// Use this for initialization
	void Start () {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
