using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerStates)), RequireComponent(typeof(PlayerInteraction))]
public class Player : Creature
{
    public static Player Instance;

    // ---
    
    public PlayerInput Input => input;
    [RequireAndAssignComponent, SerializeField] PlayerInput input;

    public PlayerStates States => states;
    [RequireAndAssignComponent, SerializeField] PlayerStates states;

    public PlayerInteraction Interaction => interaction;
    [RequireAndAssignComponent, SerializeField] PlayerInteraction interaction;

    private void Awake() 
    {
        Instance = this;
    }
}
