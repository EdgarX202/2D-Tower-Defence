using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AStar
{   
    // Dictionary
    private static Dictionary<Point, Node> nodes; 

    // Creating nodes and adding to dictionary
    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        // Run through all tiles in the game
        foreach(TileScript tile in LevelManager.Instance.Tiles.Values )
        {
            // Add node to dictionary
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    // Getting the final path
    public static Stack<Node> GetPath(Point start, Point goal)
    {
        // If no nodes exist, create nodes
        if(nodes == null )
        {
            CreateNodes();
        }
        
        // Creates an open list for A* algorithm
        HashSet<Node> openList = new HashSet<Node>();
        // Creates a closed list for A* algorithm
        HashSet<Node> closeList = new HashSet<Node>();
        // Pushing final path to stack
        Stack<Node> finalPath = new Stack<Node>();
        // Finds the start node and creates a reference to it
        Node currentNode = nodes[start];
        // Add the start node to open list
        openList.Add(currentNode);

        // Calculating g,h,f and neighbours
        while(openList.Count > 0)
        {
            // Get the neighbours around the current node
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Get the position of the neighbour
                    Point neighbourPosition = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);

                    if (LevelManager.Instance.InsideBounds(neighbourPosition) && LevelManager.Instance.Tiles[neighbourPosition].Walkable && neighbourPosition != currentNode.GridPosition)
                    {
                        int gCost = 0;

                        /* EXAMPLE HOW x-y WORKS
                         * [14][10][14]
                         * [10][S] [10]
                         * [14][10][14]
                         * 
                         * x and y axis.
                         * Left middle: x is -1 and y is 0 = 1 = gCost is 10 (using absolute function, no negatives)
                         * Bottom middle: x is 0 and y is -1 = 1 gCost is 10
                         * Top right: x is 1 and y is 1 = 2 (not == to 1) gCost is 14
                         */

                        if (Mathf.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        {
                            //if(!ConnectedDiagonally(currentNode, nodes[neighbourPosition]))
                            //{
                            //    continue;
                            //}

                            /*
                             * Code needs to be changed, but basically making the enemy to never take diagonal route.
                             * Maybe keep the code as it is and later, when the game has more levels, adjust gCost through code.
                             * In some levels enemy could take diagonal path?
                             */
                            gCost = 104;
                            
                        }

                        // Add neighbours to open list
                        Node neighbour = nodes[neighbourPosition];

                        if (openList.Contains(neighbour))
                        {
                            // Finding a better G score
                            if (currentNode.G + gCost < neighbour.G)
                            {
                                neighbour.CalculateValues(currentNode, gCost, nodes[goal]);
                            }
                        }

                        else if (!closeList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                            neighbour.CalculateValues(currentNode, gCost, nodes[goal]);
                        }
                    }
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (openList.Count > 0)
            {
                // Order open list by the f value and take the first value (which should have lowest f value)
                currentNode = openList.OrderBy(n => n.F).First();
            }

            // Search finished (goal found), return path
            if(currentNode == nodes[goal])
            {
                while(currentNode.GridPosition != start)
                {
                    // Adding found path to stack
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                return finalPath;
            }
        }
        GameObject.Find("AStarDebug").GetComponent<AStarDebug>().DebugPath(openList, closeList, finalPath);
        return null;
    }

    // Finding diagonal paths
    private static bool ConnectedDiagonally(Node currentNode, Node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        // If the node is inside the grid and tile is not walkable
        if (LevelManager.Instance.InsideBounds(first) && !LevelManager.Instance.Tiles[first].Walkable)
        {
            return false;
        }
        if (LevelManager.Instance.InsideBounds(second) && !LevelManager.Instance.Tiles[second].Walkable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
