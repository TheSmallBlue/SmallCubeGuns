using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerStateGrounded : PlayerState
{

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(Source.Input.GetCameraBasedForward(), (Source.Movement as PlayerMovement).GetAppropiateMovementSetting());
    }

    public override void OnStateUpdate()
    {
        if(!Source.Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Falling);
        }
    }
}
