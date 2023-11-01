using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Reference to CurrencyManager
    public CurrencyManager currencyManager;

    public GameObject basicTowerPrefab;
    public int basicTowerCost;

    // Tower prices ------- add more above with more towers ---------
    public int GetTowerCost(GameObject towerPrefab)
    {
        int cost = 0;

        if(towerPrefab == basicTowerPrefab)
        {
            cost = basicTowerCost;
        }

        return cost;
    }

    // Take the money away when buying
    public void BuyTower(GameObject towerPrefab)
    {
        currencyManager.RemoveMoney(GetTowerCost(towerPrefab));
    }

    // Check if player has more money than the tower price is
    public bool CanBuyTower(GameObject towerPrefab)
    {
        int cost = GetTowerCost(towerPrefab);

        bool canBuy = false;

        if(currencyManager.GetCurrentMoney() >= cost)
        {
            canBuy = true;
        }

        return canBuy;
    }
}
