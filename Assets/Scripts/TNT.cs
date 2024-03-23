using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TNT : FallingNode, ITappable
{

    public int explosionRadius = 2;
    public bool alreadyExploded = false;

    public override bool BlowUp()
    {
        return false;
    }

    public override bool Shake()
    {
        return false;
    }

    public bool Tap()
    {
        alreadyExploded = true;
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
}


