using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : Node, ITappable
{

    public bool canTransformToTNT = false;
    public Sprite normalSprite;
    public Sprite tntVersionSprite;
    public GameObject tntPrefab;

    public void turnIntoTNTVersion()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = tntVersionSprite;
        canTransformToTNT = true;
    }

    public override bool BlowUp()
    {
        return true;
    }

    public override bool Shake()
    {
        return false;
    }

    public bool Tap()
    {
        HashSet<Node> visited = new HashSet<Node>();
        List<Vector2Int> toDestroyIndexes = new List<Vector2Int>();
        List<Vector2Int> nodesToShake = new List<Vector2Int>();
        Dfs(xIndex, yIndex, visited, toDestroyIndexes, nodesToShake);

        int comboCount = 0;
        foreach (Node n in visited)
        {
            if (n.nodeType == nodeType)
            {
                comboCount++;
            }
        }

        if (comboCount >= 2)
        {
            foreach (Vector2Int pos in nodesToShake)
            {
                int i = pos.x;
                int j = pos.y;
                if (Board.instance.board[i, j].Shake())
                {
                    toDestroyIndexes.Add(pos);
                }
            }

            foreach (Vector2Int pos in toDestroyIndexes)
            {
                int i = pos.x;
                int j = pos.y;
                Board.instance.board[i, j].DestroySelf();
            }

            if(canTransformToTNT)
            {
                SpawnTNT();
            }

            return true;
        }
        else
        {
            return false;
        }

    }

    private void SpawnTNT()
    {
        GameObject newTNT = Instantiate(tntPrefab, Board.instance.GetGamePos(xIndex, yIndex), Quaternion.identity);
        Node nodeComponent = newTNT.GetComponent<Node>();
        Board.instance.board[xIndex, yIndex] = nodeComponent;
        nodeComponent.setIndexes(xIndex, yIndex);
        
    }

    public void Dfs(int i, int j, HashSet<Node> visited, List<Vector2Int> toDestroyIndexes, List<Vector2Int> nodesToShake)
    {
        if (i < 0 || j < 0 || i >= Board.instance.width || j >= Board.instance.height || visited.Contains(Board.instance.board[i, j]))
        {
            return;
        }

        Node node = Board.instance.board[i, j];
        if (node == null) {
            return;
        }
        if (node.nodeType != nodeType) {
            nodesToShake.Add(new Vector2Int(i, j));
            return;
        }

        toDestroyIndexes.Add(new Vector2Int(i, j));
        visited.Add(node);
        Dfs(i - 1, j, visited, toDestroyIndexes, nodesToShake);
        Dfs(i + 1, j, visited, toDestroyIndexes, nodesToShake);
        Dfs(i, j - 1, visited, toDestroyIndexes, nodesToShake);
        Dfs(i, j + 1, visited, toDestroyIndexes, nodesToShake);
    }
}

