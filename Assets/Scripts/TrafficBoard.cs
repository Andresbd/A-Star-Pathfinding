using System.Collections.Generic;
using UnityEngine;

public class TrafficBoard : MonoBehaviour
{
    public GameObject black, white;
    public Node[,] grid = new Node[100,100];
    public Node[] consult = new Node[40];
    public GameObject[] NS;
    public GameObject[] WE;
    public Node goalNode;
    public List<Node> path;
    public int nCount, wCount, cCount,  dCount, nCars, wCars;
    // Start is called before the first frame update
    void Awake()
    {
        dCount = 20;
        WE = new GameObject[20];
        NS = new GameObject[20];
        GenerateBoard();
    }
//40,60 > 59,60 NS
//39,59 > 39,40 WE
    void GenerateBoard()  {
        for(int i = 0; i < 99;  i++){
            for(int j = 0; j < 100; j++) {
                if(i <= 40 && j > 39 && j < 60) {
                    white = Instantiate(white, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(false,i,j);
                    grid[i, j].reward = -0.4f;
                    if (i == 39 && j >= 40 && j <= 59) {
                        WE[wCount] = white;
                        consult[cCount] = grid[i,j];
                        wCount++;
                        cCount++;
                    }
                }else if(i > 39 && i < 60){
                    white = Instantiate(white, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(false,i,j);
                    grid[i, j].reward = -0.4f;
                    if (j == 60) {
                        NS[nCount] = white;
                        consult[dCount] = grid[i,j];
                        nCount++;
                        dCount++;
                    }
                }else if(i >= 60 && j > 39 && j  < 60) {
                    white = Instantiate(white, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(false,i,j);
                    grid[i, j].reward = -0.4f;
                }
                else {
                    black = Instantiate(black, new Vector3(i,0,j),Quaternion.identity);
                    grid[i,j] = new Node(true,i,j);
                    grid[i, j].reward = -10f;
                }
            }
        }
    }

    public List<Node> FindNeighbours(Node n) {

        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) {
                    continue;
                }

                int checkX = n.posX + x;
                int checkY = n.posY + y;

                if(checkX >= 0 && checkX < 99 && checkY >= 0 && checkY < 100)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}
