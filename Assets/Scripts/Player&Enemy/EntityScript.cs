using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityState
{
    [Header("Attack")]
    public float physicalAttackDamage = 1f;
    public float magicAttackDamage = 1f;

    [Header("Health")]
    public float currentHealth = 10f;
    public float maxHealth = 10f;

    public enum Teams
    {
        Player,
        Enemy
    }

    [Header("Team")]
    public Teams currentTeam;
}

public class EntityScript : MonoBehaviour
{
    [Header("EntityData -- THIS WILL REWRITE THE STATS")]
    public EntityData entityData;

    [Header("EntityState")]
    public EntityState currentState;

    [Header("HealthScript")]
    public HealthScript healthScript;

    //Attack target
    public virtual void Attack(EntityScript targetEntity)
    {
        Debug.LogFormat("{0} attack {1}", name, targetEntity.name);
        //For succesful attack
        if (currentState.currentTeam != targetEntity.currentState.currentTeam)
        {
            targetEntity.healthScript.ChangeHealth(-currentState.physicalAttackDamage);
        }
    }

    //Get initial settings 
    private void Start()
    {
        if (entityData != null)
        {
            //Set intial state
            currentState = entityData.state;
        }
        else
        {
            Debug.LogWarning("EntityData is not assigned to " + gameObject.name);
        }

        //Set initial health
        healthScript = gameObject.GetComponent<HealthScript>();
        healthScript.maxHealth = currentState.maxHealth;
        healthScript.ResetCurrHealth();
    }
}
