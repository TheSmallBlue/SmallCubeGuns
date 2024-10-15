using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : PlayerState
{
    [Header("Air movement")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration, decceleration;

    [Header("Fall")]
    [SerializeField] float fallGravityMultiplier;

    PlayerMovement Movement => Source.Movement;

    public override void OnStateEnter()
    {
        Movement.GravityMultiplier = fallGravityMultiplier;
    }

    public override void OnStateFixedUpdate()
    {
        Movement.Move(maxSpeed, acceleration, decceleration);

        if(Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Grounded);
        }
    }

    public override void OnStateExit(PlayerStates.StateType nextState)
    {
        Movement.GravityMultiplier = 1;
    }
}
