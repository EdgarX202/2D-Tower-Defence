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
                tooltip = string.Format("<color=#FF8500><size=20><b>Flame</b></size></color>\nDamage: {0} \nFlame chance: {1}% \n<color=#D3FBFB>Has a chance to deal \nadditional damage</color>", flame.Damage, flame.Proc);
                    break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#B60AB8><size=20><b>Poison</b></size></color>\nDamage: {0} \nDrip chance: {1}% \n<color=#D3FBFB>Target might leave \na poison drip</color>", poison.Damage, poison.Proc);
                    break;
            case "Electric":
                ElectricTower electric = towerPrefab.GetComponentInChildren<ElectricTower>();
                tooltip = string.Format("<color=#FFE666><size=20><b>Electric</b></size></color>\nDamage: {0} \nElectric charge chance: {1}% \n<color=#D3FBFB>Has a chance to inflict damage \nto multiple targets</color>", electric.Damage, electric.Proc);
                    break;
            case "Ice":
                IceTower ice = towerPrefab.GetComponentInChildren<IceTower>();
                tooltip = string.Format("<color=#1D88FA><size=20><b>Ice</b></size></color>\nDamage: {0} \nFrost chance: {1}% \n<color=#D3FBFB>Has a chance to slow enemies down</color>", ice.Damage, ice.Proc);
                    break;
            //case "Normal":
            //    IceTower ice = towerPrefab.GetComponentInChildren<IceTower>();
            //    tooltip = string.Format("<color=#add8e6ff><size=20><b>Ice</b></size></color>\nDamage: {0} \nProc: {1}% \nDebuff duration: {2}sec \nSlowing effect: {3}% \n- Slow down effect", ice.Damage, ice.Proc, ice.DebuffDuration, ice.SlowingFactor);
            //    break;
            default:
                    break;
        }

        GameManager.Instance.SetTooltipTxt(tooltip);
        GameManager.Instance.ShowTooltip();
    }
}
