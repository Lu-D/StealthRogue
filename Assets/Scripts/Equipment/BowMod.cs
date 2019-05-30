using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BowMod : MonoBehaviour
{
    public int rangeMod = 0;
    public int dmgMod = 0;
    public int aimSpeedMod = 0;
    public int arrowShootCountMod = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();
            BowData bowData = player.bowData;

            bowData.modifiers["Range"] += rangeMod;
            bowData.modifiers["Dmg"] += dmgMod;
            bowData.modifiers["Aim Speed"] += aimSpeedMod;
            bowData.modifiers["Arrow Shoot Count"] += arrowShootCountMod;

            Log(rangeMod, "Range");
            Log(dmgMod, "Dmg");
            Log(aimSpeedMod, "Aim Speed");
            Log(arrowShootCountMod, "Arrow Shoot Count");

            Destroy(gameObject);
        }
    }

    private void Log(int modifier, string modifierName)
    {
        if (modifier > 0)
            Debug.Log(modifierName + " Raised");
        else if (modifier < 0)
            Debug.Log(modifierName + " Lowered");
    }
}
