using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CubeGuns.FSM;

[RequireComponent(typeof(PlayerMovement)), RequireComponent(typeof(PlayerInput))]
public class PlayerStateJumping : PlayerState
{
    #region Serialized Variables

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float startHoverThreshold, endHoverThreshold, hoverGravityMultiplier;

    [Header("Jump Buffering")]
    [SerializeField] float jumpBufferTimeLength;

    #endregion

    #region Shortcuts & Helpers

    PlayerMovement movement => (PlayerMovement)Source.Movement;
    PlayerInput input => Source.Input;
    Rigidbody rb => movement.RB;

    bool IsJumpBuffered => Time.time - lastJumpPressTime < jumpBufferTimeLength;

    #endregion

    #region Private Variables

    float lastJumpPressTime = -1f;

    JumpPhases currentPhase;

    #endregion

    private void Update() 
    {
        // Jump Buffering
        if (input.IsButtonDown("Jump") && !movement.IsGrounded && lastJumpPressTime < 0)
            lastJumpPressTime = Time.time;
        else if(!IsJumpBuffered)
            lastJumpPressTime = -1f;

        // Jump check
        if((input.IsButtonDown("Jump") || IsJumpBuffered) && movement.IsGrounded)
            SourceFSM.ChangeState(PlayerStates.StateType.Jumping);

    }

    public override void OnStateEnter(State<PlayerStates.StateType> previousState)
    {
        currentPhase = JumpPhases.Jump;
        rb.velocity = rb.velocity.CollapseAxis(VectorAxis.Y);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override void OnStateFixedUpdate()
    {
        movement.HorizontalMovement(input.GetCameraBasedForward(), movement.GetAppropiateMovementSetting());

        switch (currentPhase)
        {
            case JumpPhases.Jump:
                StarterPhase();
                break;
            case JumpPhases.Hover:
                MiddlePhase();
                break;
            case JumpPhases.Fall:
                EndingPhase();
                break;
        }
    }

    void StarterPhase()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            currentPhase = JumpPhases.Fall;
            return;
        }

        if(rb.velocity.y < startHoverThreshold)
        {
            currentPhase = JumpPhases.Hover;
            return;
        }
    }

    void MiddlePhase()
    {
        movement.GravityMultiplier = hoverGravityMultiplier;

        if (!Input.GetKey(KeyCode.Space))
        {
            currentPhase = JumpPhases.Fall;
            return;
        }

        if(rb.velocity.y < endHoverThreshold)
        {
            currentPhase = JumpPhases.Fall;
            return;
        }
    }

    void EndingPhase()
    {
        SourceFSM.ChangeState(PlayerStates.StateType.Falling);
    }

    public override void OnStateExit(PlayerStates.StateType nextState)
    {
        movement.GravityMultiplier = 1;
    }

    enum JumpPhases
    {
        Jump,
        Hover,
        Fall
    }
}
