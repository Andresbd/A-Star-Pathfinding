using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public List<Node> Open;
    public List<Node> Closed;
    public Node startNode;
    public Node goalNode;
    Board b;

    private void Awake()
    {
        b = GetComponent<Board>();
    }

    private void Start()
    {
        int randomStart = Random.Range(0, 51);
        startNode = b.grid[randomStart, 0];

        goalNode = b.grid[30, 30];
    }
}
