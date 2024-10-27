using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerStates))]
public class Player : Creature
{
    public static Player Instance;

    // ---
    
    public PlayerInput Input => this.GetComponentIfVarNull(ref input);
    PlayerInput input;

    public PlayerStates States => this.GetComponentIfVarNull(ref states);
    PlayerStates states;

    private void Awake() 
    {
        Instance = this;
    }
}
