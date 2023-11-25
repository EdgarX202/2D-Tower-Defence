using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebug : MonoBehaviour
{
    // Serialized fields
    [Header("Debugging Prefabs")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject debugTilePrefab;

    // Private
    private TileScript _start;
    private TileScript _goal;


    // Debug Update()
    /*
    void Update()
    {
        //ClickTile();

        // Generate path between 2 points
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AStar.GetPath(_start.GridPosition, _goal.GridPosition);
        }
    }
    */

    // Selecting start and exit by using mouse click
    private void ClickTile()
    {
        if(Input.GetMouseButtonDown(1))
        {
            // Using raycast to get mouse click position
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // Create debug tiles for start and exit
            if(hit.collider != null )
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if(tmp != null )
                {
                    if(_start == null)
                    {
                        _start = tmp;
                        CreateDebugTile(_start.WorldPosition, new Color32(255, 135, 0, 255));
                    }
                    else if(_goal == null)
                    {
                        _goal = tmp;
                        CreateDebugTile(_goal.WorldPosition, new Color32(255, 0, 0, 255));
                    }
                }
            }
        }
    }

    // Create open, close, final paths with neighbouring tiles pointing towards parent
    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> pathFinal)
    {
        // Open list tiles
        foreach (Node node in openList)
        {
            if (node.TileReference != _start && node.TileReference != _goal)
            {
                CreateDebugTile(node.TileReference.WorldPosition, Color.cyan, node);
            }

            PointToParent(node, node.TileReference.WorldPosition);
        }

        // Closed list tiles
        foreach (Node node in closedList)
        {
            if (node.TileReference != _start && node.TileReference != _goal && !pathFinal.Contains(node))
            {
                CreateDebugTile(node.TileReference.WorldPosition, Color.blue, node);
            }

            PointToParent(node, node.TileReference.WorldPosition);
        }

        // Final path tiles
        foreach (Node node in pathFinal)
        {
            if (node.TileReference != _start && node.TileReference != _goal)
            {
                CreateDebugTile(node.TileReference.WorldPosition, Color.green, node);
            }
        }
    }

    // Point neighbouring tiles to parent
    private void PointToParent(Node node, Vector2 pos)
    {
        if(node.Parent != null)
        {
            // Instantiate an arrow prefab and set its sorting layer
            GameObject arrow = (GameObject)Instantiate(arrowPrefab, pos, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;

            // Point to the right
            if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            // Top right angle
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            // Point up
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            // Top left angle
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            // Point to the left
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            // Bottom left angle
            else if ((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            // Point down
            else if ((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            // Down right angle
            else if ((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y))
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }
        }
    }

    // Create debug tile prefab showing G,H,F numbers
    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null)
    {
        // Instantiate debug tile prefab
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos, Quaternion.identity);

        if(node != null)
        {
            // Create text with correct G,H,F numbers
            DebugTile tmp = debugTile.GetComponent<DebugTile>();
            tmp.G.text += node.G;
            tmp.H.text += node.H;
            tmp.F.text += node.F;
        }

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
