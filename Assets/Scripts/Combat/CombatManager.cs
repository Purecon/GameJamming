using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : Singleton<CombatManager>
{
    [Header("Turn Manager")]
    public TurnManager turnManager;

    //Enemies
    [Header("Enemy")]
    public GameObject enemiesGameObject;

    //Player
    [Header("Player")]
    public GameObject playerGameObject;

    //Scripts data
    private EntityScriptsAllData entityScriptsAllData;

    //Actions
    private Dictionary<string, System.Action<PlayerScript, EntityScript>> playerActions;

    //Targeted enemy
    private EnemyScript targetedEnemy;

    public EnemyScript[] GetEnemiesScript()
    {
        return enemiesGameObject.GetComponentsInChildren<EnemyScript>();
    }

    public PlayerScript GetPlayerScript()
    {
        return playerGameObject.GetComponent<PlayerScript>();
    }

    [System.Serializable]
    public class EntityScriptsAllData
    {
        public List<EnemyScript> enemyScripts;
        public PlayerScript playerScript;

        public EntityScriptsAllData(List<EnemyScript> enemyScripts, PlayerScript playerScript)
        {
            this.enemyScripts = enemyScripts;
            this.playerScript = playerScript;
        }
    }

    public EntityScriptsAllData GetEntityScriptsData()
    {
        List<EnemyScript> enemyScripts = new List<EnemyScript>(GetEnemiesScript());
        PlayerScript playerScript = GetPlayerScript();
        // Convert to List for easy addition
        EntityScriptsAllData entityScriptsAllData = new EntityScriptsAllData(enemyScripts, playerScript);

        return entityScriptsAllData;
    }

    private void Start()
    {
        if(entityScriptsAllData == null)
        {
            entityScriptsAllData = GetEntityScriptsData();
        }

        //Set enemy ID to be unique
        foreach(EnemyScript enemyScript in entityScriptsAllData.enemyScripts)
        {
            enemyScript.currentState.entityName += enemyScript.gameObject.name; 
        }

        // Player Action
        playerActions = new Dictionary<string, System.Action<PlayerScript, EntityScript>>()
        {
            { "Attack", (player, target) => player.Attack(target,"attack")},
            { "M.Attack", (player, target) => player.Attack(target,"magic-attack5")},
            { "AOE_Attack", (player, target) => player.AOEAttack(entityScriptsAllData.enemyScripts,"attack")},
            { "AOE_M.Attack", (player, target) =>  player.AOEAttack(entityScriptsAllData.enemyScripts,"magic-attack1")},

            { "TimeMagic", (player, target) => turnManager.ChangeToPreviousTurn(1,entityScriptsAllData)},
        };

        //Turn 0
        turnManager.StartNewTurn(entityScriptsAllData);

        //Select default enemy 
        if(entityScriptsAllData.enemyScripts != null)
        {
            targetedEnemy = entityScriptsAllData.enemyScripts[0];
            targetedEnemy.SetTargeted(true);
        }
    }

    public void SetEnemyTarget(EnemyScript newTarget)
    {
        //Disable old target
        targetedEnemy.SetTargeted(false);
        //New target
        targetedEnemy = newTarget;
        targetedEnemy.SetTargeted(true);
    }

    //Player and Enemy Turn
    public void TurnCombat(string playerTurnType)
    {
        //Get script
        PlayerScript playerScript = entityScriptsAllData.playerScript;
        List<EnemyScript> enemyScripts = entityScriptsAllData.enemyScripts;

        //Check cooldown
        PlayerScript.SkillCooldown skillCD = playerScript.skillCooldowns.Find(x => x.name == playerTurnType);
        if(skillCD != null)
        {
            if (skillCD.crntCooldown > 0)
            {
                return;
            }
            else
            {
                skillCD.crntCooldown = skillCD.defaultCooldown;
            }
        }

        //Reduce cooldown
        foreach(PlayerScript.SkillCooldown playerSkillCD in playerScript.skillCooldowns)
        {
            if(playerSkillCD.name != playerTurnType)
            {
                playerSkillCD.crntCooldown = Mathf.Max(0, playerSkillCD.crntCooldown-1);   
            }
        }

        //Player action
        playerActions[playerTurnType](playerScript,targetedEnemy);

        if(playerTurnType!= "TimeMagic")
        {
            //TODO: Check death, add death screen
            if (playerScript.healthScript.CheckDeath())
            {
                Debug.Log("Player DIED");
                Destroy(playerGameObject);
            }
            //TODO: Enemy death
            if (targetedEnemy.healthScript.CheckDeath())
            {
                enemyScripts.Remove(targetedEnemy);
                targetedEnemy.gameObject.SetActive(false);

                if (enemyScripts.Count > 0)
                {
                    targetedEnemy = enemyScripts[0];
                    targetedEnemy.SetTargeted(true);
                    Debug.Log("Targeted enemy " + targetedEnemy.name);
                }
                else
                {
                    //TODO: WIN
                    Debug.Log("YOU WIN!");
                }
            }

            //Each enemies attack
            foreach (EnemyScript enemyScript in enemyScripts)
            {
                enemyScript.Attack(playerScript, "attack");
            }
            
            //End of turn
            entityScriptsAllData = GetEntityScriptsData();
            turnManager.StartNewTurn(entityScriptsAllData);
        }
    }
}
