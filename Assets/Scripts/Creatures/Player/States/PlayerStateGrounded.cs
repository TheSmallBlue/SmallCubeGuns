using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerStateGrounded : PlayerState
{

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(Source.Input.GetCameraBasedForward(), Source.Movement.GetAppropiateMovementSetting());
    }

    public override void OnStateUpdate()
    {
        if(!Source.Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Falling);
        }
    }
}
