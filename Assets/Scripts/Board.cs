using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

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
  

    public Node[,] board;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        board = new Node[width, height];
        FillBoard();
        CheckBoardForTNTs();
    }

    public void FillBoard()
    {
        for(int i = 0; i < width; i++) { 
            for(int j = 0; j < height; j++)
            {
                int randint = Random.Range(0, 4);
                float x = (i - (float)width / 2) * nodesGap + transform.position.x + nodesOffset;
                float y = (j - (float)height / 2) * nodesGap + transform.position.y + nodesOffset;
                Vector3 pos = new Vector3(x, y, -j);
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
                }
                GameObject newObject = Instantiate(prefab, pos, Quaternion.identity);
                Node newNode = newObject.GetComponent<Node>();
                newNode.setIndexes(i, j);
                board[i, j] = newNode;
            }
        }
    }

    private void CheckBoardForTNTs ()
    {
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

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Node>() is ITappable)
            {
                hit.collider.gameObject.GetComponent<ITappable>().Tap();
            }

        }
    }
}
