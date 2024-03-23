using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    [SerializeField]
    public NodeType nodeType;
    public int xIndex;
    public int yIndex;

    public void setIndexes(int x, int y) {
        xIndex = x; yIndex = y;
    }

    public abstract bool Shake();

    public abstract bool BlowUp();

    public void DestroySelf() {
        Board.instance.board[xIndex, yIndex] = null;
        Invoke("DestroySelfNow",  0.05f);
    }

    public void DestroySelfNow()
    {
        
        Destroy(gameObject);
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
