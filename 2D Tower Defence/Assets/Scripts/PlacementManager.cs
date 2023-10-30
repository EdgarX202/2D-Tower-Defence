using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public ShopManager shopManager;

    public GameObject basicTowerObj;

    private GameObject currentTowerPlacing;

    private GameObject dummyPlacement;

    public Camera cam;

    private GameObject hoverTile;

    public LayerMask mask;
    public LayerMask towerMask;

    public bool isBuilding;

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
        bool towerOnSlot = false;

        Vector2 mousePosition = GetMousePosition();

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, new Vector2(0, 0), 0.1f, towerMask, -100, 100);

        if(hit.collider != null )
        {
            towerOnSlot = true;
        }

        return towerOnSlot;
    }

    public void PlaceBuilding()
    {
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
                    Debug.Log("Insufficient funds!");
                }
            }
        }
    }

    public void StartBuilding(GameObject towerToBuild)
    {
        isBuilding = true;

        currentTowerPlacing = towerToBuild;

        dummyPlacement = Instantiate(currentTowerPlacing);

        if(dummyPlacement.GetComponent<Tower>() != null)
        {
            Destroy(dummyPlacement.GetComponent<Tower>());
        }

        if(dummyPlacement.GetComponent<BarrelRotation>() != null)
        {
            Destroy(dummyPlacement.GetComponent<BarrelRotation>());
        }
    }

    public void EndBuilding()
    {
        isBuilding = false;

        if(dummyPlacement != null)
        {
            Destroy(dummyPlacement);
        }
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

            if(Input.GetButtonDown("Fire1"))
            {
                PlaceBuilding();
            }
        }
    }
}
