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
        health--;
        if (health <= 0)
        {
            Board.instance.vaseCount--;
            return true;
        }
        GetComponent<SpriteRenderer>().sprite = damagedSprite;
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
