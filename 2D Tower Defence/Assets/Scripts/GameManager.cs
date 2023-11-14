using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TowerButton ClickedBtn { get; set; }

    private int currency;
    [SerializeField] private TextMeshProUGUI currencyTxt;

    public int Currency 
    {
        get 
        {
            return currency; 
        }
        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString();
        }
    }

    private void Start()
    {
        Currency = 50;   
    }
    private void Update()
    {
        HandleEscapeButton();
    }
    public void PickTower(TowerButton twrButton)
    {
        if (Currency >= twrButton.Price)
        {
            // Store clicked button
            this.ClickedBtn = twrButton;
            // Activate hover icon
            Hover.Instance.Activate(twrButton.Sprite);
        }
    }
    public void BuyTower()
    {
        if(Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }

    // What happens when ESC is pressed
    private void HandleEscapeButton()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }
}
