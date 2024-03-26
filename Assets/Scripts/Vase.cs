using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : FallingNode
{
    [SerializeField]
    public Sprite damagedSprite;
    private int health = 2;
    public override bool BlowUp()
    {
        // drop your health if blown up
        // show the damaged sprite
        // if health is 0, get destroyed
        health--;
        if (health <= 0)
        {
            Board.instance.vaseCount--;
            return true;
        }
        GetComponent<SpriteRenderer>().sprite = damagedSprite;
        return false;
    }

    public override bool Shake()
    {
        // drop your health if shook down
        // show the damaged sprite
        // if health is 0, get destroyed
        health--;
        if (health <= 0)
        {
            Board.instance.vaseCount--;
            return true;
        }
        GetComponent<SpriteRenderer>().sprite = damagedSprite;
        return false;
    }

}
