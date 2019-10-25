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
    public Node startNode, gNode;
    public Node current, actual, next;
    public bool readyMove, nSpawn, go;
    
    public GameObject car, red;
    private int randomStart, randomGoal, cont, index, pos;
    TrafficBoard b;

    public void GenerateCar() {
        cont = 60;

        pos = 1;

        PrepareLists();
        spawnPlayer();
        SetGoal();

        Open.Add(startNode);

        startNode.gValue = 0;
        startNode.hValue = 0;
        startNode.fValue = 0;
        startNode.hValue = startNode.addHscore(gNode);
        startNode.fValue = startNode.addFScore(startNode.gValue, startNode.hValue);

        PathFinding(gNode);

        go = true;
    }

    void FixedUpdate() {
        if(go) {
            if(cont <= 0) {
                actual = b.grid[Path[pos].posX,Path[pos].posY];
                next = b.grid[Path[pos+1].posX,Path[pos+1].posY];
                if(next == gNode) {
                    if(nSpawn) {
                        b.nCars--;
                        actual.iscar = false;
                        next.iscar = false;
                        Destroy(gameObject);
                    }else {
                        b.wCars--;
                        actual.iscar = false;
                        next.iscar = false;
                        Destroy(gameObject);
                    }
                }
                readyMove = CheckNext();
                if(readyMove) {
                    readyMove = false;
                    actual.iscar = false;
                    next.iscar = true;
                    transform.position = new Vector3(Path[pos].posX,1,Path[pos].posY);
                    pos++;
                    cont = 60;
                }
            }else {
                cont--;
            }
        }
    }

    bool CheckNext() {

        if(next.iscar == true || next.isTrafficLight == true || actual.isWall == true) {
            return false;
        }else {
            return true;
        }
    }

    void PrepareLists() {
        b = FindObjectOfType<TrafficBoard>();
        Open = new List<Node>();
        Closed = new List<Node>();
    }

    void spawnPlayer() {

        startNode = null;

        if(nSpawn) {

            randomStart = Random.Range(40, 59);

            startNode = b.grid[randomStart,99];

            transform.position = new Vector3(randomStart,1,99);

            if (startNode.isWall == true || startNode.iscar == true)
            {
                spawnPlayer();
            }

            b.nCars++;

        }else {
            
            randomStart = Random.Range(40, 59);

            startNode = b.grid[0,randomStart];

            transform.position = new Vector3(0,1,randomStart);

            if (startNode.isWall == true || startNode.iscar == true)
            {
                spawnPlayer();
            }

            b.wCars++;
        }
    }

    void SetGoal() {
        gNode = null;

        if(nSpawn) {
            int dirRan = Random.Range(0,2);
            if(dirRan == 1) {
                //40,0 - 59,0
                int gPos = Random.Range(40,59);
                gNode = b.grid[gPos,0];
            }else {
                int gPos = Random.Range(40,59);
                gNode = b.grid[98,gPos];
            }
        }else {
            int dirRan = Random.Range(0,2);
            if(dirRan == 1) {
                //40,0 - 59,0
                int gPos = Random.Range(40,59);
                gNode = b.grid[98,gPos];
            }else {
                int gPos = Random.Range(40,59);
                gNode = b.grid[gPos,0];
            }
        }
    }

    void PathFinding(Node goal) {

        while (Open.Count != 0)
        {
            double tempMin = 1000;

            Open.ForEach((n) => { if (tempMin > n.fValue) { tempMin = n.fValue; current = n; } });

            if (current == gNode)
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
                    neighbour.hValue = GetDistance(neighbour, gNode);
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
