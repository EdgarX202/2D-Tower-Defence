using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDebuff : Debuff
{
    // Private
    private float _timeTick;
    private float _timeSinceLastTick;
    private float _damageTick;

    // Constructor
    public FlameDebuff(float damageTick, float timeTick, float duration, Enemy target) : base(target, duration)
    {
        this._damageTick = damageTick;
        this._timeTick = timeTick;
    }

    // Enable debuff damage - deal extensive damage
    public override void Update()
    {
        // If debuff has a target
        if (target != null)
        {
            // Increase timer since last tick
            _timeSinceLastTick += Time.deltaTime;

            // If the time that has passed is larger than the time that should pass before taking damage again
            if(_timeSinceLastTick >= _timeTick)
            {
                // Reset the timer
                _timeSinceLastTick = 0;

                // Deal damage
                target.TakeDamage(_damageTick, Element.FLAME);
            }
        }
        base.Update();
    }
}
