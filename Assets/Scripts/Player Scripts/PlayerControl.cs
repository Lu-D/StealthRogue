﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour {

    public float defaultSpeed;
    public float sprintSpeed;
    public bool isSpotted;
    public bool invincible;
    public float dashDist;
    public float dashSpeed;
    public float timeSlowFactor;

    public bool playerMoving;
    public bool isAttacking;
    public Vector2 lastMove;
    private bool canControl = true; //set to false if you want to override player controls using stun(float)

    public int equip;
    public GameObject equipment;
    public bool capturedBullet;
    public GameObject gun;
    public GameObject bullet;
    public string mapLocation;
    public bool changingLocation;
    public int health;
    public int maxStamina;
    public float maxBowRange;
    private GameObject crosshair;

    public AttackPatterns attackPatterns;

    public int currStamina;
    public PlayerSearchableArea searchableArea;

    public AudioClip[] audioClips;

    Task rollOneShot;

    private Rigidbody2D myRigidbody;
    private Animator anim;
    public AudioSource myAudioSource;
    private float moveSpeed;
    private Renderer myRenderer;

	// Use this for initialization
	private void Awake () {
        lastMove.y = -1;
        isSpotted = false;

        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        myRenderer = GetComponent<Renderer>();

        attackPatterns = new AttackPatterns();

        equip = 0;
        capturedBullet = false;
        gun = transform.Find("Gun").gameObject;
        crosshair = transform.Find("Crosshair").gameObject;

        currStamina = maxStamina;

        moveSpeed = defaultSpeed;

        searchableArea = transform.Find("Searchable Area").GetComponent<PlayerSearchableArea>();
    }
	
	// Update is called once per frame
	private void Update () {
        if (Input.GetKeyDown("space") && (rollOneShot == null || !rollOneShot.Running) && currStamina > 60)
        {
            rollOneShot = new Task(roll());
            currStamina -= 60;
        }

        //Inputs for equipment
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(equip == 0)
        //    {
        //        anim.SetTrigger("IsAttacking");
        //    }
        //    else
        //    {
        //        equipment.GetComponent<EquipmentController>().onKeyDown();
        //    }
        //}

        //if(Input.GetMouseButtonDown(1) && equip != 0)
        //{
        //    equipment.GetComponent<EquipmentController>().throwEquip(equip);
        //}

        if(Input.GetMouseButtonDown(1))
        {
            crosshair.SetActive(true);
        }
        else if(Input.GetMouseButton(1))
        {
            //Push crosshair towards mouse
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos = mousePos - objectPos;


            if (mousePos.normalized != (crosshair.transform.position - transform.position).normalized)
            {
                crosshair.transform.position = mousePos.normalized * (crosshair.transform.position - transform.position).magnitude + transform.position;
            }

            if ((crosshair.transform.position - transform.position).magnitude < maxBowRange)
            {
                crosshair.transform.Translate(mousePos.normalized * Time.deltaTime * 2f);
            }
        }
        else if(Input.GetMouseButtonUp(1))
        {
            crosshair.SetActive(false);
            crosshair.transform.position = transform.position;
        }

        playerMove();

        if (currStamina != maxStamina)
        {
            ++currStamina;
        }

    }

    public IEnumerator stun(float stunTime)
    {
        canControl = false;
        yield return new WaitForSeconds(stunTime);
        canControl = true;
    }

    private void playerMove()
    {
        if (!canControl) return;

        playerMoving = false;

        //moves player left and right
        if ((Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1) && Input.GetAxisRaw("Vertical") == 0 && !isAttacking)
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
        if ((Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) && Input.GetAxisRaw("Horizontal") ==0 && !isAttacking) 
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

        //player moves diagonal
        if(Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") != 0)
        {
            //moves player according to moveSpeed horizontally
            //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            myRigidbody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);

            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        //assigns values to animator
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
    }

    private void playAttackSound()
    {
        myAudioSource.PlayOneShot(audioClips[0], 1f);
    }

    public IEnumerator roll()
    {
        Vector3 target = transform.position + new Vector3(lastMove.x, lastMove.y, 0) * dashDist;

        float rollSpeedX = lastMove.x * dashSpeed;
        float rollSpeedY = lastMove.y * dashSpeed;

        float rollTime = Vector3.Distance(transform.position, target) / dashSpeed;
        float startTime = Time.time;

        //sets player to invincible
        invincible = true;

        //changes color to show player is invincible
        //save initial material and color
        Material m = this.myRenderer.material;
        Color32 c = this.myRenderer.material.color;
        //switch color to grey
        this.myRenderer.material = null;
        this.myRenderer.material.color = Color.white;

        //rolls ends after hitting target distance or rolltime runs out
        while (Vector3.Distance(transform.position, target) > .1f && Time.time - startTime < rollTime)
        {
            myRigidbody.velocity = new Vector3(rollSpeedX, rollSpeedY, 0);
            yield return null;
        }

        //player no longer invincible
        invincible = false;
        //set player back to original color
        this.myRenderer.material = m;
        this.myRenderer.material.color = c;
    }

    public void OnCollisionEnter2DKnife(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<BEnemy>().health = 0;
        }
        if(equipment != null)
        {
            equipment.GetComponent<EquipmentController>().onCollide(collision);
        }
    }
}
