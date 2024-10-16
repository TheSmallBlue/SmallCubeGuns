using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement)), RequireComponent(typeof(PlayerStates))]
public class Player : MonoBehaviour
{
    public CharacterMovement Movement { get; private set; }
    public PlayerStates States { get; private set;}

    private void Awake() 
    {
        Movement = GetComponent<CharacterMovement>();
        States = GetComponent<PlayerStates>();
    }
}
