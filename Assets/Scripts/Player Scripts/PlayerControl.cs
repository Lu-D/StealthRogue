﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour {

    public float moveSpeed;
    public bool isSpotted;
    public bool invincible;

    public bool playerMoving;
    public bool isAttacking;
    public Vector2 lastMove;

    public int equip;
    public GameObject equipment;
    public bool capturedBullet;
    public GameObject gun;
    public GameObject bullet;
    public string mapLocation;
    public bool changingLocation;

    public AudioClip[] audioClips;

    private Rigidbody2D myRigidbody;
    private Animator anim;
    private AudioSource myAudioSource;
    private Light light;

	// Use this for initialization
	void Start () {
        lastMove.y = -1;
        isSpotted = false;

        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();

        equip = 0;
        capturedBullet = false;
        gun = transform.Find("Gun").gameObject;

        light = transform.Find("Player Light").gameObject.GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        playerMoving = false;

        //moves player left and right
        if ((Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1) && !isAttacking)
        {
            //moves player according to moveSpeed horizontally
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, myRigidbody.velocity.y);

            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }

        //otherwise don't move in x direction
        else
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }

        //moves player up and down
        if ((Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) && !isAttacking)
        {
            //moves player according to moveSpeed vertically
            //Transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));

            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);

            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        //otherwise don't move in y direction
        else
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0f);
        }

        if (Input.GetKeyDown("space") && !isAttacking)
        {
            isAttacking = true;
            Invoke("setAttackBack", .5f);
        }
        if (Input.GetKeyDown("e"))
        {
            equipment.GetComponent<EquipmentController>().onKeyDown();
        }

        //assigns values to animator
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetBool("IsAttacking", isAttacking);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);

        if (changingLocation)
        {
            StartCoroutine(adjustLight());
        }
    }

    private void setAttackBack()
    {
        isAttacking = false;
    }

    private void playAttackSound()
    {
        myAudioSource.PlayOneShot(audioClips[0], 1f);
    }

    public IEnumerator adjustLight()
    {
        if(mapLocation == "")
        {
            for(int i = 54; i >= 30; i-=2)
            {
                light.spotAngle = i;
                yield return null;
            }
            changingLocation = false;
        }
        else
        {
            for (int i = 30; i <= 54; i+=2)
            {
                light.spotAngle = i;
                yield return null;
            }
            changingLocation = false;
        }
    }

    public void OnCollisionEnter2DHurt(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionEnter2DKnife(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if(equipment != null)
        {
            equipment.GetComponent<EquipmentController>().onCollide(collision);
        }
    }
}
