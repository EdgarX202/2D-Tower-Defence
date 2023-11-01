using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    // Reference to PlayerHealth class
    public PlayerHealth playerHealth;

    public Image healthBar;
    public TMP_Text healthBarText;

    private void Update()
    {
        // Creating health bar fill and text
        healthBar.fillAmount = playerHealth.GetCurrentPlayerHealth() / playerHealth.startingHealth;
        healthBarText.text = "Health: " + Mathf.Floor(playerHealth.GetCurrentPlayerHealth()) + "/" + playerHealth.startingHealth;
    }
}
