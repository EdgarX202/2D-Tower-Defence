using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TowerUpgrade
{
    // Properties
    public int Price { get; private set; }
    public int Damage { get; private set; }
    public float DebuffDuration { get; private set; }
    public float Proc { get; private set; }
    public float SlowingFactor { get; private set; }
    public float TimeTick { get; private set; }
    public int SpecialDamage { get; private set; }

    // Constructors
    public TowerUpgrade(int price, int damage, float debuffDuration, float proc)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffDuration;
        this.Proc = proc;
        this.Price = price;
    }

    public TowerUpgrade(int price, int damage, float debuffDuration, float proc, float slowingFactor)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffDuration;
        this.Proc = proc;
        this.SlowingFactor = slowingFactor;
        this.Price = price;
    }

    public TowerUpgrade(int price, int damage, float debuffDuration, float proc, float timeTick, int specialDamage)
    {
        this.Damage = damage;
        this.DebuffDuration = debuffDuration;
        this.Proc = proc;
        this.SpecialDamage = specialDamage;
        this.TimeTick = timeTick;
        this.Price = price;
    }
}
