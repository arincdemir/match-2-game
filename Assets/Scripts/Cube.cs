using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : FallingNode, ITappable
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

    public void turnIntoNormalVersion()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;
        canTransformToTNT = false;
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
        HashSet<Pair<int, int>> toDestroyIndexes = new HashSet<Pair<int, int>>();
        HashSet<Pair<int, int>> nodesToShake = new HashSet<Pair<int, int>>();
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
            foreach (Pair<int, int> pos in nodesToShake)
            {
                int i = pos.First;
                int j = pos.Second;
                if (Board.instance.board[i, j].Shake())
                {
                    toDestroyIndexes.Add(pos);
                }
            }

            foreach (Pair<int, int> pos in toDestroyIndexes)
            {
                int i = pos.First;
                int j = pos.Second;
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

    public void Dfs(int i, int j, HashSet<Node> visited, HashSet<Pair<int, int>> toDestroyIndexes, HashSet<Pair<int, int>> nodesToShake)
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
            nodesToShake.Add(new Pair<int, int>(i, j));
            return;
        }

        toDestroyIndexes.Add(new Pair<int, int>(i, j));
        visited.Add(node);
        Dfs(i - 1, j, visited, toDestroyIndexes, nodesToShake);
        Dfs(i + 1, j, visited, toDestroyIndexes, nodesToShake);
        Dfs(i, j - 1, visited, toDestroyIndexes, nodesToShake);
        Dfs(i, j + 1, visited, toDestroyIndexes, nodesToShake);
    }
}

