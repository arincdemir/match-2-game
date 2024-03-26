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
    public int moves;

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

    public int boxCount;
    public int stoneCount;
    public int vaseCount;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // if there are still nodes to drop, dont process the click
            if (nodesToBeDroppedQueue.Count > 0)
            {
                return;
            }
            // send a ray and find the node that was hit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // if the hit node is tappable, process the tap
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Node>() is ITappable)
            {
                bool broke = hit.collider.gameObject.GetComponent<ITappable>().Tap();
                if (broke)
                {
                    moves--;
                    MakeNodesFallDown();
                    float fillingTime = FillFromTop();
                    Invoke("CheckBoardForTNTs", fillingTime);
                }

            }
        }
    }

    // return the count of obstacles as an int array
    public int[] GetObstacleCounts()
    {
        int[] ans = new int[3];
        ans[0] = boxCount;
        ans[1] = stoneCount;
        ans[2] = vaseCount;
        return ans;
    }

    // given the level object, set the board up
    public void Initialize(Level level)
    {
        instance = this;
        width = level.grid_width;
        height = level.grid_height;
        moves = level.move_count;
        transform.localScale = new Vector3(width * nodesGap, height * nodesGap, 0);
        board = new Node[width, height];
        FillBoard(level.grid);
        CheckBoardForTNTs();

    }

    // return the number of moves left as an int
    public int getMoveCount()
    {
        return moves;
    }


    // given the grid from the level file, fill the board up
    public void FillBoard(string[] grid)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject prefab = bluePrefab;
                int index = i + j * width;
                switch (grid[index])
                {
                    case "r":
                        prefab = redPrefab;
                        break;
                    case "g":
                        prefab = greenPrefab;
                        break;
                    case "b":
                        prefab = bluePrefab;
                        break;
                    case "y":
                        prefab = yellowPrefab;
                        break;
                    case "t":
                        prefab = tntPrefab;
                        break;
                    case "bo":
                        prefab = boxPrefab;
                        boxCount++;
                        break;
                    case "s":
                        prefab = stonePrefab;
                        stoneCount++;
                        break;
                    case "v":
                        prefab = vasePrefab;
                        vaseCount++;
                        break;
                    case "rand":
                        // create a random cube
                        int randint = Random.Range(0, 4);
                        switch (randint)
                        {
                            case 0:
                                prefab = bluePrefab;
                                break;
                            case 1:
                                prefab = greenPrefab;
                                break;
                            case 2:
                                prefab = yellowPrefab;
                                break;
                            case 3:
                                prefab = redPrefab;
                                break;
                        }
                        break;
                }

                // create the new object and link it to our board
                Vector3 pos = GetGamePos(i, j);
                GameObject newObject = Instantiate(prefab, pos, Quaternion.identity);
                board[i,j] = newObject.GetComponent<Node>();
                board[i, j].setIndexes(i, j);
            }
        }
    }


    // turn all cubes into their non-tnt mode
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

    // check the cubes on the board and turn them into their tnt versions if necessary
    private void CheckBoardForTNTs ()
    {
        ResetTNTCubes();

        HashSet<Node> visited = new HashSet<Node>();
        HashSet<Pair<int, int>> placeHolder = new HashSet<Pair<int, int>>();
        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++)
            {
                // if the node is not already visited, run the dfs algorithm on it
                Node node = board[i, j];
                if (node is Cube && !visited.Contains(node))
                {
                    // call the dfs algorithm to get the same colored cubes that are connected
                    Cube curCube = (Cube) node;
                    HashSet<Pair<int, int>> sameColoreds = new HashSet<Pair<int, int>>();
                    curCube.Dfs(i, j, visited, sameColoreds, placeHolder);
                    if (sameColoreds.Count >= 5)
                    {
                        foreach (Pair<int, int> pos in sameColoreds)
                        {
                            ((Cube)board[pos.First, pos.Second]).turnIntoTNTVersion();
                        }
                    }
                }
            }
        }
    }

    
    // return the converted position on the scene for indexes on the board
    public Vector3 GetGamePos(int i, int j)
    {
        float x = (i - (float)width / 2) * nodesGap + transform.position.x + nodesOffset;
        float y = (j - (float)height / 2) * nodesGap + transform.position.y + nodesOffset;
        // use -j as the z coordinate in order to prioritize the rendering of the cubes that are on top
        Vector3 pos = new Vector3(x, y, -j);
        return pos;
    }

    // make the nodes fall down to empty spaces
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

    // fill the empty places on the board from the top.
    // returns the total time needed for the board to be filled.
    float FillFromTop()
    {
        // here the code goes horizontally from top to bottom
        bool nullFound = true;
        int maxBlocksToWait = 0;
        // fill up the i indexes that we want to visit, which is all of them in the beginning
        HashSet<int> iSToVisit = new HashSet<int>();
        for (int i = 0; i < width; i++)
        {
            iSToVisit.Add(i);
        }

        // we go from top to bottom. we check if an empty place was found in the previous iteration with nullfound
        for (int j = height - 1; j >= 0 && nullFound; j--)
        {
            nullFound = false;
            // we will drop the i indexes that are not empty on this j level
            List<int> iSToDrop = new List<int>();
            foreach (int i in iSToVisit)
            {
                if (board[i, j] == null)
                {
                    // spawn a random cube
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
                    // add a delay such that the previous blocks to drop are out of the way
                    int blocksToWait = height - 1 - j;
                    maxBlocksToWait = blocksToWait;
                    // put them in a queue and wait for the delay for calling the function that initializes them
                    nodesToBeDroppedQueue.Enqueue(new NodeToBeDropped { prefab = prefab, pos = pos, i = i });
                    Invoke("createAndFallDownNewNode", blocksToWait * FallingNode.timeToFallOneBlockDown());
                }
                else
                {
                    // if there are no nulls found, we dont need to check this i index again
                    iSToDrop.Add(i);
                }
            }
            foreach (int i in iSToDrop)
            {
                iSToVisit.Remove(i);
            }
        }


        // 
        return (maxBlocksToWait + 1) * FallingNode.timeToFallOneBlockDown();
         
    }

    // create the new object that was put on the queue
    private void createAndFallDownNewNode() {
        NodeToBeDropped n = nodesToBeDroppedQueue.Dequeue();
        GameObject newObject = Instantiate(n.prefab, n.pos, Quaternion.identity);
        Node newNode = newObject.GetComponent<Node>();
        newNode.setIndexes(n.i, height);
        ((FallingNode)newNode).FallDown();
    }

}
