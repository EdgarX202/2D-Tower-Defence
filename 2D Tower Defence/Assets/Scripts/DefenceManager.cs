using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceManager
{
    private int defenceDamage;
    private int defenceHealth;
    private int defenceCost;

    // Properties
    public int DefenceDamage
    {
        get { return defenceDamage; }
        set { defenceDamage = value; }
    }
    public int DefenceHealth
    {
        get { return defenceHealth; }
        set { defenceHealth = value; }
    }
    public int DefenceCost
    {
        get { return defenceCost; }
        set { defenceCost = value; }
    }

    // Constructor
    public DefenceManager(int damage, int health, int cost)
    {
        DefenceDamage = damage;
        DefenceHealth = health;
        DefenceCost = cost;
    }
}
