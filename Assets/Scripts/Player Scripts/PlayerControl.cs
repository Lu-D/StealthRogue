using System.Collections;
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
        if (Input.GetKeyDown("e") && capturedBullet)
        {
            gun.GetComponent<GunControl>().Fire(bullet, 0);
            capturedBullet = false;
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
            Debug.Log("playerhit");
            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionEnter2DKnife(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("enemycontact");
            Destroy(collision.gameObject);
        }
        switch (equip)
        {
            case 2:
                //laserSword
                if (collision.gameObject.tag == "Projectile")
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
                    BProjectile bulletControl = collision.gameObject.GetComponent<BProjectile>();
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = (bulletControl.source.transform.position - transform.position).normalized * bulletControl.projSpeed;
                    bulletControl.source = transform.gameObject;
                }
                return;
            case 4:
                //shovel
                if (collision.gameObject.tag == "Obstacle")
                {
                    Destroy(collision.gameObject);
                }
                return;
            case 5:
                //darkSword
                if (collision.gameObject.tag == "Projectile")
                {
                    Destroy(collision.gameObject);
                    capturedBullet = true;
                }
                return;
        }
    }
}
