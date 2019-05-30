using System.Collections;
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
    public GunControl gun;
    public GameObject bullet;
    public string mapLocation;
    public bool changingLocation;
    public int health = 3;
    private GameObject crosshair;
    public GameObject flashlight;

    public PlayerSearchableArea searchableArea;
    public BowData bowData;

    private SoundManager soundManager;

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

        equip = 0;
        capturedBullet = false;
        gun = transform.Find("Gun").gameObject.GetComponent<GunControl>();
        crosshair = transform.Find("Crosshair").gameObject;

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        bowData = new BowData();

        moveSpeed = defaultSpeed;

        searchableArea = transform.Find("Searchable Area").GetComponent<PlayerSearchableArea>();

        flashlight = transform.Find("Flashlight").gameObject;
    }
	
	//Tracks player inputs
	private void Update () {

        //Flashlight following mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos = mousePos - objectPos;
        Quaternion lookMouse = Quaternion.LookRotation(mousePos, Vector3.back);
        lookMouse.x = 0;
        lookMouse.y = 0;
        flashlight.transform.rotation = Quaternion.RotateTowards(transform.rotation, lookMouse, 360f);

        //Input for rolling
        if (Input.GetKeyDown("space") && (rollOneShot == null || !rollOneShot.Running))
        {
            rollOneShot = new Task(roll());
        }

        //Inputs for firing arrow
        if(Input.GetMouseButtonDown(1))
        {
            crosshair.SetActive(true);

            soundManager.Play("Pull_Bow_Back");
        }
        else if(Input.GetMouseButton(1))
        {
            //Push crosshair towards mouse
            mousePos = Input.mousePosition;
            mousePos.z = 0f;
            objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos = mousePos - objectPos;


            if (mousePos.normalized != (crosshair.transform.position - transform.position).normalized)
            {
                crosshair.transform.position = mousePos.normalized * (crosshair.transform.position - transform.position).magnitude + transform.position;
            }
            if ((crosshair.transform.position - transform.position).magnitude < bowData.getRange())
            {
                crosshair.transform.Translate(mousePos.normalized * Time.deltaTime * bowData.getAimSpeed());
            }

            if (Input.GetMouseButtonDown(0))
            {
                float angleRestrictions = 40f;
                float currAngle = angleRestrictions;
                float angleStepCount = angleRestrictions * 2 / (bowData.getArrowShootCount()-1);

                if (bowData.getArrowShootCount() == 1)
                    gun.Fire(bullet, 0, bowData.getDmg());
                else
                {
                    for (int i = 0; i < bowData.getArrowShootCount(); ++i)
                    {
                        gun.Fire(bullet, currAngle, bowData.getDmg());
                        currAngle -= angleStepCount;
                    }
                }

                soundManager.PlayOneShot("Shoot_Bow");

                soundManager.Stop("Pull_Bow_Back");
                soundManager.Play("Pull_Bow_Back");

                crosshair.transform.position = transform.position;
            }
        }
        else if(Input.GetMouseButtonUp(1))
        {
            crosshair.SetActive(false);
            crosshair.transform.position = transform.position;
            soundManager.Stop("Pull_Bow_Back");
        }

        //input for basic player movement
        playerMove();
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
        Material m = myRenderer.material;
        Color32 c = myRenderer.material.color;
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
        myRenderer.material = m;
        myRenderer.material.color = c;
    }
}
