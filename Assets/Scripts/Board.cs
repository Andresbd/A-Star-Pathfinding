using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject white, black;
    public Node[,] grid =  new Node[50,50];
    public int cont;
    // Start is called before the first frame update
    void Start()
    {
        int walls = Random.Range(500, 800);
        cont = 0;

        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {

                if (cont < walls)
                {
                    int createWall = Random.Range(0, 4);

                    if (createWall == 1)
                    {
                        black = Instantiate(black, new Vector3(x, 0, y), Quaternion.identity);
                        black.transform.parent = transform;
                        grid[x, y] = new Node(true, x, y);
                        cont++;
                    }
                    else {
                        white = Instantiate(white, new Vector3(x, 0, y), Quaternion.identity);
                        white.transform.parent = transform;

                        grid[x, y] = new Node(false, x, y);
                    }
                }
                else {
                    white = Instantiate(white, new Vector3(x, 0, y), Quaternion.identity);
                    white.transform.parent = transform;
                    grid[x, y] = new Node(false, x, y);
                }
            }
        }
    }

    public List<Node> FindNeighbours(Node n) {

        List<Node> neighbours = new List<Node>();

        return neighbours;
    }
}
