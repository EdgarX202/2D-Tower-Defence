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
            // Lvl 2 Upgrade
            new TowerUpgrade(5,6,1,0,1,10),
            // Lvl 3 Upgrade
            new TowerUpgrade(10,10,1,0,1,5)
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
            return string.Format("<color=#FFE666>{0}</color>{1} \nElectric charge chance: {2}%", "<size=20><b>Electric</b></size> ", base.GetStats(), Proc);
        }

        // Return the current upgrade
        return string.Format("<color=#FFE666>{0}</color>{1} \nElectric charge chance: {2}%", "<size=20><b>Electric</b></size> ", base.GetStats(), Proc);
    }
}
