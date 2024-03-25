using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Node
{
    public override bool BlowUp()
    {
        Board.instance.boxCount--;
        return true;
    }

    public override bool Shake()
    {
        Board.instance.boxCount--;
        return true;
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
