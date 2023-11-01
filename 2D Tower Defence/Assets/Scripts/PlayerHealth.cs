using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    public float startingHealth;

    private void Start()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = startingHealth;
    }

    // Subtract player health when taking damage
    public void DamagePlayer(float amount)
    {
        currentHealth -= amount;
    }

    // Get players current health
    public float GetCurrentPlayerHealth()
    {
        return currentHealth;
    }
}
