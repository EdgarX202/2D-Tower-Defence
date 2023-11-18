using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    private float _timeSinceLastTick;
    private float _timeTick;
    private PoisonSplash splashPrefab;
    private int _splashDamage;

    // Constructor
    public PoisonDebuff(int splashDamage, float timeTick, PoisonSplash splashPrefab, float duration, Enemy target) : base(target,duration)
    {
        this._splashDamage = splashDamage;
        this._timeTick = timeTick;
        this.splashPrefab = splashPrefab;
    }

    public override void Update()
    {
        if(target != null)
        {
            _timeSinceLastTick += Time.deltaTime;

            if(_timeSinceLastTick >= _timeTick)
            {
                _timeSinceLastTick = 0;
                Splash();

            }
        }

        base.Update();
    }

    private void Splash()
    {
        PoisonSplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity);

        tmp.Damage = _splashDamage;

        // Ignore collision between the target and the poison splash
        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
