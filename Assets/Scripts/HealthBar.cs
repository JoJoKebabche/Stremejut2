using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public float health;
    public float maxHealth = 100f;

    // 100 health = 1 fill amount
    // 45 health = 0.45 fill amount

    void Start()
    {
        health = maxHealth;
        fillBar.fillAmount = 1f;
    }

    public void LoseHealth(int value)
    {

        Player player = FindFirstObjectByType<Player>();

        if (player.IsInvincible())
            return;

        // Do nothing if you have no health
        if (health <= 0)
            return;

        // Reduce the health
        health -= value;
        // Refresh the UI fillBar
        health = Mathf.Clamp(health, 0, maxHealth);

        fillBar.fillAmount = health / maxHealth;
        // Check if your health is zero or less => dead
        if (health <= 0)
        {
            FindFirstObjectByType<Player>().Die();
        }
    }
    public void Heal(int value)
    {
        // Increase health
        health += value;

        // Clamp so it doesn't go over maxHealth
        health = Mathf.Clamp(health, 0, maxHealth);

        // Update UI
        fillBar.fillAmount = health / maxHealth;
    }

    public float GetCurrentHealth()
    {
        return health;
    }
}
