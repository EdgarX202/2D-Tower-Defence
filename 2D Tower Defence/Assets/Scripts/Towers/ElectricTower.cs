using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTower : Tower
{
    // Serialised fields
    [SerializeField] private float timeTick;
    [SerializeField] private float _damage;

    // Properties
    public float TimeTick { get { return timeTick; } }
    public float DamageTick { get { return _damage; } }

    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.ELECTRIC;

        Upgrades = new TowerUpgrade[]
        {
            new TowerUpgrade(15,22,10,10,1,5),
            new TowerUpgrade(20,25,15,15,2,10)
         };

    }
    public override Debuff GetDebuff()
    {
        return new ElectricDebuff(DamageTick, Target);
    }

    public override string GetStats()
    {
        // If next upgrade is available
        if (NextUpgrade != null)
        {
            // Return new upgrade stats
            return string.Format("<color=#FFE666>{0}</color>{1} \nElectric damage: {2} <color=#00ff00ff>+{3}</color> \nElectric charge chance: {4} <color=#00ff00ff>+{5}</color>", "<size=20><b>Electric</b></size> ", base.GetStats(), DamageTick, NextUpgrade.SpecialDamage, Proc, NextUpgrade.Proc);
        }

        // Return the current upgrade
        return string.Format("<color=#FFE666>{0}</color>{1} \nDamage: {2}", "<size=20><b>Electric</b></size> ", base.GetStats(), DamageTick);
    }
}
