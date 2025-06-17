using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    //TODO: Player and Enemy Turn
    public void TurnCombat(string playerTurnType)
    {
        //Get script
        PlayerScript playerScript = entityScriptsAllData.playerScript;
        List<EnemyScript> enemyScripts = entityScriptsAllData.enemyScripts;

        //TODO: Correct the target
        //TEST just attack the first enemy
        EnemyScript testEnemy = enemyScripts[0];

        playerActions[playerTurnType](playerScript,testEnemy);
        //playerScript.Attack(testEnemy);

        if(playerTurnType!= "TimeMagic")
        {
            testEnemy.Attack(playerScript, "attack");

            //TODO: Check death
            if (playerScript.healthScript.CheckDeath())
            {
                Debug.Log("Player DIED");
                Destroy(playerGameObject);
            }
            //TODO: Enemy death
            //TEST just for the first enemy
            if (testEnemy.healthScript.CheckDeath())
            {
                Debug.Log("TEST DIED, YOU WIN");
                Destroy(testEnemy.gameObject);
            }

            //End of turn
            entityScriptsAllData = GetEntityScriptsData();
            turnManager.StartNewTurn(entityScriptsAllData);
        }
    }
}
