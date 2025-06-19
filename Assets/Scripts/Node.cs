using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int position;
    public bool isWalkable;
    public int gCost;
    public int hCost;
    public int FCost => gCost + hCost;
    public Node parent;

    public Node(Vector2Int pos, bool walkable)
    {
        position = pos;
        isWalkable = walkable;
    }
}
