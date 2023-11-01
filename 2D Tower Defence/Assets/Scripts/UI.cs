using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    // Reference to CurrencyManager
    public CurrencyManager currencyManager;
  
    public TMP_Text currencyTxt;

    private void Update()
    {
        // How much money player has
        currencyTxt.text = "C: " + currencyManager.GetCurrentMoney();
    }
}
