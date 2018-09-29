using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentController : MonoBehaviour {
    //variables Equipments modify
    public float playerSpeedBonus = 0;

    public abstract void onKeyDown();
	
}
