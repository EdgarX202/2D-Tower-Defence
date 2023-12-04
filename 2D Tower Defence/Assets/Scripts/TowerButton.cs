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
    public GameObject TowerPrefab
        {
            get { return towerPrefab; }
        }

    private void Start()
    {
        priceTxt.text = price.ToString();
        GameManager.Instance.ChangeC += new ChangeOfCurrency(PriceCheck);
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

    // Show tower information in the tooltip
    public void ShowInformation(string type)
    {
        string tooltip = string.Empty;

        switch (type)
        {
            case "Flame":
                FireTower flame = towerPrefab.GetComponentInChildren<FireTower>();
                tooltip = string.Format("<color=#D93500><size=20><b>Flame Tower</b></size></color>\nDamage: {0} \nFlame chance: {1}% \n<color=#D3FBFB>Has a chance to deal \nadditional damage</color>", flame.Damage, flame.Proc);
                    break;
            case "Poison":
                PoisonTower poison = towerPrefab.GetComponentInChildren<PoisonTower>();
                tooltip = string.Format("<color=#B60AB8><size=20><b>Poison Tower</b></size></color>\nDamage: {0} \nDrip chance: {1}% \n<color=#D3FBFB>Target might leave \na poison drip</color>", poison.Damage, poison.Proc);
                    break;
            case "Electric":
                ElectricTower electric = towerPrefab.GetComponentInChildren<ElectricTower>();
                tooltip = string.Format("<color=#FFE666><size=20><b>Electric Tower</b></size></color>\nDamage: {0} \nElectric charge chance: {1}% \n<color=#D3FBFB>Has a chance to inflict damage \nto multiple targets</color>", electric.Damage, electric.Proc);
                    break;
            case "Ice":
                IceTower ice = towerPrefab.GetComponentInChildren<IceTower>();
                tooltip = string.Format("<color=#1DDDFA><size=20><b>Ice Tower</b></size></color>\nDamage: {0} \nFrost chance: {1}% \n<color=#D3FBFB>Has a chance to slow enemies down</color>", ice.Damage, ice.Proc);
                    break;
            case "Normal":
                NormalTower normal = towerPrefab.GetComponentInChildren<NormalTower>();
                tooltip = string.Format("<color=#3BE87B><size=20><b>Regular Tower</b></size></color>\nDamage: {0} \n<color=#D3FBFB>No effects. Basic.</color>", normal.Damage);
                break;
            default:
                    break;
        }

        GameManager.Instance.SetTooltipTxt(tooltip);
        GameManager.Instance.ShowTooltip();
    }
}
