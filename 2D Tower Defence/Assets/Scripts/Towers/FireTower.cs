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
            return string.Format("<color=#FF8500>{0}</color>{1} \nFlame damage: {2} <color=#00ff00ff>+{3}</color> \nFlame chance: {4} <color=#00ff00ff>+{5}</color>", "<size=20><b>Flame</b></size> ", base.GetStats(), DamageTick, NextUpgrade.SpecialDamage, Proc, NextUpgrade.Proc);
        }

        // Return the current upgrade
        return string.Format("<color=#FF8500>{0}</color>{1} \nDamage: {2}", "<size=20><b>Flame</b></size> ", base.GetStats(), DamageTick);
    }

    public override void Upgrade()
    {
        // Increase damage, reduce tick time
        this.timeTick -= NextUpgrade.TimeTick;
        this.damageTick += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
