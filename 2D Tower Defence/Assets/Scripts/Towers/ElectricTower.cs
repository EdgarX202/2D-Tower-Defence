using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTower : Tower
{
    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.ELECTRIC;
    }
    public override Debuff GetDebuff()
    {
        return new ElectricDebuff(Target);
    }
}
