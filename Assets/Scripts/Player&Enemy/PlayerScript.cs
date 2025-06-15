using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : EntityScript
{
    [Header("Player Buff")]
    public float physicalAttackBuff = 0f;
    
    //TODO: Penalty

    //Special to player only
    //TODO: Buff
    public override void Attack(EntityScript targetEntity)
    {
        Debug.LogFormat("{0} attack {1}", name, targetEntity.name);
        //For succesful attack
        if (currentState.currentTeam != targetEntity.currentState.currentTeam)
        {
            //Change health script
            targetEntity.healthScript.ChangeHealth(-(currentState.physicalAttackDamage + physicalAttackBuff));
            //Change current state
            targetEntity.currentState.currentHealth = targetEntity.healthScript.currentHealth;
        }

        //Buff
        physicalAttackBuff++;
    }
}
