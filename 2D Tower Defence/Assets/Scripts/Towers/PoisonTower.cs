using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
{
    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.POISON;
    }
}