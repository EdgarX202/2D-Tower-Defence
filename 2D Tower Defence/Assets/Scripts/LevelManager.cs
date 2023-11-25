using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    TileScript tileScript;

    // Serialized fields
    [Header ("Tiles")]
    [SerializeField] private GameObject[] _tilePrefabs;
    [Header ("Obstacles")]
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

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelTxt();

        _mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

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
        SpawnObstacles(mapXSize, mapYSize);
    }

    // Spawn random obstacles on the map
    public void SpawnObstacles(int mapX, int mapY)
    {
        int rand = random.Next(0, 2);

        for (int i = 0; i < 15; i++)
        {
            int randX = random.Next(mapX);
            int randY = random.Next(mapY);

            Vector3 obstaclePos = new Vector3(randX , randY - 2, 0f);
            Instantiate(_obstacles[rand], obstaclePos, Quaternion.identity);
        }
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStartPos)
    {
        // Parse to int to use indexer when creating a enw tile
        int tileIndex = int.Parse(tileType);
        // Create a new tile
        TileScript newTile = Instantiate(_tilePrefabs[tileIndex].GetComponent<TileScript>());
        // Place new tile next to previous tile
        newTile.Setup(new Point(x,y), new Vector3(worldStartPos.x + (TileSize * x), worldStartPos.y - (TileSize * y), 0), _map);
    }

    private string[] ReadLevelTxt()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    private void SpawnDoors()
    {
        // Random Y position for entrance/exit
        int doorInRand = random.Next(0, 8);
        int doorOutRand = random.Next(0, 8);

        // Spawn at the start of the road
        _doorIn = new Point(0,doorInRand);
        // Spawn at the end of the round
        _doorOut = new Point(17, doorOutRand);

        GameObject entrance = Instantiate(_doorInPrefab, Tiles[_doorIn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        Entrance = entrance.GetComponent<EnemyEntrance>();
        Entrance.name = "door_in";

        Instantiate(_doorOutPrefab, Tiles[_doorOut].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }

    public bool InsideBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < _mapSize.X && position.Y < _mapSize.Y;
    }

    public void GeneratePath()
    {
        _enemyPath = AStar.GetPath(_doorIn, _doorOut);
    }
}
