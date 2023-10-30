using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public CurrencyManager currencyManager;

    public GameObject basicTowerPrefab;

    public int basicTowerCost;

    public int GetTowerCost(GameObject towerPrefab)
    {
        int cost = 0;

        if(towerPrefab == basicTowerPrefab)
        {
            cost = basicTowerCost;
        }

        return cost;
    }

    public void BuyTower(GameObject towerPrefab)
    {
        currencyManager.RemoveMoney(GetTowerCost(towerPrefab));
    }

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
