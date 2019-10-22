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
    public List<Node> Path;
    public Node startNode;
    public Node current, actual, next;
    public bool nextMove, readyMove;
    
    public GameObject car, red;
    private int randomStart, randomGoal, cont, index, pos;
    Board b;


    void Start()
    {
        cont = 90;

        PrepareLists();

        spawnPlayer();

        SetGoal();

        pos = 1;

        transform.position = new Vector3(randomStart, 1, 0);

        Open.Add(startNode);

        startNode.gValue = 0;
        startNode.hValue = startNode.addHscore(b.goalNode);
        startNode.fValue = startNode.addFScore(startNode.gValue, startNode.hValue);

        PathFinding(b.goalNode);

    }

    void Update() {
        if(cont <= 0) {
            actual = b.grid[Path[pos].posX,Path[pos].posY];
            next = b.grid[Path[pos+1].posX,Path[pos+1].posY];
            readyMove = CheckNext();
            if(readyMove) {
                readyMove = false;
                actual.iscar = false;
                next.iscar = true;
                transform.position = new Vector3(Path[pos].posX,1,Path[pos].posY);
                pos++;
                cont = 90;
            }
        }else {
            cont--;
        }
    }

    bool CheckNext() {

        if(next.iscar == true || next.isWall == true) {
            return false;
        }else {
            return true;
        }
    }

    void PrepareLists() {

        Open = new List<Node>();
        Closed = new List<Node>();
        b = FindObjectOfType<Board>();
    }

    void spawnPlayer() {

        randomStart = Random.Range(0, 50);

        startNode = b.grid[randomStart, 0];

        if (startNode.isWall == true || startNode.iscar == true)
        {
            spawnPlayer();
        }

        startNode.iscar = true;

    }

    void SetGoal() {

    }

    void PathFinding(Node goal) {

        while (Open.Count != 0)
        {
            double tempMin = 10000;

            Open.ForEach((n) => { if (tempMin > n.fValue) { tempMin = n.fValue; current = n; } });

            if (current == b.goalNode)
            {
                construct_path(current);
            }

            Open.Remove(current);
            Closed.Add(current);

            foreach (Node neighbour in b.FindNeighbours(current)) {

                if (neighbour.isWall || Closed.Contains(neighbour)) {
                    
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

        Path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            Path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Path.Reverse();

        b.path = Path;
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
