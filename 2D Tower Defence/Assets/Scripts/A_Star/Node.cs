using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // Properties
    public Point GridPosition { get; private set; }
    public TileScript TileReference { get; private set; }

    // Constructor
    public Node(TileScript tileReference)
    {
        this.TileReference = tileReference;
        this.GridPosition = tileReference.GridPosition;
    }
}
