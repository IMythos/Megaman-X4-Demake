using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Elements")]
    public Image bossHealthBarFill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (bossHealthBarFill != null)
        {
            bossHealthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Zero Boss ha sido derrotado!");
    }
}
