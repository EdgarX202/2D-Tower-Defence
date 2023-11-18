using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower
{
    [SerializeField] private float _slowingFactor;

    private void Start()
    {
        ElementType = Element.ICE;
    }

    public override Debuff GetDebuff()
    {
        return new IceDebuff(_slowingFactor, Target, DebuffDuration);
    }
}
