using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Turn
{
    public int turnId;
    public EntityState playerState;
    public List<EntityState> enemiesState;

    public Turn(int dataId, EntityState playerState, List<EntityState> enemiesState)
    {
        this.turnId = dataId;
        this.playerState = playerState;
        this.enemiesState = enemiesState;
    }
} 

public class TurnManager : MonoBehaviour
{
    [Header("Turn Dictionary")]
    public List<Turn> turnData;
    public int currentTurnCount = 0;

    private void Start()
    {
        //Set the turn UI
        updateTurnUI();
    }

    //This is the changed of a turn and saved the data
    public void StartNewTurn(CombatManager.EntityScriptsAllData entityScriptsData)
    {
        //Get entities
        Debug.LogFormat("EntityData:{0}", entityScriptsData);

        //Save entities
        EntityState playerState = entityScriptsData.playerScript.currentState.Clone();
        List<EntityState> enemiesState = entityScriptsData.enemyScripts.Select(e => e.currentState.Clone()).ToList();

        //DEBUGGING
        Debug.Log($"Player currentState ref: {playerState.GetHashCode()}");
        foreach (EntityState enemyState in enemiesState) {
            Debug.Log($"Enemy currentState ref: {enemyState.GetHashCode()}");
        }

        turnData.Add(new Turn(currentTurnCount, playerState, enemiesState));

        //Move the turn
        currentTurnCount++;

        //Set the turn UI
        updateTurnUI();
    }

    public void ChangeToPreviousTurn(int prevTurn, CombatManager.EntityScriptsAllData entityScriptsData)
    {
        //Move the turn
        currentTurnCount -= prevTurn;
        currentTurnCount = Mathf.Max(0, currentTurnCount);
        int crntIdx = currentTurnCount - 1;
        Debug.Log($"CurrentIndex {crntIdx}");

        //Load entities
        Turn pastTurn = turnData[crntIdx];
        Debug.Log($"PlayerState {pastTurn.playerState.currentHealth}");

        //Change state
        PlayerScript playerScript = entityScriptsData.playerScript;
        List<EnemyScript> enemyScripts = entityScriptsData.enemyScripts;

        //Set player state
        playerScript.currentState = pastTurn.playerState.Clone();
        playerScript.healthScript.SetHealth(pastTurn.playerState.currentHealth);
        playerScript.SetMana(pastTurn.playerState.currentMana);

        //Set enemies state
        foreach (EntityState pastEnemyState in pastTurn.enemiesState) 
        { 
            EnemyScript settingEnemy = enemyScripts.Find(e => e.currentState.entityName == pastEnemyState.entityName);
            if (settingEnemy != null)
            {
                settingEnemy.currentState = pastEnemyState.Clone();
                settingEnemy.healthScript.SetHealth(pastEnemyState.currentHealth);
            }
        }

        //for (int i = 0; i < pastTurn.enemiesState.Count; i++)
        //{
        //    //TODO: FIX THIS, BASED ON ID/SLOT
        //    EnemyScript settingEnemy = enemyScripts.ElementAtOrDefault(i);
        //    if (settingEnemy != null)
        //    {
        //        settingEnemy.currentState = pastTurn.enemiesState[i].Clone();
        //        settingEnemy.healthScript.SetHealth(pastTurn.enemiesState[i].currentHealth);
        //    }
        //}

        //Delete the array after
        turnData.RemoveRange(crntIdx + 1, turnData.Count - (crntIdx + 1));

        //Set the turn UI
        updateTurnUI();
    }

    //TODO: Create better turn UI
    void updateTurnUI()
    {
        UIManager.Instance.GetComponent<UIManager>().SetTurnText(currentTurnCount);
    }
}
