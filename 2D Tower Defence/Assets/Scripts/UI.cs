using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public CurrencyManager currencyManager;

    public TMP_Text currencyTxt;

    private void Update()
    {
        currencyTxt.text = "$: " + currencyManager.GetCurrentMoney();
    }
}
