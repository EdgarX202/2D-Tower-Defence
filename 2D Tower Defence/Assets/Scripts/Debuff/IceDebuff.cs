using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDebuff : Debuff
{
    private float _slowingFactor;
    private bool _applied;

    // Constructor
    public IceDebuff(float slowingFactor, Enemy target, float duration) : base(target, duration)
    {
        this._slowingFactor = slowingFactor;
    }

    // Enable debuff effect - slow down the enemy
    public override void Update()
    {
        if(target != null)
        {
            if(!_applied)
            {
                // Apply debuf to slow enemy speed
                _applied = true;
                target.EnemySpeed -= (target.MaxSpeed * _slowingFactor) / 100;
            }
        }
        base.Update();
    }

    // Remove debuff, return to normal speed
    public override void Remove()
    {
        // BUGS ALL OVER THE PLACE
        //target.EnemySpeed = target.MaxSpeed;

        base.Remove();
    }
}
