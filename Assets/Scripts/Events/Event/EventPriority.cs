using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * Highest number is highest priority. Events will replace each other
 * if priority matches or is greater. 
 * 
 * Naming will be Px if priorities are added to the list. To add a priority
 * in between already existing Px priorities, use Pxax. For example, to add 
 * a priority between P1 and P2, the naming will go like P1a0, P1a1, P1a2, etc.
 * To add priorities between for example P1a0 and P1a1, use P1a0b0, P1a0b1, etc.
 * */
namespace Events
{
    public enum Priority
    {
        P0 = 0,
        P1,
        P2,
        P3,
        P4,
        P5,
        P6,
        P7,
        P8,
        P9,
        P10,
        P100
    }
}
