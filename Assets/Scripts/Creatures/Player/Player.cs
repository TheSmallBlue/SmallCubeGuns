using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)), RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerEquipmentHolder)), RequireComponent(typeof(PlayerStates))]
public class Player : MonoBehaviour
{
    public PlayerInput Input { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerEquipmentHolder Equipment { get; private set; }
    public PlayerStates States { get; private set; }

    private void Awake() 
    {
        Input = GetComponent<PlayerInput>();
        Movement = GetComponent<PlayerMovement>();
        Equipment = GetComponent<PlayerEquipmentHolder>();
        States = GetComponent<PlayerStates>();
    }
}
