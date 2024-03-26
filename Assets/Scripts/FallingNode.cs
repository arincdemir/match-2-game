using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class FallingNode : Node
{

    public static float gravity = 4f;

    // moves node to the farthest empty place that it can reach
    public void FallDown()
    {
        int j = yIndex - 1;
        while (j >= 0 && Board.instance.board[xIndex, j] == null)
        {
            j--;
        }
        j++;
        if (j != yIndex)
        {
            if (yIndex < Board.instance.height) {
                Board.instance.board[xIndex, yIndex] = null;
            }
            Board.instance.board[xIndex, j] = this;
            Vector3 newPos = Board.instance.GetGamePos(xIndex, j);
            // moves the node using cubic easing, which creates a realistic falling effect
            transform.DOMove(newPos, fallingDownTimeLength(yIndex - j)).SetEase(Ease.InCubic);
            yIndex = j;
        }
    }


    // calculates the time it takes to fall using newton's physics
    private static float fallingDownTimeLength(int dist)
    {
        return Mathf.Sqrt((float)dist / gravity);
    }

    // calculates the time to fall one block down
    public static float timeToFallOneBlockDown()
    {
        return fallingDownTimeLength(1);
    }
}
