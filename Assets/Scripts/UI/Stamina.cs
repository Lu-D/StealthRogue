using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class Stamina : MonoBehaviour {

    public Slider slider;
    public PlayerControl player;

	// Use this for initialization
	void Start () {
        slider = gameObject.GetComponent<Slider>();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = player.currStamina;
	}
}
