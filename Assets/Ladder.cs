using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IPlayerInteractable
{
    public void Interact(Player source)
    {
        source.States.ChangeState(PlayerStates.StateType.Climbing);
    }
}
