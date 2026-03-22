using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyScript : EntityScript
{
    //Targeting
    [Header("Enemy target")]
    public bool targeted = false;
    public GameObject targetedUI;

    //Set targeted
    public void SetTargeted(bool isTargeted)
    {
        targeted = isTargeted;
        targetedUI.SetActive(targeted);
    }

    //On click
    void OnMouseOver()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Whatever you want it to do.
            Debug.Log("Left-clicked while hovering!");
            CombatManager.Instance.GetComponent<CombatManager>().SetEnemyTarget(this);
        }
    }
}
