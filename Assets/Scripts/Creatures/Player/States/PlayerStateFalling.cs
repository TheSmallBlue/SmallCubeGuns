using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerStateFalling : PlayerState
{
    #region Serialized Variables

    [Header("Fall")]
    [SerializeField] float fallGravityMultiplier;

    [Header("Coyote time")]
    [SerializeField] float coyoteTimeLength;

    #endregion

    #region Shortcuts & Helpers

    CharacterMovement Movement => Source.Movement;

    bool InCoyoteTime => Time.time - lastGroundedTime < coyoteTimeLength && coyoteTimeLength >= 0;

    #endregion

    #region Private Variables

    float lastGroundedTime = -1f;

    #endregion

    public override void OnStateEnter(State<PlayerStates.StateType> previousState)
    {
        if(previousState.Name == "PlayerGrounded") 
        {
            lastGroundedTime = Time.time;
        }

        Movement.GravityMultiplier = fallGravityMultiplier;
    }

    public override void OnStateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && InCoyoteTime) 
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Jumping);
            lastGroundedTime = -1f;
        }
    }

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(Source.Input.GetCameraBasedForward(), Source.Movement.GetAppropiateMovementSetting());

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
