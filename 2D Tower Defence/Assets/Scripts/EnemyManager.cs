using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    private int enemyDamage;
    private int enemyHealth;
    private int enemyShield;

    // Properties
    public int EnemyDamage
    { 
        get { return enemyDamage; }
        set { enemyDamage = value; } 
    }
    public int EnemyHealth
    {
        get { return enemyHealth; }
        set { enemyHealth = value; } 
    }
    public int EnemyShield
    {
        get { return enemyShield; }
        set { enemyShield = value; }
    }

    // Constructor
    public EnemyManager(int damage, int health, int shield)
    {
        EnemyDamage = damage;
        EnemyHealth = health;
        EnemyShield = shield;
    }
}
