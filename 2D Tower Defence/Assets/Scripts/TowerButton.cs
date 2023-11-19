using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    // Serialised fields
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceTxt;

    // Properties
    public Sprite Sprite { get => sprite; }
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

    // Show information in the tooltip
    public void ShowInformation(string type)
    {
        string tooltip = string.Empty;

        switch (type)
        {
            case "Flame":
                FireTower flame = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Flame</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nFlame effect: {3}% \n- Extra damage", flame.Damage, flame.Proc, flame.DebuffDuration, flame.DamageTick);
                    break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#00ffffff><size=20><b>Poison</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nPoison effect: {3}% \n- Poison effect", poison.Damage, poison.Proc, poison.DebuffDuration, poison.SplashDamage);
                    break;
            case "Electric":
                ElectricTower electric = towerPrefab.GetComponentInChildren<ElectricTower>();
                tooltip = string.Format("<color=#00ff00ff><size=20><b>Electric</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \n- Affects multiple enemies", electric.Damage, electric.Proc, electric.DebuffDuration);
                    break;
            case "Ice":
                IceTower ice = towerPrefab.GetComponentInChildren<IceTower>();
                tooltip = string.Format("<color=#add8e6ff><size=20><b>Ice</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nSlowing effect: {3}% \n- Slow down effect", ice.Damage, ice.Proc, ice.DebuffDuration, ice.SlowingFactor);
                    break;
            default:
                    break;
        }

        GameManager.Instance.SetTooltipTxt(tooltip);
        GameManager.Instance.ShowTooltip();
    }
}
