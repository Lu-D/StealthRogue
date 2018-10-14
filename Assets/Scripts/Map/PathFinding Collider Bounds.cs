using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class PathFindingColliderBounds : MonoBehaviour {

    Collider2D collider;


    private void Awake()
    {
        ////Expands the bounds along the z axis
        //   Collider2D collider = GetComponent<Collider2D>();
        //collider.bounds.Expand(Vector3.forward * 1000);
        //var guo = new GraphUpdateObject(collider.bounds);
    }

}
