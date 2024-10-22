using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerStateGrounded : PlayerState
{

    public override void OnStateFixedUpdate()
    {
        Source.GetPlayerComponent<PlayerMovement>().HorizontalMovement(Source.GetPlayerComponent<PlayerInput>().GetCameraBasedForward(), Source.GetPlayerComponent<PlayerMovement>().GetAppropiateMovementSetting());
    }

    public override void OnStateUpdate()
    {
        if(!Source.GetPlayerComponent<PlayerMovement>().IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Falling);
        }
    }
}
