using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    // Serialized fields
    [SerializeField] private GameObject[] tilePrefabs;
    [SerializeField] private GameObject doorInPrefab;
    [SerializeField] private GameObject doorOutPrefab;
    [SerializeField] private Transform map;

    private Point doorIn;
    private Point doorOut;

    // Properties
    // So that no other script could change tile size
    public float TileSize
    {
        // Get the tile width and return as a float
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    public Dictionary<Point, TileScript> Tiles { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();


        string[] mapData = ReadLevelTxt();

        // Calculate x and y map size
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        // Calculate world start point. Top Left
        Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // Create tile on x and y positions
        for(int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for(int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStartPos);
            }
        }

        maxTile = Tiles[new Point(mapXSize-1, mapYSize-1)].transform.position;

        SpawnDoors();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStartPos)
    {
        // Parse to int to use indexer when creating a enw tile
        int tileIndex = int.Parse(tileType);
        // Create a new tile
        TileScript newTile = Instantiate(tilePrefabs[tileIndex].GetComponent<TileScript>());
        // Place new tile next to previous tile
        newTile.Setup(new Point(x,y), new Vector3(worldStartPos.x + (TileSize * x), worldStartPos.y - (TileSize * y), 0), map);
    }

    private string[] ReadLevelTxt()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnDoors()
    {
        // Spawn at the start of the road
        doorIn = new Point(0,0);
        // Spawn at the end of the round
        doorOut = new Point(17, 8);

        Instantiate(doorInPrefab, Tiles[doorIn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        Instantiate(doorOutPrefab, Tiles[doorOut].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
}
