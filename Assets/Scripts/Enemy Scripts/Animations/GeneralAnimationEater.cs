using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GeneralAnimationEater : MonoBehaviour
{
    //All filler animation stuff
    private Quaternion up; //to keep texture upright
    private GameObject front;
    private Pathfinding.IAstarAI pathFinder;
    private struct animDirection
    {
        public static float x;
        public static float y;
    }

    private void Awake()
    {
        //For filler animation
        up = transform.rotation;
        front = new GameObject("front");
        front.transform.position = Vector3.up + transform.position;
        front.transform.parent = gameObject.transform;
        pathFinder = GetComponent<Pathfinding.IAstarAI>();
    }

    private void Update()
    {
        updateAnim();
    }

    //FILLER animation for now, REPLACE
    private void updateAnim()
    {
        Animator anim = transform.Find("Texture").GetComponent<Animator>();
        Vector3 directionVector = front.transform.position - transform.position;

        if (directionVector.x > 0.01f)
        {
            animDirection.x = 1;
        }
        else if (directionVector.x < -.01f)
        {
            animDirection.x = -1;
        }
        else
            animDirection.x = 0;


        if (directionVector.y > .01f)
        {
            animDirection.y = 1;
        }
        else if (directionVector.y < -.01f)
        {
            animDirection.y = -1;
        }
        else
        {
            animDirection.y = 0;
        }

        anim.SetFloat("MoveX", animDirection.x);
        anim.SetFloat("MoveY", animDirection.y);

        //keeps animation texture upright
        Transform texture = transform.Find("Texture");
        //if (texture.rotation != up)
        //    texture.rotation = up;


        //determines whether or not to play idle animation
        if (transform.GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0) || pathFinder.canMove)
            anim.SetBool("isMoving", true);
        else
            anim.SetBool("isMoving", false);
    }
}

