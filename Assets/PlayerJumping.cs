using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerJumping : PlayerState
{
    #region Serialized Variables

    [Header("Air movement")]
    [SerializeField] CharacterMovement.HorizontalMovementSettings horizontalMovementSettings;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float startHoverThreshold, endHoverThreshold, hoverGravityMultiplier;

    [Header("Jump Buffering")]
    [SerializeField] float jumpBufferTimeLength;

    #endregion

    #region Shortcuts & Helpers

    CharacterMovement Movement => Source.Movement;
    Rigidbody RB => Movement.RB;

    bool IsJumpBuffered => Time.time - lastJumpPressTime < jumpBufferTimeLength;

    #endregion

    #region Private Variables

    float lastJumpPressTime = -1f;

    JumpPhases currentPhase;

    #endregion

    private void Update() 
    {
        // Jump Buffering
        if (Input.GetKeyDown(KeyCode.Space) && !Movement.IsGrounded && lastJumpPressTime < 0)
            lastJumpPressTime = Time.time;
        else if(!IsJumpBuffered)
            lastJumpPressTime = -1f;

        // Jump check
        if((Input.GetKeyDown(KeyCode.Space) || IsJumpBuffered) && Movement.IsGrounded)
            SourceFSM.ChangeState(PlayerStates.StateType.Jumping);

    }

    public override void OnStateEnter(State<PlayerStates.StateType> previousState)
    {
        currentPhase = JumpPhases.Jump;
        RB.velocity = RB.velocity.CollapseAxis(VectorAxis.Y);

        RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(InputHelpers.GetInputForward(Camera.main, "Horizontal", "Vertical"), horizontalMovementSettings);

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

        if(RB.velocity.y < startHoverThreshold)
        {
            currentPhase = JumpPhases.Hover;
            return;
        }
    }

    void MiddlePhase()
    {
        Movement.GravityMultiplier = hoverGravityMultiplier;

        if (!Input.GetKey(KeyCode.Space))
        {
            currentPhase = JumpPhases.Fall;
            return;
        }

        if(RB.velocity.y < endHoverThreshold)
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
        Movement.GravityMultiplier = 1;
    }

    enum JumpPhases
    {
        Jump,
        Hover,
        Fall
    }
}
