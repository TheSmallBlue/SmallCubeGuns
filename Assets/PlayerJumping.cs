using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : PlayerState
{
    [Header("Air movement")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration, decceleration;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float startHoverThreshold, endHoverThreshold, hoverGravityMultiplier;

    PlayerMovement Movement => Source.Movement;
    Rigidbody RB => Movement.RB;

    JumpPhases currentPhase;

    public override void OnStateEnter()
    {
        currentPhase = JumpPhases.Jump;
        RB.velocity = Vector3.zero;

        RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public override void OnStateFixedUpdate()
    {
        Movement.Move(maxSpeed, acceleration, decceleration);
        
        switch (currentPhase)
        {
            case JumpPhases.Jump:
                StartPhase();
                break;
            case JumpPhases.Hover:
                MiddlePhase();
                break;
            case JumpPhases.Fall:
                EndPhase();
                break;
        }
    }

    void StartPhase()
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

    void EndPhase()
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
