using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class FallingNode : Node
{

    public static float gravity = 4f;
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
            transform.DOMove(newPos, fallingDownTimeLength(yIndex - j)).SetEase(Ease.InCubic);
            yIndex = j;
        }
    }


    private static float fallingDownTimeLength(int dist)
    {
        return Mathf.Sqrt((float)dist / gravity);
    }

    public static float timeToFallOneBlockDown()
    {
        return fallingDownTimeLength(1);
    }
}
