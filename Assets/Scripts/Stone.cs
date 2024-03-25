using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Node
{
    public override bool BlowUp()
    {
        Board.instance.stoneCount--;
        return true;
    }

    public override bool Shake()
    {
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
