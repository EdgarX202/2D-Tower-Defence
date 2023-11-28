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
            new TowerUpgrade(2,1,0.5f,0.1f,1),
            new TowerUpgrade(5,1,0.5f,0.1f,1)
        };
    }
    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(_splashDamage, _timeTick, splashPrefab, DebuffDuration, Target);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)
        {
            return string.Format("<color=#B60AB8>{0}</color>{1} \nDrip damage: {2} <color=#00ff00ff>+{3}</color> \nDrip chance: {4}% <color=#00ff00ff>+{5}</color>", "<size=20><b>Poison</b></size>", base.GetStats(), SplashDamage, NextUpgrade.SpecialDamage, Proc, NextUpgrade.Proc);
        }

        return string.Format("<color=#B60AB8>{0}</color>{1} \nDrip damage: {2}", "<size=20><b>Poison</b></size>", base.GetStats(), SplashDamage);
    }

    public override void Upgrade()
    {
        this._splashDamage += NextUpgrade.SpecialDamage;
        this._timeTick -= NextUpgrade.TimeTick;
        base.Upgrade();
    }
}
