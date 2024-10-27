using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerEquipmentHolder)), RequireComponent(typeof(PlayerStates))]
public class Player : MonoBehaviour
{
    public static Player Instance;

    public PlayerInput Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerEquipmentHolder Equipment { get; private set; }
    public PlayerStates States { get; private set; }

    private void Awake() 
    {
        Instance = this;

        Input = GetComponent<PlayerInput>();
        Movement = GetComponent<PlayerMovement>();
        Equipment = GetComponent<PlayerEquipmentHolder>();
        States = GetComponent<PlayerStates>();
    }
}
