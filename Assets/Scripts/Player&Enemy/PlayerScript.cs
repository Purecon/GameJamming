using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : EntityScript
{
    //Special to player only
    [Header("Player Buff")]
    public float physicalAttackBuff = 0f;

    [Header("Player Mana")]
    public float currentMana = 10f;
    public float maxMana = 10f;
    public Slider manaBar;

    [System.Serializable]
    public class SkillCooldown
    {
        public string name;
        // Skill cooldown 0 means usable
        public int crntCooldown;
        public int defaultCooldown;

        public SkillCooldown(string skillName, int skillCooldown)
        {
            name = skillName;
            crntCooldown = 0;
            defaultCooldown = skillCooldown;
        }
    }
    [Header("Player Skill Cooldown -Edit in code-")]
    public List<SkillCooldown> skillCooldowns;

    public override void Start()
    {
        base.Start();

        skillCooldowns = new List<SkillCooldown>()
        {
            new SkillCooldown("AOE_Attack", 3),
            new SkillCooldown("AOE_M.Attack",3)
        };
    }


    public override void Attack(EntityScript targetEntity, string turnType)
    {
        Debug.LogFormat("{0} {2} {1}", name, targetEntity.name, turnType);
        //Attack the enemy
        if (currentState.currentTeam != targetEntity.currentState.currentTeam)
        {
            if(turnType.Contains("magic-attack"))
            {
                //Change health script
                float magicDamage = -currentState.magicAttackDamage * (targetEntity.currentState.magicResistance ? 0.5f : 1f) * (targetEntity.currentState.magicVulnerability ? 2f : 1f);
                targetEntity.healthScript.ChangeHealth(magicDamage);
                //Change current state
                targetEntity.currentState.currentHealth = targetEntity.healthScript.currentHealth;
                //Minus current mana
                Debug.Log("DEBUGGING" + turnType[turnType.Length - 1].ToString());
                float manaValue = float.Parse(turnType[turnType.Length - 1].ToString());
                ChangeMana(-manaValue);
            }
            else
            {
                //Change health script
                float physDamage = -(currentState.physicalAttackDamage + physicalAttackBuff) * (targetEntity.currentState.physicalResistance ? 0.5f : 1f) * (targetEntity.currentState.physicalVulnerability ? 2f : 1f);
                targetEntity.healthScript.ChangeHealth(physDamage);
                //Change current state
                targetEntity.currentState.currentHealth = targetEntity.healthScript.currentHealth;
            }
        }

        //Buff
        physicalAttackBuff++;
    }

    // AOE attack
    public void AOEAttack(List<EnemyScript> targetEntity, string turnType)
    {
        foreach (EnemyScript enemyTargetScript in targetEntity)
        {
            Attack(enemyTargetScript, turnType);
        }
    }

    //Change mana
    public void ChangeMana(float change)
    {
        //Damage for negative change
        //if (change < 0)
        //{
        //    Debug.LogFormat("Mana change for {0}", change * -1);
        //}

        //Change
        currentMana += change;
        Mathf.Clamp(currentMana, 0, maxMana);

        //For better look clamp
        manaBar.value = Mathf.Clamp(currentMana / maxMana, 0.1f, maxMana);
        //State value
        currentState.currentMana = currentMana;
    }

    //Set mana
    public void SetMana(float newMana)
    {
        //Change
        currentMana = newMana;
        Mathf.Clamp(currentMana, 0, maxMana);

        //For better look clamp
        manaBar.value = Mathf.Clamp(currentMana / maxMana, 0.1f, maxMana);
        //State value
        currentState.currentMana = currentMana;
    }
}
