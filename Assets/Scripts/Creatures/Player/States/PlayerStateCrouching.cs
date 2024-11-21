using CubeGuns.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCrouching : PlayerState
{
    PlayerInput input => Source.Input;

    [SerializeField] CapsuleCollider capsule;

    float defaultHeight;

    public override void OnStateEnter(State<PlayerStates.StateType> previousState)
    {
        defaultHeight = capsule.height;

        capsule.height *= 0.5f;

        Source.Movement.GroundedCheck(out RaycastHit hitInfo);
        Source.Movement.RB.position = hitInfo.point + hitInfo.normal * 0.5f;
    }

    public override void OnStateFixedUpdate()
    {
        Source.Movement.HorizontalMovement(Source.Input.GetCameraBasedForward(), (Source.Movement as PlayerMovement).GetAppropiateMovementSetting());
    }

    public override void OnStateUpdate()
    {
        if(input.IsButtonUp("Crouch"))
        {
            SourceFSM.ChangeState(PlayerStates.StateType.Grounded);
            return;
        }
    }

    public override void OnStateExit(PlayerStates.StateType nextInput)
    {
        capsule.height = defaultHeight;
    }
}
