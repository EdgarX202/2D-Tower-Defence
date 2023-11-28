using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NormalTower : Tower
{


    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.NONE;

        Upgrades = new TowerUpgrade[]
        {
            // Lvl 2 Upgrade
            new TowerUpgrade(5,4),
            // Lvl 3 Upgrade
            new TowerUpgrade(7,6)
        };
    }

    public override Debuff GetDebuff()
    {
        throw new System.NotImplementedException();
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //If next upgrade availabe, show this
        {
            //return String.Format("<color=#3BE87B>{0}</color>{1} \nDamage: {2} <color=#00ff00ff>+{3}</color>", "<size=20><b>Regular</b></size>", base.GetStats(), Damage, NextUpgrade.Damage);
        }

        // If no more upgrades, show this
        return String.Format("<color=#3BE87B>{0}</color>{1}", "<size=20><b>Regular</b></size>", base.GetStats());
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }
}
