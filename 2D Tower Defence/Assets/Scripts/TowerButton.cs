using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceTxt;

    public int Price
    {
        get { return price; }
    }

    private void Start()
    {
        priceTxt.text = price.ToString();
        GameManager.Instance.ChangeC += new ChangeOfCurrency(PriceCheck);
    }

    public GameObject TowerPrefab
    {
        get { return towerPrefab; }
    }

    // If the player doesnt have enough currency, grey out the button
    private void PriceCheck()
    {
        if(price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceTxt.color = Color.grey;
        }
    }

    public Sprite Sprite { get => sprite; }
}
