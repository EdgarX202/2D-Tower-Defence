using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{

    // Private
    private SpriteRenderer spriteRenderer;
    private Tower myTower;
    private int health = 15;

    private Color32 fullColour = new Color32(255, 118, 118, 255); // RED
    private Color32 emptyColour = new Color32(96, 255, 90, 255); // GREEN
    
    // Properties
    public Point GridPosition { get; private set; }
    public bool IsEmpty { get; set; }
    public bool Debugging { get; set; }
    public bool Walkable { get; set; }
    public Vector2 WorldPosition
    {
        get {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2),
                               transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
            }
    }

    // Random
    System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Sets up the tile
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        Walkable = true;
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    // Select/Deselect or place the tower on a tile
    private void OnMouseOver()
    {
        // Check grip position index
        //Debug.Log(GridPosition.X + ", " + GridPosition.Y);

        // Check if the mouse is over a UI element
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            // If the GameManager has a selected tower
            if (IsEmpty && !Debugging)
            {
                // Set the tile colour
                TileColour(emptyColour);
            }
            if(!IsEmpty && !Debugging)
            {
                TileColour(fullColour);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                // If mouse button is clicked, place the tower in that spot
                PlaceTower();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButton(0))
        {
            // If the GameManager does not have a selected tower
            if (myTower != null)
            {
                // Select the tower on that tile if it has a tower
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
                // Deselect any selected towers if there's no tower on that tile
                GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        if (!Debugging)
        {
            TileColour(Color.white);
        }
    }

    // Check if path doesnt exist and place tower
    private void PlaceTower()
    {
        Walkable = false;

        if(AStar.GetPath(LevelManager.Instance.DoorIn, LevelManager.Instance.DoorOut) == null)
        {
            // There is no path then it is walkable
            Walkable = true;
            return;
        }

        // Create a tower object
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        // Increase sorting layer number for the tower above not to overlap current tower
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        // Set the tile as transform parent to the tower (the tower will become a child)
        tower.transform.SetParent(transform);

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();

        IsEmpty = false;
        // Set the colour back to white
        TileColour(Color.white);

        // Get the price of the clicked tower
        myTower.Price = GameManager.Instance.ClickedBtn.Price;

        // Buy the tower
        GameManager.Instance.BuyTower();

        Walkable = false;
    }

    private void TileColour(Color newColor)
    {
       // Set the colour of the tile
       spriteRenderer.color = newColor;
    }
}
