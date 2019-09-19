using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Armando Hernandez
//Eli Rodriguez
//Andres BD

public class AStarPathfinding : MonoBehaviour
{
    public List<Node> Open;
    public List<Node> Closed;
    public Node startNode;
    
    public Node current;
    public GameObject red;
    private int randomStart, randomGoal;
    Board b;


    void Start()
    {

        Open = new List<Node>();
        Closed = new List<Node>();
        b = FindObjectOfType<Board>();

        spawnPlayer();

        transform.position = new Vector3(randomStart, 1, 0);

        Open.Add(startNode);

        startNode.gValue = 0;
        startNode.hValue = startNode.addHscore(b.goalNode);
        startNode.fValue = startNode.addFScore(startNode.gValue, startNode.hValue);

        PathFinding(b.goalNode);

    }

    void spawnPlayer() {

        randomStart = Random.Range(0, 50);

        startNode = b.grid[randomStart, 0];

        if (startNode.isWall == true || startNode.playerHere == true)
        {
            spawnPlayer();
        }

        startNode.playerHere = true;
    }

    

    void PathFinding(Node goal) {

        while (Open.Count != 0)
        {
            double tempMin = 10000;

            Open.ForEach((n) => { if (tempMin > n.fValue) { tempMin = n.fValue; current = n; } });

            if (current == b.goalNode)
            {
                Debug.Log("Camino Encontrado");
                construct_path(current);
            }

            Open.Remove(current);
            Closed.Add(current);

            foreach (Node neighbour in b.FindNeighbours(current)) {

                if (neighbour.isWall || Closed.Contains(neighbour) || neighbour.playerHere) {
                    
                    continue;
                }

                double gCalculated = current.gValue + GetDistance(current, neighbour);
                if (gCalculated < neighbour.gValue || !Open.Contains(neighbour))
                {
                    neighbour.gValue = gCalculated;
                    neighbour.hValue = GetDistance(neighbour, b.goalNode);
                    neighbour.parent = current;

                    if (!Open.Contains(neighbour))
                        Open.Add(neighbour);
                }
            }
        }
    }

    void construct_path(Node endNode) {

        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            Instantiate(red, new Vector3(currentNode.posX, 1, currentNode.posY), Quaternion.identity);
            path.Add(currentNode);
            currentNode = currentNode.parent;
            currentNode.playerHere = true;
        }

        path.Reverse();

        b.path = path;
    }

    double GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.posX - nodeB.posX);
        int dstY = Mathf.Abs(nodeA.posY - nodeB.posY);

        if (dstX > dstY)
            return 1.414 * dstY + 1 * (dstX - dstY);
        return 1.414 * dstX + 1 * (dstY - dstX);
    }
}
