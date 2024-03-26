using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Node
{
    public override bool BlowUp()
    {
        // get destroyed if it is blown up
        Board.instance.stoneCount--;
        return true;
    }

    public override bool Shake()
    {
        // do not get destroyed if shook
        return false;
    }

}
