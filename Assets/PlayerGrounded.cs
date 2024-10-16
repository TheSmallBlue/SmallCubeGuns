using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    [SerializeField] PlayerMovement.HorizontalMovementSettings horizontalMovementSettings;

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(InputHelpers.GetInputForward(Camera.main, "Horizontal", "Vertical"), horizontalMovementSettings);
    }

    public override void OnStateUpdate()
    {
        if (Source.Movement.IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Jumping);
        }

        if(!Source.Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Falling);
        }
    }
}
