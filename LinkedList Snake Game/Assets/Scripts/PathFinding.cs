using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    private PathNode[,] grid;
    
    private List<PathNode> openList;
    private List<PathNode> closedList;

    private int gridWidth;
    private int gridHeight;

    public PathNode[,] GetGrid()
    {
        return grid;
    }
    
    public PathFinding(int width, int height)
    {
        gridWidth = width;
        gridHeight = height;

        grid = new PathNode[gridWidth, gridHeight];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new PathNode(grid, x, y);
            }
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid[startX, startY];
        PathNode endNode = grid[endX, endY];
        
        openList = new List<PathNode> {startNode};
        closedList = new List<PathNode>();
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].gCost = int.MaxValue;
                grid[x, y].CalculateFCost();
                grid[x, y].cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        
        return null; //if no path were found
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        //for this specific game I have commented out diagonal neighbours
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0)
        {
            //left
            neighbourList.Add(grid[currentNode.x - 1, currentNode.y]);
            
            //left down 
            /*if(currentNode.y - 1 >= 0)
                neighbourList.Add(grid[currentNode.x - 1, currentNode.y - 1]);
            //left up
            if(currentNode.y + 1 < gridHeight)
                neighbourList.Add(grid[currentNode.x - 1, currentNode.y + 1]);*/
        }

        if (currentNode.x + 1 < gridWidth)
        {
            //right
            neighbourList.Add(grid[currentNode.x + 1, currentNode.y]);
            
            //right down
            /*if(currentNode.y - 1 >= 0)
                neighbourList.Add(grid[currentNode.x + 1, currentNode.y - 1]);
            //right up
            if(currentNode.y + 1 < gridHeight)
                neighbourList.Add(grid[currentNode.x + 1, currentNode.y + 1]);*/
        }
        
        //down
        if(currentNode.y - 1 >= 0)
            neighbourList.Add(grid[currentNode.x, currentNode.y - 1]);
        //up
        if(currentNode.y + 1 < gridHeight)
            neighbourList.Add(grid[currentNode.x, currentNode.y + 1]);

        return neighbourList;
    }


    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse(); //reverse the path to go from start to end
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(b.x - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;
    }
}
