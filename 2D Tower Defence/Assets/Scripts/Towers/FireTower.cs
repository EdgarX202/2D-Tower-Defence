using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    [SerializeField] private float timeTick;
    [SerializeField] private float damageTick;

    // Properties
    public float TimeTick { get { return timeTick; } }
    public float DamageTick { get { return damageTick; } }

    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.FLAME;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(2,2,.5f,5,-0.1f,1),
            new TowerUpgrade(5,3,.5f,5,-0.1f,1)
        };
    }
    public override Debuff GetDebuff()
    {
        return new FlameDebuff(DamageTick, timeTick, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        // If next upgrade is available
        if (NextUpgrade != null)
        {
            // Return new upgrade stats
            return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2} <color=#00ff00ff>{4}</color>\nTick damage: {3} <color=#00ff00ff>+{5}</color>", "<size=20><b>Fire</b></size> ", base.GetStats(), TimeTick, DamageTick, NextUpgrade.TimeTick, NextUpgrade.SpecialDamage);
        }

        // Return the current upgrade
        return string.Format("<color=#ffa500ff>{0}</color>{1} \nTick time: {2}\nTick damage: {3}", "<size=20><b>Fire</b></size> ", base.GetStats(), TimeTick, DamageTick);
    }

    public override void Upgrade()
    {
        // Increase damage, reduce tick time
        this.timeTick -= NextUpgrade.TimeTick;
        this.damageTick += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
