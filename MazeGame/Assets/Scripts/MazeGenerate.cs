using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Unlike the other scripts in this assignment that were 100original and authentic,
 * a various amount of the code in this MazeGenerate script was not originally created by me.
 * The logic behind the generating function were same as the ones introduced in class.
 * However, some of the coding were adopted from a maze-generate tutorial, 
 * The overall creation of the maze still uses the methods introduced in class. 
 * Link of the tutorial: https://www.bilibili.com/video/BV1Rv411i74c#reply3540014736
 * 
 * The path searching dynamic algorithm coding, random entry and exit coding, are 100% original
 */
public class MazeGenerate : MonoBehaviour
{

    public int row, col, wallWidth;
    public Transform wall, currentTile, normalTile, swall;
    public int startIndex, endIndex;
    public List<Cell> onlyPath;


    public class Cell 
    {
        public bool visited;
        public bool leftC, rightC, DownC, UpC;
        public int posIndex;


        public Cell(bool visited, bool l, bool r, bool d, bool c, int pos)
        {
            this.visited = visited;
            leftC = l;
            rightC = r;
            DownC = d;
            UpC = c;
            posIndex = pos;
        }
    }

    private Cell[] maze;
    private int visitedIndex;
    //private Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
    private List<int> store1 = new List<int>();
    private List<int> store2 = new List<int>();
    private List<Transform> prev = new List<Transform>();
    System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        maze = new Cell[row * col];
        for (int i = 0; i < maze.Length; i++)
        {
            maze[i] = new Cell(false, false, false, false, false, i);
        }
        int x = rand.Next(row);
        int y = rand.Next(col);
        store1.Add(x);
        store2.Add(y);
        maze[y * row + x].visited = true;
        visitedIndex = 1;

