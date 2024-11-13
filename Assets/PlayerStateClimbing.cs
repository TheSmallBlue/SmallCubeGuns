using CubeGuns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerStateClimbing : PlayerState
{
    [SerializeField] float climbSpeed = 0.1f;

    Rigidbody rb => Source.Movement.RB;
    PlayerInput input => Source.Input;

    float timeClimbing;

    RaycastHit hit;

    Vector3 intendedPosition, upVector;
    Vector3 velocity;
    Vector3 dirToWall;

    bool isAgainstWall => Physics.Raycast(rb.transform.position, dirToWall);

    public override void OnStateEnter(State<PlayerStates.StateType> previousState)
    {
        timeClimbing = 0f;
        
        rb.isKinematic = true;

        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit);

        var transformHorValue = Vector3.Dot(hit.transform.position, hit.transform.right) * hit.transform.right;
        var hitHorValue = hit.point.CollapseDirection(hit.transform.right);
        intendedPosition = hitHorValue + transformHorValue + hit.normal * 1.1f;

        upVector = hit.transform.up;
        dirToWall = -hit.normal;
    }

    public override void OnStateFixedUpdate()
    {
        timeClimbing += Time.fixedDeltaTime;

        intendedPosition += input.MovementInput.y * upVector * climbSpeed;
        rb.transform.position = Vector3.SmoothDamp(rb.transform.position, intendedPosition, ref velocity, 0.25f);

        Debug.Log(isAgainstWall);

        if(timeClimbing > 0.5f && Source.Movement.IsGrounded)
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Grounded);
            return;
        }

        if(!isAgainstWall || Input.GetButton("Jump"))
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Jumping);
            return;
        }
    }

    public override void OnStateExit(PlayerStates.StateType nextInput)
    {
        rb.isKinematic = false;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(intendedPosition, 0.5f);
    }
}
