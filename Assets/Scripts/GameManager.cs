using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Managers")]
    public CombatManager combatManager;
    public UIManager uiManager;

    private void Start()
    {
        Debug.Log("Game start");
        if (combatManager == null)
        {
            combatManager = CombatManager.Instance;
        }

        //Ideally use a button to trigger this, for now through script
        //combatManager.TurnCombat();
    }

    //TODO: Create Player action and Enemy action
    public void TurnAction(string playerTurnType)
    {
        //TEST
        combatManager.TurnCombat(playerTurnType);
    }
}
