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
    public int currentTurnCount = 1;

    //TODO: What happened in a turn
    //This is the changed of a turn and saved the data
    public void StartNewTurn(CombatManager.EntityScriptsAllData entityScriptsData)
    {
        //Get entities
        Debug.LogFormat("EntityData:{0}", entityScriptsData);

        //Save entities
        EntityState playerState = entityScriptsData.playerScript.entityData.state;
        List<EntityState> enemiesState = entityScriptsData.enemyScripts.Select(e => e.entityData.state).ToList();

        turnData.Add(new Turn(currentTurnCount, playerState, enemiesState));

        //Move the turn
        currentTurnCount++;
    }
}
