using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityState
{
    public string entityName;

    [Header("Attack")]
    public float physicalAttackDamage = 1f;
    public float magicAttackDamage = 1f;

    [Header("Health")]
    public float currentHealth = 10f;
    public float maxHealth = 10f;

    [Header("Mana")]
    public float currentMana = 10f;
    public float maxMana = 10f;

    [Header("Resistances")]
    public bool physicalResistance = false;
    public bool magicResistance = false;

    [Header("Vulnerabilities")]
    public bool physicalVulnerability = false;
    public bool magicVulnerability = false;

    public enum Teams
    {
        Player,
        Enemy
    }

    [Header("Team")]
    public Teams currentTeam;

    //Deep copy
    public EntityState Clone()
    {
        return new EntityState
        {
            entityName = this.entityName,
            physicalAttackDamage = this.physicalAttackDamage,
            magicAttackDamage = this.magicAttackDamage,

            maxHealth = this.maxHealth,
            currentHealth = this.currentHealth,
            maxMana = this.maxMana,
            currentMana = this.currentMana,
            
            physicalResistance = this.physicalResistance,
            magicResistance = this.magicResistance,
            physicalVulnerability = this.physicalVulnerability,
            magicVulnerability = this.magicVulnerability,

            currentTeam = this.currentTeam,
        };
    }
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
    public virtual void Attack(EntityScript targetEntity, string turnType)
    {
        Debug.LogFormat("{0} {2} {1}", name, targetEntity.name, turnType);
        //For succesful attack
        if (currentState.currentTeam != targetEntity.currentState.currentTeam)
        {
            //Change health script
            float physDamage = -(currentState.physicalAttackDamage) * (targetEntity.currentState.physicalResistance ? 0.5f : 1f) * (targetEntity.currentState.physicalVulnerability ? 2f : 1f);
            targetEntity.healthScript.ChangeHealth(physDamage);
            //Change current state
            targetEntity.currentState.currentHealth = targetEntity.healthScript.currentHealth;
        }
    }

    //Get initial settings 
    private void Start()
    {
        if (entityData != null)
        {
            //Set intial state
            currentState = entityData.state.Clone();
            Debug.Log($"{name} currentState ref: {currentState.GetHashCode()}");
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
