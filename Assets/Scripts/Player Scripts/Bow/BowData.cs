using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BowData
{
    private float baseRange = 2.1f;
    private int baseDmg = 1;
    private float baseAimSpeed = 2.5f;
    private int baseArrowShootCount = 1;

    public Dictionary<string, int> modifiers;

    public BowData()
    {
        modifiers = new Dictionary<string, int>();
        modifiers.Add("Range", 0);
        modifiers.Add("Dmg", 0);
        modifiers.Add("Aim Speed", 0);
        modifiers.Add("Arrow Shoot Count", 0);
    }

    public float getRange()
    {
        return baseRange + modifiers["Range"] * 1f;
    }

    public int getDmg()
    {
        return baseDmg + modifiers["Dmg"] * 1;
    }

    public float getAimSpeed()
    {
        return baseAimSpeed + modifiers["Aim Speed"] * 1f;
    }

    public int getArrowShootCount()
    {
        return baseArrowShootCount + modifiers["Arrow Shoot Count"] * 1;
    }
}
