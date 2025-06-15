using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    [Header("Turn Manager")]
    public TurnManager TurnManager;

    //Enemies
    [Header("Enemy")]
    public GameObject enemiesGameObject;

    //Player
    [Header("Player")]
    public GameObject playerGameObject;

    //Scripts data
    private EntityScriptsAllData entityScriptsAllData;

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

    public EntityScriptsAllData GetEntityScripts()
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
            entityScriptsAllData = GetEntityScripts();
        }
    }

    //TODO: Player and Enemy Turn
    public void TurnCombat()
    {
        //Get script
        PlayerScript playerScript = entityScriptsAllData.playerScript;
        List<EnemyScript> enemyScripts = entityScriptsAllData.enemyScripts;

        //TODO: Correct the target
        //TEST just attack the first enemy
        EnemyScript testEnemy = enemyScripts[0];
        playerScript.Attack(testEnemy);
        testEnemy.Attack(playerScript);

        //TODO: Check death
        if (playerScript.healthScript.CheckDeath())
        {
            Debug.Log("Player DIED");
            Destroy(playerGameObject);
        }
        //TEST just for the first enemy
        if (testEnemy.healthScript.CheckDeath())
        {
            Debug.Log("TEST DIED, YOU WIN");
            Destroy(enemiesGameObject);
        }

        //TEST end turn one
        TurnManager.StartNewTurn(entityScriptsAllData);
    }
}
