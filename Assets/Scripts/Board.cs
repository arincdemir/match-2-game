using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public float nodesGap;
    public float nodesOffset;

    public static Board instance;

    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject tntPrefab;
    public GameObject boxPrefab;
    public GameObject stonePrefab;
    public GameObject vasePrefab;
  

    public Node[,] board;

    private Queue<NodeToBeDropped> nodesToBeDroppedQueue = new Queue<NodeToBeDropped>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        board = new Node[width, height];
        FillBoard();
        CheckBoardForTNTs();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Node>() is ITappable)
            {
                bool broke = hit.collider.gameObject.GetComponent<ITappable>().Tap();
                if (broke)
                {
                    MakeNodesFallDown();
                    float fillingTime = FillFromTop();
                    Invoke("CheckBoardForTNTs", fillingTime);
                }

            }
        }
    }

    public void FillBoard()
    {
        for(int i = 0; i < width; i++) { 
            for(int j = 0; j < height; j++)
            {
                int randint = Random.Range(0, 8);
                Vector3 pos = GetGamePos(i, j);
                GameObject prefab = bluePrefab;
                switch(randint)
                {
                    case 0:
                        prefab = bluePrefab;
                        break;
                    case 1:
                        prefab = redPrefab;
                        break;
                    case 2:
                        prefab = greenPrefab;
                        break;
                    case 3:
                        prefab = yellowPrefab;
                        break;
                    case 4:
                        prefab = tntPrefab;
                        break;
                    case 5:
                        prefab = boxPrefab;
                        break;
                    case 6:
                        prefab = stonePrefab;
                        break;
                    case 7:
                        prefab = vasePrefab;
                        break;
                }
                GameObject newObject = Instantiate(prefab, pos, Quaternion.identity);
                Node newNode = newObject.GetComponent<Node>();
                newNode.setIndexes(i, j);
                board[i, j] = newNode;
            }
        }
    }

    private void ResetTNTCubes()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Node node = board[i, j];
                if (node != null && node is Cube)
                {
                    ((Cube)node).turnIntoNormalVersion();
                }
            }
        }
    }

    private void CheckBoardForTNTs ()
    {
        ResetTNTCubes();

        HashSet<Node> visited = new HashSet<Node>();
        List<Vector2Int> placeHolder = new List<Vector2Int>();
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++)
            {
                Node node = board[i, j];
                if (node is Cube && !visited.Contains(node))
                {
                    Cube curCube = (Cube) node;
                    List<Vector2Int> sameColoreds = new List<Vector2Int>();
                    curCube.Dfs(i, j, visited, sameColoreds, placeHolder);
                    if (sameColoreds.Count >= 5)
                    {
                        foreach (Vector2Int pos in sameColoreds)
                        {
                            ((Cube)board[pos.x, pos.y]).turnIntoTNTVersion();
                        }
                    }
                }
            }
        }
    }

    public Vector3 GetGamePos(int i, int j)
    {
        float x = (i - (float)width / 2) * nodesGap + transform.position.x + nodesOffset;
        float y = (j - (float)height / 2) * nodesGap + transform.position.y + nodesOffset;
        Vector3 pos = new Vector3(x, y, -j);
        return pos;
    }

 
    void MakeNodesFallDown()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (board[i, j] is FallingNode)
                {
                    ((FallingNode)board[i, j]).FallDown();
                }
            }
        }
    }

    float FillFromTop()
    {
        bool nullFound = true;
        int maxBlocksToWait = 0;
        HashSet<int> iSToVisit = new HashSet<int>();
        for (int i = 0; i < width; i++)
        {
            iSToVisit.Add(i);
        }

        for (int j = height - 1; j >= 0 && nullFound; j--)
        {
            nullFound = false;
            List<int> iSToDrop = new List<int>();
            foreach (int i in iSToVisit)
            {
                if (board[i, j] == null)
                {
                    nullFound = true;
                    int randint = Random.Range(0, 4);
                    Vector3 pos = GetGamePos(i, height);
                    GameObject prefab = bluePrefab;
                    switch (randint)
                    {
                        case 0:
                            prefab = bluePrefab;
                            break;
                        case 1:
                            prefab = redPrefab;
                            break;
                        case 2:
                            prefab = greenPrefab;
                            break;
                        case 3:
                            prefab = yellowPrefab;
                            break;
                    }
                    int blocksToWait = height - 1 - j;
                    maxBlocksToWait = blocksToWait;
                    nodesToBeDroppedQueue.Enqueue(new NodeToBeDropped { prefab = prefab, pos = pos, i = i });
                    Invoke("createAndFallDownNewNode", blocksToWait * FallingNode.timeToFallOneBlockDown());
                }
                else
                {
                    iSToDrop.Add(i);
                }
            }
            foreach (int i in iSToDrop)
            {
                iSToVisit.Remove(i);
            }
        }


        return (maxBlocksToWait + 1) * FallingNode.timeToFallOneBlockDown();
         
    }

    private void createAndFallDownNewNode() {
        NodeToBeDropped n = nodesToBeDroppedQueue.Dequeue();
        GameObject newObject = Instantiate(n.prefab, n.pos, Quaternion.identity);
        Node newNode = newObject.GetComponent<Node>();
        newNode.setIndexes(n.i, height);
        ((FallingNode)newNode).FallDown();
    }

}
