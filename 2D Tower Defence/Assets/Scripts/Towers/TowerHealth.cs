using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [SerializeField] private Stats health;

    public bool IsAlive
    {
        // Return true is health is more than 0, otherwise the tower is dead
        get { return health.CurrentVal > 0; }
    }

    private void Awake()
    {
        health.Initialize();
    }
}
