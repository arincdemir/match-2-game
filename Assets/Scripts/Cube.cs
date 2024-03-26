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
        // gets destroyed if blown up
        return true;
    }

    public override bool Shake()
    {
        // does not get destroyed if shook
        return false;
    }


    // processes the tap on a cube, returns true if the tap was successful
    // which means there were two or more connected cubes
    public bool Tap()
    {

        HashSet<Node> visited = new HashSet<Node>();
        HashSet<Pair<int, int>> sameColoredConnected = new HashSet<Pair<int, int>>();
        HashSet<Pair<int, int>> nodesToShake = new HashSet<Pair<int, int>>();
        Dfs(xIndex, yIndex, visited, sameColoredConnected, nodesToShake);

        int comboCount = sameColoredConnected.Count;
        
        // if there are two or more connected same colored cubes
        if (comboCount >= 2)
        {
            // shake the nodes and destroy them if necessary
            foreach (Pair<int, int> pos in nodesToShake)
            {
                int i = pos.First;
                int j = pos.Second;
                if (Board.instance.board[i, j].Shake())
                {
                    Board.instance.board[i, j].DestroySelf();
                }
            }

            // destroy the connected same colored cubes
            foreach (Pair<int, int> pos in sameColoredConnected)
            {
                int i = pos.First;
                int j = pos.Second;
                Board.instance.board[i, j].DestroySelf();
            }

            // if the current node can transform into a tnt, do it
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


    // spawn a tnt at this index
    private void SpawnTNT()
    {
        GameObject newTNT = Instantiate(tntPrefab, Board.instance.GetGamePos(xIndex, yIndex), Quaternion.identity);
        Node nodeComponent = newTNT.GetComponent<Node>();
        Board.instance.board[xIndex, yIndex] = nodeComponent;
        nodeComponent.setIndexes(xIndex, yIndex);
        
    }


    // does deep first search on the board
    // fills the visited nodes
    // fills the same colored nodes that are connected
    // fills nodesToShake with nodes that were reached but are not the same color
    public void Dfs(int i, int j, HashSet<Node> visited, HashSet<Pair<int, int>> connectedSameColored, HashSet<Pair<int, int>> nodesToShake)
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

        connectedSameColored.Add(new Pair<int, int>(i, j));
        visited.Add(node);
        Dfs(i - 1, j, visited, connectedSameColored, nodesToShake);
        Dfs(i + 1, j, visited, connectedSameColored, nodesToShake);
        Dfs(i, j - 1, visited, connectedSameColored, nodesToShake);
        Dfs(i, j + 1, visited, connectedSameColored, nodesToShake);
    }
}

