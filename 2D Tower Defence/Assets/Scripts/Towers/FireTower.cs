using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    // Serialised Fields
    [SerializeField] private float _timeTick;
    [SerializeField] private float _damageTick;

    // Properties
    public float TimeTick { get { return _timeTick; } }
    public float DamageTick { get { return _damageTick; } }

    private void Start()
    {
        base.SetRenderer();

        // Type of the tower
        ElementType = Element.FLAME;

        // Tower upgrades
        Upgrades = new TowerUpgrade[]
        {
            // Lvl 2 Upgrade
            new TowerUpgrade(6,4,1,0,1,3),
            // Lvl 3 Upgrade
            new TowerUpgrade(9,5,1,0,1,4)
        };
    }

    public override Debuff GetDebuff()
    {
        return new FlameDebuff(DamageTick, _timeTick, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        // If next upgrade is available
        if (NextUpgrade != null)
        {
            // Return new upgrade stats
            return string.Format("<color=#D93500>{0}</color>{1} \nFlame chance: {2}%", "<size=20><b>Flame</b></size> ", base.GetStats(), Proc);
        }

        // Return the current upgrade
        return string.Format("<color=#D93500>{0}</color>{1} \nFlame chance: {2}%", "<size=20><b>Flame</b></size> ", base.GetStats(), Proc);
    }

    public override void Upgrade()
    {
        // Increase damage, reduce tick time
        this._timeTick -= NextUpgrade.TimeTick;
        this._damageTick += NextUpgrade.SpecialDamage;
        base.Upgrade();
    }
}
