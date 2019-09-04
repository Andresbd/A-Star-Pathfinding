using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private bool isWall;

    //H stands for ammount of nodes to move
    //G stands for the acumulated move cost
    public int posX, posY;
    public double gValue, hValue, fValue;

    public Node parent;

    public Node(bool wall, int px, int py)
    {
        posX = px;
        posY = py;
        isWall = wall;

        double addGScore(Node parent, float moveCost) {

            return parent.gValue + moveCost;
        }

        double addHscore(Node goal) {

            double hAdded = 1;

            double distanceX = Mathf.Abs(posX - goal.posX);
            double distanceY = Mathf.Abs(posY - goal.posY);
            double sumDis = distanceX + distanceY;
            double resDis = distanceX - distanceY;
            double diagonal = (1.414 - 2) * 1;

            return 1 * sumDis + diagonal * resDis;
        }

        double addFScore(double g, double h) {

            return g + h;
        }
    }
}
