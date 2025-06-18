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

        // Player Action
        playerActions = new Dictionary<string, System.Action<PlayerScript, EntityScript>>()
        {
            { "Attack", (player, target) => player.Attack(target,"attack")},
            { "M.Attack", (player, target) => player.Attack(target,"magic-attack5")},
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

    //TODO: Player and Enemy Turn
    public void TurnCombat(string playerTurnType)
    {
        //Get script
        PlayerScript playerScript = entityScriptsAllData.playerScript;
        List<EnemyScript> enemyScripts = entityScriptsAllData.enemyScripts;

        playerActions[playerTurnType](playerScript,targetedEnemy);
        //playerScript.Attack(targetedEnemy);

        if(playerTurnType!= "TimeMagic")
        {
            targetedEnemy.Attack(playerScript, "attack");

            //TODO: Check death, add death screen
            if (playerScript.healthScript.CheckDeath())
            {
                Debug.Log("Player DIED");
                Destroy(playerGameObject);
            }
            //TODO: Enemy death
            if (targetedEnemy.healthScript.CheckDeath())
            {
                entityScriptsAllData.enemyScripts.Remove(targetedEnemy);
                Destroy(targetedEnemy.gameObject);

                if (entityScriptsAllData.enemyScripts.Count > 0)
                {
                    targetedEnemy = entityScriptsAllData.enemyScripts[0];
                    targetedEnemy.SetTargeted(true);
                    Debug.Log("Targeted enemy " + targetedEnemy.name);
                }
                else
                {
                    //TODO: WIN
                    Debug.Log("YOU WIN!");
                }
            }

            //End of turn
            entityScriptsAllData = GetEntityScriptsData();
            turnManager.StartNewTurn(entityScriptsAllData);
        }
    }
}
