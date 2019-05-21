using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Bow
{
    private float baseRange = 2.1f;
    private int baseDmg = 1;
    private int baseShootCount = 1;
    private float baseAimSpeed = 2.5f;

    private Dictionary<string, int> modifiers;

    public Bow()
    {
        modifiers = new Dictionary<string, int>();

        modifiers.Add("Range", 0);
        modifiers.Add("Dmg", 0);
        modifiers.Add("Shoot Count", 0);
        modifiers.Add("Aim Speed", 0);
    }

    public float getRange()
    {
        return baseRange + modifiers["Range"];
    }

    public int getDmg()
    {
        return baseDmg + modifiers["Dmg"];
    }

    public int getShootCount()
    {
        return baseShootCount + modifiers["Shoot Count"];
    }

    public float getAimSpeed()
    {
        return baseAimSpeed + modifiers["Aim Speed"];
    }
}

