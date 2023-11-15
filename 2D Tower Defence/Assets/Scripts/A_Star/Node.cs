using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Properties
    public Point GridPosition { get; private set; }
    public TileScript TileReference { get; private set; }
    public Node Parent { get; private set; }
    public Vector2 WorldPos { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    // Constructor
    public Node(TileScript tileReference)
    {
        this.TileReference = tileReference;
        this.GridPosition = tileReference.GridPosition;
        this.WorldPos = tileReference.WorldPosition; 
    }

    // Calculate all node values
    public void CalculateValues(Node parent, int gCost, Node goal)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = ((Mathf.Abs(GridPosition.X - goal.GridPosition.X)) + Mathf.Abs((goal.GridPosition.Y - GridPosition.Y))) * 10;
        this.F = G + H;
    }
}
