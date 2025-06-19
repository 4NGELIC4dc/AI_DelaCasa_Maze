using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public Vector2Int gridSize;
    public float cellSize = 1f;

    private Dictionary<Vector2Int, Node> grid;

    public void InitializeGrid()
    {
        grid = new Dictionary<Vector2Int, Node>();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2 worldPos = GridToWorld(new Vector2Int(x, y));
                bool walkable = !Physics2D.OverlapBox(worldPos, Vector2.one * 0.8f, 0f, obstacleLayer);
                grid[new Vector2Int(x, y)] = new Node(new Vector2Int(x, y), walkable);
            }
        }
    }

    public List<Vector2> FindPath(Vector2 startWorld, Vector2 targetWorld)
    {
        Vector2Int start = WorldToGrid(startWorld);
        Vector2Int end = WorldToGrid(targetWorld);

        if (!grid.ContainsKey(start) || !grid.ContainsKey(end)) return null;

        Node startNode = grid[start];
        Node endNode = grid[end];

        List<Node> openList = new List<Node> { startNode };
        HashSet<Node> closedList = new HashSet<Node>();

        foreach (var node in grid.Values)
        {
            node.gCost = int.MaxValue;
            node.hCost = GetDistance(node.position, end);
            node.parent = null;
        }

        startNode.gCost = 0;

        while (openList.Count > 0)
        {
            Node current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < current.FCost || (openList[i].FCost == current.FCost && openList[i].hCost < current.hCost))
                    current = openList[i];
            }

            openList.Remove(current);
            closedList.Add(current);

            if (current == endNode)
                return RetracePath(startNode, endNode);

            foreach (Vector2Int neighborOffset in new Vector2Int[] {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
            })
            {
                Vector2Int neighborPos = current.position + neighborOffset;
                if (!grid.ContainsKey(neighborPos)) continue;

                Node neighbor = grid[neighborPos];
                if (!neighbor.isWalkable || closedList.Contains(neighbor)) continue;

                int tentativeG = current.gCost + 1;
                if (tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.parent = current;
                    if (!openList.Contains(neighbor)) openList.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    List<Vector2> RetracePath(Node start, Node end)
    {
        List<Vector2> path = new List<Vector2>();
        Node current = end;

        while (current != start)
        {
            path.Add(GridToWorld(current.position));
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x / cellSize), Mathf.RoundToInt(worldPos.y / cellSize));
    }

    Vector2 GridToWorld(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * cellSize, gridPos.y * cellSize);
    }

    int GetDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan
    }
}
