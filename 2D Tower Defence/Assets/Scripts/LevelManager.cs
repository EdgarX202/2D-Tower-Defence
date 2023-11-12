using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private GameObject[] tilePrefabs;

    // Property
    // So that no other script could change tile size
    public float TileSize
    {
        // Get the tile width and return as a float
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

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
        string[] mapData = ReadLevelTxt();

        // Calculate x and y map size
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

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
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStartPos)
    {
        int tileIndex = int.Parse(tileType);

        // Create a new tile
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        // Place new tile next to previous tile
        newTile.transform.position = new Vector3(worldStartPos.x + (TileSize * x), worldStartPos.y - (TileSize * y), 0);
    }

    private string[] ReadLevelTxt()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;

        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }
}
