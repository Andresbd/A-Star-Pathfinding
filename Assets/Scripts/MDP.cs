using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MDP : MonoBehaviour
{
    public Node startNode, goalNode, currentNode;
    private TrafficBoard t;
    private double gama = 0.8f;
    private double move = 0.9f;
    private double residium = 0.1f;
    public double value, sum, max, convSum;
    private bool stay, convergence;
    private int count;
    private Node[] directions = new Node[9]; //0-LUp,1-L,2-LD,3-Up,4-C,5-D,6-RUp,7-R,8-RD
    private double[] values = new double[9];

    // Start is called before the first frame update
    void Start()
    {
        t = FindObjectOfType<TrafficBoard>();
        convergence = false;

        while (convergence == false) {
            Node[,] newState = new Node[100, 100];
            for (int x = 0; x <= t.grid.Length; x++)
            {
                for (int y = 0; y <= t.grid.Length; y++)
                {
                    currentNode = t.grid[x, y];

                    newState[x,y] = MDPSolver(x,y,newState[x,y]);
                }
            }

            for (int x = 0; x <= t.grid.Length; x++)
            {
                for (int y = 0; y <= t.grid.Length; y++)
                {
                    convSum += t.grid[x, y].reward - newState[x, y].reward;
                }
            }

            if (convSum == 0)
            {
                convergence = true;
            }
            else {

                for (int x = 0; x <= t.grid.Length; x++)
                {
                    for (int y = 0; y <= t.grid.Length; y++)
                    {
                        t.grid[x, y] = newState[x, y];
                    }
                }
            }
            
        }

        
    }

    public Node MDPSolver(int x, int y, Node newStateNode)
    {

        FindNeighbours(currentNode);
        double div = residium / count;

        //Multiplicar cada linea por combinacion

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                //LD,L,LU,D,U,RD,R,RU,S
                if (i == j)
                {
                    if (directions[j] == null)
                    {
                        stay = true;
                        continue;
                    }
                    value = move * directions[j].reward;
                }
                else {
                    if (j == 8 && stay == true)
                    {
                        stay = false;
                        value = move * directions[j].reward;
                    }
                    else {
                        if (directions[j] == null) {
                            continue;
                        }
                        value = div * directions[j].reward;
                    }
                }

                sum += value;
            }

            values[i] = sum;
            sum = 0;
        }
        //Finding max
        max = values.Max();

        int maxPos = Array.IndexOf(values,max);
        
        switch (maxPos) {
            case (0):
                newStateNode.dir = "LD";
                break;
            case (1):
                newStateNode.dir = "L";
                break;
            case (2):
                newStateNode.dir = "LU";
                break;
            case (3):
                newStateNode.dir = "D";
                break;
            case (4):
                newStateNode.dir = "U";
                break;
            case (5):
                newStateNode.dir = "RD";
                break;
            case (6):
                newStateNode.dir = "R";
                break;
            case (7):
                newStateNode.dir = "RU";
                break;
            case (8):
                newStateNode.dir = "S";
                break;

        }

        return newStateNode;
        
    }

    public List<Node> FindNeighbours(Node n)
    {

        List<Node> neighbours = new List<Node>();
        int dir = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = n.posX + x;
                int checkY = n.posY + y;

                if (checkX >= 0 && checkX < 100 && checkY >= 0 && checkY < 100)
                {
                    directions[dir] = t.grid[checkX, checkY];
                    neighbours.Add(t.grid[checkX, checkY]);
                    count++;
                }
                dir++;
            }
        }
        directions[dir] = n;
        return neighbours;
    }
}
