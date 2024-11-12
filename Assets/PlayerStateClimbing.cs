using CubeGuns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerStateClimbing : PlayerState
{
    [SerializeField] float climbSpeed = 0.25f;

    Rigidbody rb => Source.Movement.RB;
    PlayerInput input => Source.Input;

    RaycastHit hit;

    Vector3 upVector;

    public override void OnStateEnter(State<PlayerStates.StateType> previousState)
    {
        rb.isKinematic = true;

        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit);
        var transformHorValue = Vector3.Dot(hit.transform.position, hit.transform.right) * hit.transform.right;
        var hitHorValue = hit.point.CollapseDirection(hit.transform.right);
        rb.position = hitHorValue + transformHorValue + hit.normal;

        upVector = hit.transform.up;
    }

    public override void OnStateFixedUpdate()
    {
        if (input.MovementInput.magnitude == 0) return;

        rb.position += input.MovementInput.y * upVector * climbSpeed;
    }
}
