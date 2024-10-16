using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerGrounded : PlayerState
{
    [SerializeField] CharacterMovement.HorizontalMovementSettings walkSettings, dashSettings;

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(InputHelpers.GetInputForward(Camera.main, "Horizontal", "Vertical"), Input.GetKey(KeyCode.LeftShift) ? dashSettings : walkSettings);
    }

    public override void OnStateUpdate()
    {
        if(!Source.Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Falling);
        }
    }
}
