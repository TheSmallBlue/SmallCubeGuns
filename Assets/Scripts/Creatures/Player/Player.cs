using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerEquipmentHolder)), RequireComponent(typeof(PlayerStates))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    // ---
    
    public PlayerInput Input => this.GetComponentIfVarNull(ref input);
    PlayerInput input;

    public PlayerMovement Movement => this.GetComponentIfVarNull(ref movement);
    PlayerMovement movement;

    public PlayerEquipmentHolder Equipment => this.GetComponentIfVarNull(ref equipment);
    PlayerEquipmentHolder equipment;

    public PlayerStates States => this.GetComponentIfVarNull(ref states);
    PlayerStates states;

    private void Awake() 
    {
        Instance = this;
    }
}
