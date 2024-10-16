using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerGrounded : PlayerState
{

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(InputHelpers.GetInputForward(Camera.main, "Horizontal", "Vertical"), Source.Movement.GetAppropiateMovementSetting());
    }

    public override void OnStateUpdate()
    {
        if(!Source.Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Falling);
        }
    }
}
