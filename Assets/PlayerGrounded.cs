using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerState
{
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration, decceleration;

    public override void OnStateFixedUpdate()
    {
        Source.Movement.Move(maxSpeed, acceleration, decceleration);
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
