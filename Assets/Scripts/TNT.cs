using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : FallingNode, ITappable
{

    public int explosionRadius = 2;
    public bool alreadyExploded = false;

    public override bool BlowUp()
    {
        // do not get destroyed if blown up
        // we will cover that in the tap function below
        return false;
    }

    public override bool Shake()
    {
        // do not get destroyed if shook
        return false;
    }

    public bool Tap()
    {
        // if there is a tnt close by, increase the explosion to 7x7
        if (neighborTNTExists())
        {
            explosionRadius = 3;
        }
        // set the tnt to be alreadyExploded, preventing infinite recursions of 2 tnts blowing each other out
        alreadyExploded = true;
        // get all the nodes in the explosion radius
        for (int i = -explosionRadius; i <= explosionRadius; i++)
        {
            for(int j = -explosionRadius; j <= explosionRadius; j++)
            {
                if (i + xIndex >= 0 && i + xIndex < Board.instance.width && j + yIndex >= 0 && j + yIndex < Board.instance.height)
                {
                    Node node = Board.instance.board[i + xIndex, j + yIndex];
                    if (node && node.BlowUp())
                    {
                        node.DestroySelf();
                        Board.instance.board[i + xIndex, j + yIndex] = null;
                    }
                    else if (node && node is TNT && !((TNT)node).alreadyExploded)
                    {
                        ((TNT)node).Tap();
                    }
                }
            }
        }

        DestroySelf();
        return true;
    }


    // check if a tnt exist next to it
    private bool neighborTNTExists() { 
        if (xIndex - 1 >= 0 && Board.instance.board[xIndex - 1, yIndex] is TNT) {  return true; }
        else if (xIndex + 1 < Board.instance.width && Board.instance.board[xIndex + 1, yIndex] is TNT) { return true; }
        else if (yIndex - 1 >= 0 && Board.instance.board[xIndex, yIndex - 1] is TNT) { return true; }
        else if (yIndex + 1 < Board.instance.height && Board.instance.board[xIndex, yIndex + 1] is TNT) { return true; }
        return false;
    }
}


