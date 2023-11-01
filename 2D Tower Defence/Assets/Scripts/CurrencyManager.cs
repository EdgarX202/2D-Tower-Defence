using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int currentPlayerMoney;
    public int startMoney;

    private void Start()
    {
        // Starting amount
        currentPlayerMoney = startMoney;
    }

    // Get the amount that player has left
    public int GetCurrentMoney()
    {
        return currentPlayerMoney;
    }

    //public void AddMoney(int amount)
    //{
    //    currentPlayerMoney += amount;
    //}

    // Subtract the amount from player and output the remaining amount
    public void RemoveMoney(int amount)
    {
        currentPlayerMoney -= amount;
        Debug.Log("Removed " + amount + " from players money! " + "Player has left: " + currentPlayerMoney);
    }
}
