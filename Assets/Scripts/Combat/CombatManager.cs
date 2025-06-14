using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : Singleton<CombatManager>
{
    //Enemy 
    [Header("Enemy")]
    //public Enemy enemy;
    public GameObject enemyGameObject;
    //Health bar

    //Player
    [Header("Player")]
    public GameObject playerGameObject;
}
