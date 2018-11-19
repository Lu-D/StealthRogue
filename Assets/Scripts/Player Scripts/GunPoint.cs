using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPoint : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Takes mouse position and points gun towards location
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos = mousePos - objectPos;

        Quaternion lookMouse = Quaternion.LookRotation(mousePos, Vector3.forward);
        lookMouse.x = 0;
        lookMouse.y = 0;
        transform.rotation = lookMouse;
    }
}
