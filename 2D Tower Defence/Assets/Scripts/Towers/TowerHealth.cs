using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    /*
     * Script for tower health - NOT IMPLEMENTED YET
     */

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
