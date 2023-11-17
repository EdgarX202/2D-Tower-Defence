using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    private void Start()
    {
        base.SetRenderer();
        ElementType = Element.FLAME;
    }
}
