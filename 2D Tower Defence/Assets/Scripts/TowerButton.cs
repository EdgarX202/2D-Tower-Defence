using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    }

    public GameObject TowerPrefab
    {
        get { return towerPrefab; }
    }

    public Sprite Sprite { get => sprite; }
}
