using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    // Properties
    public Point GridPosition { get; private set; }
    public bool IsEmpty { get; set; }
    public bool Debugging { get; set; }
    public bool Walkable { get; set; }

    private SpriteRenderer spriteRenderer;

    private Tower myTower;

    private Color32 fullColour = new Color32(255, 118, 118, 255); // RED
    private Color32 emptyColour = new Color32(96, 255, 90, 255); // GREEN


    public Vector2 WorldPosition
    {
        get {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2),
                               transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    private void OnMouseOver()
    {
        // Check grip position index
        //Debug.Log(GridPosition.X + ", " + GridPosition.Y);

        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {
            if (IsEmpty && !Debugging)
            {
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
            if(myTower != null)
            {
                GameManager.Instance.SelectTower(myTower);
            }
            else
            {
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

    private void PlaceTower()
    {
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

        // Buy the towert
        GameManager.Instance.BuyTower();

        Walkable = false;
    }

    private void TileColour(Color newColor)
    {
       // Set the colour of the tile
       spriteRenderer.color = newColor;
    }
}
