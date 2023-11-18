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
    }
    public override Debuff GetDebuff()
    {
        return new FlameDebuff(DamageTick, timeTick, DebuffDuration, Target);
    }
}
