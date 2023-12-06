using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ElectricDebuff : Debuff
{
    // Private
    private float _damage;

    // Constructor
    public ElectricDebuff(float damage, Enemy target) : base(target,1)
    {
        this.target = target;
        this._damage = damage;
    }

    public override void Update()
    {

        base.Update();
    }
}
