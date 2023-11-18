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
    }
    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(_splashDamage, _timeTick, splashPrefab, DebuffDuration, Target);
    }
}
