using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    [SerializeField]
    public NodeType nodeType;
    public int xIndex;
    public int yIndex;

    public GameObject particle;
    public int particleCount = 3;


    // set the board indexes of the node
    public void setIndexes(int x, int y) {
        xIndex = x; yIndex = y;
    }


    // returns true if node needs to be destroyed after another node gets destroyed next to it
    public abstract bool Shake();

    // returns true if node needs to be destroyed after blown up by tnt
    public abstract bool BlowUp();

    // creates a number of particles before destroying itself
    public void DestroySelf() {
        Vector3 particlePos = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 15f);
        for (int i = 0; i < particleCount; i++)
        {
            Instantiate(particle, particlePos, Quaternion.identity);
        }

        Board.instance.board[xIndex, yIndex] = null;
        Invoke("DestroySelfNow",  0.05f);
    }

    public void DestroySelfNow()
    {
        
        Destroy(gameObject);
    }

   
}

public enum NodeType
{
    BLUE,
    GREEN,
    RED,
    YELLOW,
    BLUETNT,
    GREENTNT,
    YELLOWTNT,
    REDTNT,
    TNT,
    BOX,
    STONE,
    VASE
}
