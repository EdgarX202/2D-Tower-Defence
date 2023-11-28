using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower
{
    [SerializeField] private float _slowingFactor;

    public float SlowingFactor { get { return _slowingFactor; } }

    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.ICE;

        Upgrades = new TowerUpgrade[]
        {
            // Lvl 2 Upgrade
            new TowerUpgrade(6,3,1,0,5),
            // Lvl 3 Upgrade
            new TowerUpgrade(8,4,1,0,10)
        };
    }

    public override Debuff GetDebuff()
    {
        return new IceDebuff(SlowingFactor, Target, DebuffDuration);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //If next upgrade availabe, show this
        {
            return String.Format("<color=#1DDDFA>{0}</color>{1} \nFrost chance: {2}%", "<size=20><b>Ice</b></size>", base.GetStats(), Proc);
        }

        // If no more upgrades, show this
        return String.Format("<color=#1DDDFA>{0}</color>{1} \nFrost chance: {2}%", "<size=20><b>Ice</b></size>", base.GetStats(), Proc);
    }

    public override void Upgrade()
    {
        this._slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
