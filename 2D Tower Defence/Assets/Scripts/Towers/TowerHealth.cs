using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    // Serialised Fields
    [SerializeField] private Stats _health;

    

    // Properties
    public bool IsAlive
    {
        // Return true is health is more than 0, otherwise the tower is dead
        get { return _health.CurrentVal > 0; }
    }

    private void Awake()
    {
        _health.Initialize();
        
    }

    public void Health(int towerHealth)
    {
        this._health.MaxVal = towerHealth;
        this._health.CurrentVal = this._health.MaxVal;
    }

    public void TakeDamage(float damage)
    {

    }
}
