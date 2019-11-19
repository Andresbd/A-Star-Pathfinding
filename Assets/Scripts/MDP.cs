using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MDP : MonoBehaviour
{
    public Node currentNode;
    private TrafficBoard t;
    private double gama = 0.8f;
    private double move = 0.9f;
    private double residium = 0.1f;
    public double value, sum, max, convSum;
    private bool stay, convergence;
    private int count, laps;
    private Node[] directions = new Node[9]; //0-LUp,1-L,2-LD,3-Up,4-C,5-D,6-RUp,7-R,8-RD
    private double[] values = new double[9];

    // Start is called before the first frame update
    void Start()
    {
        t = FindObjectOfType<TrafficBoard>();
        convergence = false;

        while (convergence == false) {
            convSum = 0;
            Node[,] newState = new Node[100, 100];
            for (int x = 0; x < 99; x++)
            {
                for (int y = 0; y < 99; y++)
                {
                    currentNode = t.grid[x, y];

                    //newState[x, y] = new Node(currentNode.isWall,currentNode.posX,currentNode.posY);

                    newState[x,y] = MDPSolver(x,y);
                }
            }

            Debug.Log("Andres");

            for (int x = 0; x < 99; x++)
            {
                for (int y = 0; y < 99; y++)
                {
                    convSum += t.grid[x, y].reward - newState[x, y].reward;
                }
            }

            if (convSum == 0f || laps == 1)
            {
                Debug.Log(laps);
                convergence = true;
            }
            else {
                laps++;
                for (int x = 0; x < 99; x++)
                {
                    for (int y = 0; y < 99; y++)
                    {
                        t.grid[x, y] = newState[x, y];
                    }
                }
            }
            
        }
    }

    public Node MDPSolver(int x, int y)
    {
        count = 0;
        value = 0;

        Node newStateNode = new Node(currentNode.isWall,x,y);

        FindNeighbours(currentNode);
        double div = residium / count;

        //Multiplicar cada linea por combinacion

        for (int i = 0; i < 9; i++) {
            for (int j = 0; j < 9; j++) {
                //LD,L,LU,D,U,RD,R,RU,S

                if (i == j) //En caso de estar en la casilla diagonal
                {
                    //No tener valor, stay
                    if (directions[j] == null)
                    {
                        stay = true;
                        continue;
                    }

                    //Prob * reward
                    value = move * directions[j].reward;
                }
                else {
                    //Si estamos en STAY y diagonal sin valor
                    if (j == 8 && stay == true)
                    {
                        stay = false;
                        //Prob * reward de stay
                        value = move * directions[j].reward;
                    }
                    else {
                        if (directions[j] == null) {
                            continue;
                        }
                        //Residuo * reward
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

        newStateNode.reward = max;
        Debug.Log(newStateNode.dir);

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
