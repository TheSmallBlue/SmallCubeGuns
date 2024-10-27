using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureMovement)), RequireComponent(typeof(EquipmentHolder)), RequireComponent(typeof(Health))]
public abstract class Creature : MonoBehaviour
{
    public CreatureMovement Movement => this.GetComponentIfVarNull(ref movement);
    CreatureMovement movement;

    public EquipmentHolder Equipment => this.GetComponentIfVarNull(ref equipment);
    EquipmentHolder equipment;
}
