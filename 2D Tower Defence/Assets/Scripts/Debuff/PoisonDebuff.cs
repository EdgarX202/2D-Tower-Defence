using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    // Private
    private PoisonSplash _splashPrefab;
    private float _timeSinceLastTick;
    private float _timeTick;
    private int _splashDamage;

    // Constructor
    public PoisonDebuff(int splashDamage, float timeTick, PoisonSplash splashPrefab, float duration, Enemy target) : base(target,duration)
    {
        this._splashDamage = splashDamage;
        this._timeTick = timeTick;
        this._splashPrefab = splashPrefab;
    }

    // Enable debuff effect - drop poison on the ground
    public override void Update()
    {
        if(target != null)
        {
            _timeSinceLastTick += Time.deltaTime;

            if(_timeSinceLastTick >= _timeTick)
            {
                // Reset
                _timeSinceLastTick = 0;
                // Drop poison
                Splash();
            }
        }
        base.Update();
    }

    // Instantiate poison prefab
    private void Splash()
    {
        PoisonSplash tmp = GameObject.Instantiate(_splashPrefab, target.transform.position, Quaternion.identity);

        tmp.Damage = _splashDamage;

        // Ignore collision between the target and the poison splash
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
