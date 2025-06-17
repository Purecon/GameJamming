using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : EntityScript
{
    [Header("Player Buff")]
    public float physicalAttackBuff = 0f;

    [Header("Player Mana")]
    public float currentMana = 10f;
    public float maxMana = 10f;
    public Slider manaBar;

    //TODO: Penalty

    //Special to player only
    //TODO: Buff
    public override void Attack(EntityScript targetEntity, string turnType)
    {
        Debug.LogFormat("{0} {2} {1}", name, targetEntity.name, turnType);
        //Attack the enemy
        if (currentState.currentTeam != targetEntity.currentState.currentTeam)
        {
            if(turnType.Contains("magic-attack"))
            {
                //Change health script
                targetEntity.healthScript.ChangeHealth(-(currentState.magicAttackDamage));
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
                targetEntity.healthScript.ChangeHealth(-(currentState.physicalAttackDamage + physicalAttackBuff));
                //Change current state
                targetEntity.currentState.currentHealth = targetEntity.healthScript.currentHealth;
            }
        }

        //Buff
        physicalAttackBuff++;
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
