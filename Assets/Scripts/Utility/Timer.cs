using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Timer
{
    private float startTime;

    public Timer()
    {
        startTime = Mathf.Infinity;
    }

    public void startTimer()
    {
        startTime = Time.time;
    }

    public float getElapsedTime()
    {
        return Time.time - startTime;
    }

    public void endTimer()
    {
        startTime = Mathf.Infinity;
    }


}

