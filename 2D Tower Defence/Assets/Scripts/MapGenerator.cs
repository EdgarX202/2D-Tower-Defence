using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Tile reference
    [SerializeField] private GameObject mapTile;
    [SerializeField] private Color pathColour;
    [SerializeField] private Color startColour;
    [SerializeField] private Color endColour;
    // Map dimensions
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    // Tile index
    private GameObject currentTile;
    private int currentTileIndex;
    private int nextTileIndex;
    // Bool
    private bool reachedX = false;
    private bool reachedY = false;
    // Lists
    public static List<GameObject> mapTiles = new();
    public static List<GameObject> pathTiles = new();
    
    public static GameObject startTile;
    public static GameObject endTile;

    private void Start()
    {
        GenerateMap();   
    }

    // Create a list for getting top tiles
    private List<GameObject> GetLeftEdgeTiles()
    {
        List<GameObject> edgeTiles = new();

        // i = top left tile, i < top right tile
        for (int i = mapWidth * (mapHeight - 1); i < mapWidth * mapHeight; i++) 
        {
            edgeTiles.Add(mapTiles[i]);
        }
        return edgeTiles;
    }
    // Create a list for getting bottom tiles
    private List<GameObject> GetRightEdgeTiles()
    {
        List<GameObject> edgeTiles = new();

        // i = bottom left tile < bottom right
        for (int i = 0; i < mapWidth; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }
        return edgeTiles;
    }

    // Move path tile Down,Up,Left,Right
    // REFACTORING NEEDED ----------------------------<<
    private void MoveLeft()
    {
        pathTiles.Add(currentTile);
        currentTileIndex = mapTiles.IndexOf(currentTile);
        nextTileIndex = currentTileIndex - mapHeight;
        currentTile = mapTiles[nextTileIndex];
    }
    private void MoveRight()
    {
        // Add current tile to list of path tiles
        pathTiles.Add(currentTile);
        // Get the index of the current tile
        currentTileIndex = mapTiles.IndexOf(currentTile);
        // Calculate the index of the next tile
        nextTileIndex = currentTileIndex + mapHeight;
        // Set the next tile to be the current tile
        currentTile = mapTiles[nextTileIndex];
    }
    private void MoveDown()
    {
        pathTiles.Add(currentTile);
        currentTileIndex = mapTiles.IndexOf(currentTile);
        nextTileIndex = currentTileIndex-1;
        currentTile = mapTiles[nextTileIndex];
    }
    private void MoveUp()
    {
        pathTiles.Add(currentTile);
        currentTileIndex = mapTiles.IndexOf(currentTile);
        nextTileIndex = currentTileIndex+1;
        currentTile = mapTiles[nextTileIndex];
    }

    private void GenerateMap()
    {
        int loopCount = 0;

        // Generate the map tiles
        for (int x = 0; x < mapWidth; x++) 
        { 
            for(int y = 0; y < mapHeight; y++) 
            {
                GameObject newTile = Instantiate(mapTile);
                mapTiles.Add(newTile);
                newTile.transform.position = new Vector2(x, y);
            }
        }

        // Lists for getting side edge tiles
        List<GameObject> leftEdgeTiles = GetLeftEdgeTiles();
        List<GameObject> rightEdgeTiles = GetRightEdgeTiles();

        // Get a random tile from each side
        int leftRand = Random.Range(0, mapHeight+1);
        int rightRand = Random.Range(0, mapHeight);

        // Make random tiles start/end markers
        startTile = leftEdgeTiles[leftRand];
        endTile = rightEdgeTiles[rightRand];

        currentTile = startTile;

        // First move when the game starts
       // MoveLeft();

        while (!reachedX)
        {
            loopCount++;
            if (loopCount > 100)
            {
                Debug.Log("Loop ran too long! Broke out of it!");
                break;
            }
            if (currentTile.transform.position.x > endTile.transform.position.x)
            {
                MoveLeft();
            }
            else if (currentTile.transform.position.x < endTile.transform.position.x)
            {
                MoveRight();
            }
            else
            {
                reachedX = true;
            }
        }

        while (!reachedY)
        {
            if (currentTile.transform.position.y > endTile.transform.position.y)
            {
                MoveDown();
            }
            else if (currentTile.transform.position.y < endTile.transform.position.y)
            {
                MoveUp();
            }
            else
            {
                reachedY = true;
            }
        }

        pathTiles.Add(endTile);

        // Change colours for path/end/start tiles
        foreach (GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = pathColour;
        }    

        startTile.GetComponent<SpriteRenderer>().color = startColour;
        endTile.GetComponent<SpriteRenderer>().color = endColour;
    }
}
