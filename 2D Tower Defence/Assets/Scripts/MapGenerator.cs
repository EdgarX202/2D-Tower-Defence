using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mapTile;
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;

    private List<GameObject> mapTiles = new();
    private List<GameObject> pathTiles = new();

    private bool reachedX = false;
    private bool reachedY = false;
    private GameObject currentTile;
    private int currentTileIndex;
    private int nextTileIndex;

    public Color pathColour;

    private void Start()
    {
        GenerateMap();   
    }

    private List<GameObject> getTopEdgeTiles()
    {
        List<GameObject> edgeTiles = new();

        for (int i = mapWidth * (mapHeight - 1); i < mapWidth * mapHeight; i++) 
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    private List<GameObject> getBottomEdgeTiles()
    {
        List<GameObject> edgeTiles = new();

        for (int i = 0; i < mapWidth; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    private void MoveDown()
    {
        pathTiles.Add(currentTile);
        currentTileIndex = mapTiles.IndexOf(currentTile);
        nextTileIndex = currentTileIndex - mapWidth;
        currentTile = mapTiles[nextTileIndex];
    }

    private void MoveLeft()
    {
        pathTiles.Add(currentTile);
        currentTileIndex = mapTiles.IndexOf(currentTile);
        nextTileIndex = currentTileIndex-1;
        currentTile = mapTiles[nextTileIndex];
    }

    private void MoveRight()
    {
        pathTiles.Add(currentTile);
        currentTileIndex = mapTiles.IndexOf(currentTile);
        nextTileIndex = currentTileIndex+1;
        currentTile = mapTiles[nextTileIndex];
    }

    private void GenerateMap()
    {
        for(int y = 0; y < mapHeight; y++) 
        { 
            for(int x = 0; x < mapWidth; x++) 
            {
                GameObject newTile = Instantiate(mapTile);

                mapTiles.Add(newTile);

                newTile.transform.position = new Vector2(x, y);
            }
        }

        List<GameObject> topEdgeTiles = getTopEdgeTiles();
        List<GameObject> bottomEdgeTiles = getBottomEdgeTiles();

        GameObject startTile;
        GameObject endTile;

        int rand1 = Random.Range(0, mapWidth);
        int rand2 = Random.Range(0, mapWidth);

        startTile = topEdgeTiles[rand1];
        endTile = bottomEdgeTiles[rand2];

        currentTile = startTile;

        MoveDown();

        int loopCount = 0;

        while (!reachedX)
        {
            loopCount++;
            if(loopCount > 100)
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
            if(currentTile.transform.position.y > endTile.transform.position.y)
            {
                MoveDown();
            }
            else
            {
                reachedY = true;
            }
        }

        pathTiles.Add(endTile);

        foreach (GameObject obj in pathTiles)
        {
            obj.GetComponent<SpriteRenderer>().color = pathColour;
        }    
    }
}