        Initialize();


    }

    // Update is called once per frame
    void Update()
    {
        RB_Algorithm();
        onlyPath = path(maze[startIndex], maze[endIndex], maze); 
    }
    void Initialize()
    {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                for(int wy = 0; wy < wallWidth; wy++)
                {
                    for(int wx = 0; wx < wallWidth; wx++)
                    {
                        Vector3 v3 = new Vector3(x * (wallWidth + 1) + wx - 7, 1, y * (wallWidth + 1) + wy + 19);
                        Instantiate(normalTile, v3, Quaternion.identity);
                        Vector3 v3_1 = new Vector3(x * (wallWidth + 1) + wx - 7, 1, y * (wallWidth + 1) + wallWidth + 19);
                        Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth - 7, 1, y * (wallWidth + 1) + wx + 19);
                        Instantiate(wall, v3_1, Quaternion.identity);
                        Instantiate(wall, v3_2, Quaternion.identity);
                    }
                }
                /*
                 * Currently the wall took care the row and column side. 
                 * However, the intersection of row and column are still empty, therefore should be added
                 * with an additional step
                 *  W W W E
                 *  T T T W
                 *  T T T W
                 *  T T T W
                 *   for W = added wall, T = added tile, 
                 *   E is the missing piece, the interaction and an additional step should take place
                 */
                Instantiate(wall, new Vector3((x+1) * (wallWidth + 1) - 1 - 7, 1, (y+1) * (wallWidth + 1) -1 + 19), Quaternion.identity);
            }
        }
        for(int i = 0; i < row * (wallWidth +1); i++)
        {
            Instantiate(wall, new Vector3(i - 7, 1, (col - col) * (wallWidth + 1) - 1 + 19), Quaternion.identity);
        }
        for(int j = -1; j < col * (wallWidth + 1); j++)
        {
            Instantiate(wall, new Vector3((row - row) * (wallWidth + 1) - 1 - 7, 1, j+ 19), Quaternion.identity);
        }

        //Destroy wall for enter and exit
        int randx = UnityEngine.Random.Range(1, row);
        int randy = UnityEngine.Random.Range(1, col);
        this.startIndex = 0 * row + randx - 1;
        this.endIndex = 4 * row + randy - 1;


        for (int x = 1; x < wallWidth + 1; x++)
        {
            Vector3 v3 = new Vector3(randx * (wallWidth + 1) - x -1 - 7, 0 + 1,  0 -1 + 19);
            if (checkPosEmpty(v3))
            {
                Instantiate(swall, new Vector3(randx * (wallWidth + 1) - x - 1 - 7, 0, 0 - 1 + 19), Quaternion.identity);
            }
        }
        for (int y = 1; y < wallWidth + 1; y++)
        {
            Vector3 v3 = new Vector3(randy * (wallWidth + 1) -y - 1 - 7, 0 + 1, col * (wallWidth + 1) - 1 + 19);
            if (checkPosEmpty(v3))
            {
                Instantiate(swall, new Vector3(randy * (wallWidth + 1) - y - 1 - 7, 0, col * (wallWidth + 1) - 1 + 19), Quaternion.identity);
            }
        }
        
    }

    void RB_Algorithm()
    {
        List<int> neighbours = new List<int>();
        Func<int, int, uint> lookAt = (px, py) => (uint)((store1[store1.Count - 1] + px) + (store2[store2.Count - 1] + py) * row);

        if (visitedIndex < row * col)
        {
            //up
            if (store2[store2.Count - 1] > 0 && maze[lookAt(0, -1)].visited == false)
            {
                neighbours.Add(0);
            }
            //right
            if (store1[store1.Count - 1] < (row - 1) && maze[lookAt(+1, 0)].visited == false)
            {
                neighbours.Add(1);
            }
            //down
            if (store2[store2.Count - 1] < (col - 1) && maze[lookAt(0, +1)].visited == false)
            {
                neighbours.Add(2);
            }
            //left
            if ( store1[store1.Count - 1] > 0 && maze[lookAt(-1, 0)].visited == false )
            {
                neighbours.Add(3);
            }
            
            //if no neighbour then neighbours.count = 0
            if(neighbours.Count > 0)
            {
                //there are neighbours
                int nextTile = neighbours[rand.Next(neighbours.Count)];
                /* 4 possibilities
                 * for each possibility the new node extends to a new direction
                 * ex. Right.
                 * 1) update the path to right to true; 
                 * 2) update the path from the right to left, to true
                 * 3) update the node to right as visited
                 * 4)a add the new node to visited
                 */
                switch (nextTile)
                {
                    case 0: //up
                        maze[lookAt(0, 0)].UpC = true;
                        maze[lookAt(0, -1)].DownC = true;
                        maze[lookAt(0, -1)].visited = true;
                        store1.Add(store1[store1.Count - 1] + 0);
                        store2.Add(store2[store2.Count - 1] - 1);
                        break;
                    case 1: //right
                        maze[lookAt(0, 0)].rightC = true;
                        maze[lookAt(+1, 0)].leftC = true;
                        maze[lookAt(+1, 0)].visited = true;
                        store1.Add(store1[store1.Count - 1] + 1);
                        store2.Add(store2[store2.Count - 1] + 0);
                        break;
                    case 2://down
                        maze[lookAt(0, 0)].DownC = true;
                        maze[lookAt(0, +1)].UpC = true;
                        maze[lookAt(0, +1)].visited = true;
                        store1.Add(store1[store1.Count - 1] + 0);
                        store2.Add(store2[store2.Count - 1] + 1);
                        break;
                    case 3: //left
                        maze[lookAt(0, 0)].leftC = true;
                        maze[lookAt(-1, 0)].rightC = true;
                        maze[lookAt(-1, 0)].visited = true;
                        store1.Add(store1[store1.Count - 1] - 1);
                        store2.Add(store2[store2.Count - 1] + 0);
                        break;
                }
                visitedIndex++;
            }
            else
            {
                store1.RemoveAt(store1.Count - 1);
                store2.RemoveAt(store2.Count - 1);
            }
            DrawMaze();
        }
    }
    void DrawMaze()
    {
        for(int x = 0; x < row; x++)
        {
            for(int y = 0; y< col; y++)
            {
                for(int p = 0; p < wallWidth; p++)
                {
                    Vector3 v3 = new Vector3(x * (wallWidth + 1) + p - 7, 0 + 1, y * (wallWidth + 1) + wallWidth + 19);
                    Vector3 v3_2 = new Vector3(x * (wallWidth + 1) + wallWidth - 7, 0 + 1, y * (wallWidth + 1) + p + 19);
                    if(maze[y * row + x].DownC && checkPosEmpty(v3))
                    {
                        Instantiate(normalTile, v3, Quaternion.identity);
                    }
                    if(maze[y * row +x].rightC && checkPosEmpty(v3_2))
                    {
                        Instantiate(normalTile, v3_2, Quaternion.identity);
                    }
                }
            }
        }

        foreach(Transform t in prev)
        {
            if(t != null)
            {
                Instantiate(normalTile, t.position, Quaternion.identity);
            }
        }
        for(int y = 0; y < wallWidth; y++)
        {
            for(int x = 0; x < wallWidth; x++)
            {
                Vector3 v3 = new Vector3(store1[store1.Count - 1] * (wallWidth + 1) + x - 7, 0 + 1, store2[store2.Count - 1] * (wallWidth + 1) + y +19);
                if (checkPosEmpty(v3))
                {
                    prev.Add(Instantiate(currentTile, v3, Quaternion.identity));
                }
            }
        }
    }
    private bool checkPosEmpty(Vector3 targetPos)
    {
        GameObject[] allTile = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject t in allTile)
        {
            if(t.transform.position == targetPos)
            {
                Destroy(t);                
            }
        }
        return true;
    }


    /* Using dynamic programming and backtracking
     * The only path from start to end would be created
     */
    public List<Cell> path(Cell startPoint, Cell endPoint, Cell[] maze)
    {
        List<int> forReturn = new List<int>();
        List<Cell> visited = new List<Cell>();
        return path_dynamic(startPoint, endPoint, maze, visited);
    }
    public List<Cell> path_dynamic(Cell currentCell, Cell endCell, Cell[] maze, List<Cell> visited)
    {
        /*1) if node current = node end, then return visited list
         * 2) if all neighbors(cell.direction = true) of this node is already in visited (visited.contains(4 neighbours)), then return null list
         * 3) visit all 4 neighbours do dynamic programming, path_dynamic(node.right, endcell, maze, visited.add(node)); if this path returns doesnt return null, return the non-null list
         */
        if (currentCell.posIndex == endCell.posIndex)
        {
            visited.Add(currentCell);
            return visited;
        }
        List<int> neighbours = new List<int>();
        if (currentCell.UpC == true)
        {
            neighbours.Add(0);
        }
        if (currentCell.rightC == true)
        {
            neighbours.Add(1);
        }
        if (currentCell.DownC == true)
        {
            neighbours.Add(2);
        }
        if (currentCell.leftC == true)
        {
            neighbours.Add(3);
        }
        Cell nextNode = currentCell;
        if (neighbours.Count == 0)
        {
            return null;
        }
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i] == 0)
            {
                nextNode = maze[currentCell.posIndex - row];
                if (!visited.Contains(nextNode))
                {//new node
                    visited.Add(currentCell);  //if not the path, delete
                    List<Cell> possiblePath = path_dynamic(nextNode, endCell, maze, visited);
                    if (possiblePath == null)
                    {
                        return null;
                    }
                    else // path returns something thats not 
                    {
                        return possiblePath;
                    }
                }
            }

            else if (neighbours[i] == 1)
            {
                nextNode = maze[currentCell.posIndex + 1];
                if (!visited.Contains(nextNode))
                {//new node
                    visited.Add(currentCell);  //if not the path, delete
                    List<Cell> possiblePath = path_dynamic(nextNode, endCell, maze, visited);
                    if (possiblePath == null)
                    {
                        return null;
                    }
                    else // path returns something thats not 
                    {
                        return possiblePath;
                    }
                }
            }

            else if (neighbours[i] == 2)
            {
                nextNode = maze[currentCell.posIndex + row];
                if (!visited.Contains(nextNode))
                {//new node
                    visited.Add(currentCell);  //if not the path, delete
                    List<Cell> possiblePath = path_dynamic(nextNode, endCell, maze, visited);
                    if (possiblePath == null)
                    {
                        return null;
                    }
                    else // path returns something thats not 
                    {
                        return possiblePath;
                    }
                }
            }

            else
            {
                nextNode = maze[currentCell.posIndex - 1];
                //if (!visited.Contains(nextNode))
                //{//new node
                visited.Add(currentCell);  //if not the path, delete
                List<Cell> possiblePath = path_dynamic(nextNode, endCell, maze, visited);
                if (possiblePath == null)
                {
                    return null;
                }
                else // path returns something thats not 
                {
                    return possiblePath;
                }
                //}
            }
        }
        return null;
    }
}
