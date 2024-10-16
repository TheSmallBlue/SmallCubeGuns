using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : PlayerState
{
    [Header("Air movement")]
    [SerializeField] PlayerMovement.HorizontalMovementSettings horizontalMovementSettings;

    [Header("Fall")]
    [SerializeField] float fallGravityMultiplier;

    PlayerMovement Movement => Source.Movement;

    public override void OnStateEnter()
    {
        Movement.GravityMultiplier = fallGravityMultiplier;
    }

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(InputHelpers.GetInputForward(Camera.main, "Horizontal", "Vertical"), horizontalMovementSettings);

        if (Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Grounded);
        }
    }

    public override void OnStateExit(PlayerStates.StateType nextState)
    {
        Movement.GravityMultiplier = 1;
    }
}
