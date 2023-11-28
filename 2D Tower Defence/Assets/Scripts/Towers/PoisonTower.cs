using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
{
    [SerializeField] private float _timeTick;
    [SerializeField] private PoisonSplash splashPrefab;
    [SerializeField] private int _splashDamage;

    // Properties
    public float TimeTick { get { return _timeTick; } }
    public int SplashDamage { get { return _splashDamage; } }

    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.POISON;

        Upgrades = new TowerUpgrade[]
        {
            // Lvl 2 Upgrade
            new TowerUpgrade(10,6,1,0,1,3),
            // Lvl 3 Upgrade
            new TowerUpgrade(12,9,1,0,1,6)
        };
    }
    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(_splashDamage, _timeTick, splashPrefab, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null) //If next upgrade availabe, show this
        {
            return string.Format("<color=#B60AB8>{0}</color>{1} \nDrip chance: {2}%", "<size=20><b>Poison</b></size>", base.GetStats(), Proc);
        }

        // If no more upgrades, show this
        return string.Format("<color=#B60AB8>{0}</color>{1} \nDrip chance: {2}%", "<size=20><b>Poison</b></size>", base.GetStats(), Proc);
    }

    public override void Upgrade()
    {
        this._splashDamage += NextUpgrade.SpecialDamage;
        this._timeTick -= NextUpgrade.TimeTick;
        base.Upgrade();
    }
}
