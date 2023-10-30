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

    public void DamagePlayer(float amount)
    {
        currentHealth -= amount;
    }

    public float GetCurrentPlayerHealth()
    {
        return currentHealth;
    }
}
