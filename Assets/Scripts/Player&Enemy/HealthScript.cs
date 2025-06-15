using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [Header("Health")]
    public float currentHealth = 10f;
    public float maxHealth = 10f;
    public Slider healthBar;

    public enum Teams
    {
        Player,
        Enemy
    }

    [Header("Attack")]
    public float attackDamage = 1f;

    [Header("Team")]
    public Teams currentTeam;

    //Reset current health 
    public void ResetCurrHealth()
    {
        currentHealth = maxHealth;
        healthBar.value = (currentHealth / maxHealth);
    }

    //Check death
    public bool CheckDeath()
    {
        return currentHealth <= 0;
    }

    //Change health
    public void ChangeHealth(float change)
    {
        //Damage for negative change
        if (change < 0)
        {
            Debug.LogFormat("Damage for {0}",change*-1);
        }

        //Change
        currentHealth += change;
        Mathf.Clamp(currentHealth, 0, maxHealth);

        //For better look clamp
        healthBar.value = Mathf.Clamp(currentHealth / maxHealth, 0.1f, maxHealth);
    }

    //private void Start()
    //{
    //    ResetCurrHealth();
    //}
}
