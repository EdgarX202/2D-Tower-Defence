using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AStar
{
    // Node dictionary
    private static Dictionary<Point, Node> nodes; 

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        // Run through tile dictionary values
        foreach(TileScript tile in LevelManager.Instance.Tiles.Values )
        {
            // Create node for each tilescript
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }
}
