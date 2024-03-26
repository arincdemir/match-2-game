using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Node
{
    public override bool BlowUp()
    {
        // the box gets destroyed if blown up
        Board.instance.boxCount--;
        return true;
    }

    public override bool Shake()
    {
        // the box gets destroyed if a nearby block shakes it
        Board.instance.boxCount--;
        return true;
    }

}
