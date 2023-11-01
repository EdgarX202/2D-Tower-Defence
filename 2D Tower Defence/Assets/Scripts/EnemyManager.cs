using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
   [SerializeField] private float enemySpeed;
   [SerializeField] private int enemyHealth;
    private int enemyShield;
    private int enemyDamage;

    // Properties
    public float EnemySpeed
    {
        get { return enemySpeed; } 
        set { enemySpeed = value; }
    }
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
    public EnemyManager(int damage, int health, int shield, float speed)
    {
        EnemyDamage = damage;
        EnemyHealth = health;
        EnemyShield = shield;
        EnemySpeed = speed;
    }
}
