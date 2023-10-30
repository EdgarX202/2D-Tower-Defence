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

    private void Start()
    {
        GenerateMap();   
    }

    private void GenerateMap()
    {
        for(int y = 0; y < mapHeight; y++) 
        { 
            for(int x = 0; x < mapWidth; x++) 
            {
                GameObject newTile = Instantiate(mapTile);

                newTile.transform.position = new Vector2(x, y);
            }
        }
    }
}
