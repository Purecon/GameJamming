using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Managers")]
    public CombatManager combatManager;

    private void Start()
    {
        Debug.Log("Game start");
        if (combatManager == null)
        {
            combatManager = CombatManager.Instance;
        }

        //Start Turn one TEST
        combatManager.TurnManager.StartNewTurn(combatManager.GetEntityScripts());
    }
}
