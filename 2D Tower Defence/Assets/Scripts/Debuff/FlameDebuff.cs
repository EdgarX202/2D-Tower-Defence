using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDebuff : Debuff
{
    private float timeTick;
    private float timeSinceLastTick;
    private float damageTick;

    // Constructor
    public FlameDebuff(float damageTick, float timeTick, float duration, Enemy target) : base(target, duration)
    {
        this.damageTick = damageTick;
        this.timeTick = timeTick;
    }

    public override void Update()
    {
        // If debuff has a target
        if (target != null)
        {
            timeSinceLastTick += Time.deltaTime;

            if(timeSinceLastTick >= timeTick)
            {
                timeSinceLastTick = 0;

                target.TakeDamage(damageTick, Element.FLAME);
            }
        }

        base.Update();
    }
}
