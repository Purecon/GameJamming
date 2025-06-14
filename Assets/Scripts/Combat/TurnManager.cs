using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turn
{
    public int id;
} 

public class TurnManager : MonoBehaviour
{
    [Header("Turn Dictionary")]
    public Turn[] turnData;
}
