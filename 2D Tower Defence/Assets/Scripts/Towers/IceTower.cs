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
        ElementType = Element.ICE;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,1,1,2,10),
            new TowerUpgrade(2,1,1,2,20)
        };
    }

    public override Debuff GetDebuff()
    {
        return new IceDebuff(SlowingFactor, Target, DebuffDuration);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //If the next is avaliable
        {
            return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}% <color=#00ff00ff>+{3}</color>", "<size=20><b>Frost</b></size>", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }

        //Returns the current upgrade
        return String.Format("<color=#00ffffff>{0}</color>{1} \nSlowing factor: {2}%", "<size=20><b>Frost</b></size>", base.GetStats(), SlowingFactor);
    }

    public override void Upgrade()
    {
        this._slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}
