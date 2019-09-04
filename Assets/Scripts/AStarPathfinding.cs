using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public List<Node> Open;
    public List<Node> Closed;
    public Node startNode;
    public Node goalNode;
    public Node current;
    Board b;

    private void Awake()
    {
        b = FindObjectOfType<Board>();
    }

    private void Start()
    {
        Open = new List<Node>();
        Closed = new List<Node>();
        int randomStart = Random.Range(0, 50);

        startNode = b.grid[randomStart, 0];
        goalNode = b.grid[30, 30];

        Open.Add(startNode);

        startNode.gValue = 0;
        startNode.hValue = startNode.addHscore(goalNode);
        startNode.fValue = startNode.addFScore(startNode.gValue, startNode.hValue);

        do {
            double tempMin = 10000;
            Open.ForEach((n) => { if (tempMin > n.fValue) { tempMin = n.fValue; current = n; }}); 
            PathFinding(current);

        } while (Open.Count != 0);
    }

    void PathFinding(Node gN) {

    }
}
