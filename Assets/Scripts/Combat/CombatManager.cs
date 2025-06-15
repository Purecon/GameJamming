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

    //TODO: Player and Enemy Turn
    public void TurnCombat()
    {
        //TODO: Ideally use a button to trigger this, for now through script
    }
}
