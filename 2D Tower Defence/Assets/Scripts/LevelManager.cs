using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    TileScript tileScript;

    // Serialized fields
    [Header ("Tiles")]
    [SerializeField] private GameObject[] _tilePrefabs;
    [Header("Obstacles")]
    [SerializeField] private GameObject[] _obstacles;
    [Header ("Start & Exit points")]
    [SerializeField] private GameObject _doorInPrefab;
    [SerializeField] private GameObject _doorOutPrefab;
    [Header ("Map")]
    [SerializeField] private Transform _map;

    // Private
    private Point _doorIn;
    private Point _doorOut;
    private Point _obstacle;
    private Point _mapSize;
    private Stack<Node> _enemyPath;

    // Random
    System.Random random = new System.Random();

    // Properties
    public Stack<Node> EnemyPath
    {
        get
        {
            if (_enemyPath == null)
            {
                GeneratePath();
            }

            // Return new stack to every enemy spawned
            return new Stack<Node>(new Stack<Node>(_enemyPath));
        }
    }
    public float TileSize
    {
        // Get the tile width and return as a float
        get { return _tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    public Point DoorOut
    {
        get
        {
            return _doorOut;
        }
    }
    public Point DoorIn
    {
        get
        {
            return _doorIn;
        }
    }
    public Point Obstacle
    {
        get
        {
            return _obstacle;
        }
    }
    public Dictionary<Point, TileScript> Tiles { get; set; }
    public EnemyEntrance Entrance { get; set; }

    public bool isWalkable = true;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Create level, tiles, obstacles, entrance/exit
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        // Data from text file for the map
        string[] mapData = ReadLevelTxt();

        // Get map size
        _mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        // Calculate x and y map size
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        // Calculate world start point. Top Left - so that the camera aligns with the edge of the map
        Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // Create tiles on x and y positions
        for(int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for(int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStartPos);
            }
        }

        maxTile = Tiles[new Point(mapXSize-1, mapYSize-1)].transform.position;

        // Spawn entrance/exit and obstacles
        SpawnDoors();
        SpawnObstacles();
    }

    // Spawn random obstacles on the map
    public void SpawnObstacles()
    {
        

        // Random obstacle
        int randObs = random.Next(0, 2);
        // Random amount of obstacles
        int randAmount = random.Next(5, 21);

        for (int i = 0; i < randAmount; i++)
        {
            // Random positions
            int randX = random.Next(1, 15);
            int randY = random.Next(1, 7);
            Vector3 obstaclePos = new Vector3(randX, randY, 0f);

            // Instantiate obstacles
            Instantiate(_obstacles[randObs], obstaclePos, Quaternion.identity);
        }
    }

    // Placing tiles next to each other
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStartPos)
    {
        // Parse to int to use indexer when creating a new tile
        int tileIndex = int.Parse(tileType);
        // Create a new tile
        TileScript newTile = Instantiate(_tilePrefabs[tileIndex].GetComponent<TileScript>());
        // Place new tile next to previous tile
        newTile.Setup(new Point(x,y), new Vector3(worldStartPos.x + (TileSize * x), worldStartPos.y - (TileSize * y), 0), _map);
    }

    // Read level data from a text file
    private string[] ReadLevelTxt()
    {
        // Load text document where the level is stored
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        // Replace new line characters with empty string
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        // Split the data string into an array of strings using '-' as the delimiter
        return data.Split('-');
    }

    // Spawning Entrance/Exit
    private void SpawnDoors()
    {
        // Random Y position for entrance/exit
        int doorInRand = random.Next(1, 7);
        int doorOutRand = random.Next(1, 7);

        // Spawn at the start of the road
        _doorIn = new Point(0,doorInRand);
        // Spawn at the end of the round
        _doorOut = new Point(17, doorOutRand);

        // Instantiate entrance
        GameObject entrance = Instantiate(_doorInPrefab, Tiles[_doorIn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        Entrance = entrance.GetComponent<EnemyEntrance>();
        Entrance.name = "door_in";
        // Instantiate exit
        Instantiate(_doorOutPrefab, Tiles[_doorOut].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }

    // Get the bounds of the map
    public bool InsideBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < _mapSize.X && position.Y < _mapSize.Y;
    }

    // Generate enemy path
    public void GeneratePath()
    {
        _enemyPath = AStar.GetPath(_doorIn, _doorOut);
    }
}
