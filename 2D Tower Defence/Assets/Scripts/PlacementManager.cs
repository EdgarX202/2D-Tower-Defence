using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    // Reference to ShopManager
    public ShopManager shopManager;

    // Layer masks
    public LayerMask mask;
    public LayerMask towerMask;

    // Bools
    public bool isBuilding;
    public bool towerOnSlot = false;

    // Camera
    public Camera cam;

    // GameObjects
    public GameObject basicTowerObj;
    private GameObject currentTowerPlacing;
    private GameObject dummyPlacement;
    private GameObject hoverTile;

    private void Start()
    {
    }

    public Vector2 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    public void CurrentHoverTile()
    {
        // Get mouse position in world space
        Vector2 mousePosition = GetMousePosition();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 0), 0.1f, mask, -100, 100);

        if(hit.collider != null)
        {
            // Check if its a map tile
            if(MapGenerator.mapTiles.Contains(hit.collider.gameObject))
            {
                // Check if a map tile is not part of the path tile
                if(!MapGenerator.pathTiles.Contains(hit.collider.gameObject))
                {
                    hoverTile = hit.collider.gameObject;
                }
            }
        }
    }

    public bool CheckForTower()
    {
        Vector2 mousePosition = GetMousePosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 0), 0.1f, towerMask, -100, 100);

        // if hit collider is not null, means there is a tower
        if(hit.collider != null )
        {
            towerOnSlot = true;
        }
        return towerOnSlot;
    }

    public void PlaceBuilding()
    {
        /* If a mouse hovers on tile that is not null
         * and if there is no tower there
         * and if a player has enough money
         * then build the tower on that tile
         */
        if(hoverTile != null)
        {
            if(CheckForTower() == false)
            {
                if (shopManager.CanBuyTower(currentTowerPlacing) == true)
                {
                    GameObject newTowerObj = Instantiate(currentTowerPlacing);
                    newTowerObj.layer = LayerMask.NameToLayer("Tower");
                    newTowerObj.transform.position = hoverTile.transform.position;

                    EndBuilding();
                    shopManager.BuyTower(currentTowerPlacing);
                }
                else
                {
                    // If a player has not enough money, let them know!
                    Debug.Log("Insufficient funds!");
                }
            }
        }
    }

    public void StartBuilding(GameObject towerToBuild)
    {
        isBuilding = true;

        currentTowerPlacing = towerToBuild;
        // Dummy is just an image of a tower with nothing attached to it
        dummyPlacement = Instantiate(currentTowerPlacing);

        // Destroy the dummy when a real tower is being built
        if(dummyPlacement.GetComponent<Tower>() != null) { Destroy(dummyPlacement.GetComponent<Tower>()); }
        if(dummyPlacement.GetComponent<BarrelRotation>() != null) { Destroy(dummyPlacement.GetComponent<BarrelRotation>()); }
    }

    public void EndBuilding()
    {
        // Destroy dummy when builbing is finished
        isBuilding = false;

        if(dummyPlacement != null) { Destroy(dummyPlacement); }
    }

    public void Update()
    {
        if (isBuilding == true)
        {
            if (dummyPlacement != null)
            {
                CurrentHoverTile();

                if (hoverTile != null)
                {
                    dummyPlacement.transform.position = hoverTile.transform.position;
                }
            }
            // Place tower on click first mouse button
            if(Input.GetButtonDown("Fire1"))
            {
                PlaceBuilding();
            }
        }
    }
}
