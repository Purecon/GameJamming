using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Game/Entity Data")]
public class EntityData : ScriptableObject
{
    public EntityState state;
}
