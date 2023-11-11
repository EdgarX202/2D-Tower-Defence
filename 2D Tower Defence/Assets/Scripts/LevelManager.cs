using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Serialised fields
    [SerializeField] private GameObject tile;

    // Property
    // So that no other script could change tile size
    public float TileSize
    {
        // Get the tile width and return as a float
        get { return tile.GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
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
        // Make the start position to be top left corner
        Vector3 worldStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        // Create tile on x and y positions
        for(int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 10; x++)
            {
                PlaceTile(x,y, worldStartPos);
            }
        }
    }

    private void PlaceTile(int x, int y, Vector3 worldStartPos)
    {
        // Create a new tile
        GameObject newTile = Instantiate(tile);
        // Place new tile next to previous tile
        newTile.transform.position = new Vector3(worldStartPos.x + (TileSize * x), worldStartPos.y - (TileSize * y), 0);
    }
}
