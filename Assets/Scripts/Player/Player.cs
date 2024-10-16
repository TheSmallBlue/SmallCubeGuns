using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerStates))]
public class Player : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }
    public PlayerStates States { get; private set;}

    private void Awake() 
    {
        Movement = GetComponent<PlayerMovement>();
        States = GetComponent<PlayerStates>();
    }
}
