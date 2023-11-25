using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    /*
     * A DEBUFF - is a negative status effect that temporarily alters characters/enemies attributes or abilities.
     * E.g. poison could slow the enemy down and deal some damage while the effect is on.
     */

    // Protected
    protected Enemy target;
    protected float duration;

    // Private
    private float _elapsed;

    // Constructor
    public Debuff(Enemy target, float duration)
    {
        this.target = target;
        this.duration = duration;
    }

    // Update _elapased and remove debuffs
    public virtual void Update()
    {
        _elapsed += Time.deltaTime;

        // Remove debuffs
        if(_elapsed >= duration)
        {
            Remove();
        }
    }

    // Remove debuffs
    public virtual void Remove()
    {
        if (target != null)
        {
            target.RemoveDebuff(this);
        }
    }
}
