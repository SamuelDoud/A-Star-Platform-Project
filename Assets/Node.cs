using UnityEngine;
using System.Collections;

public class Node{

    public bool walkable;
    public Vector3 worldposition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool _walkable, Vector3 _WorldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldposition = _WorldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}