using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : CreatureMovement
{
    #region Public Variables

    public HorizontalMovementSettings WalkingMovement => walkingMovement;
    public HorizontalMovementSettings DashingMovement => dashingMovement;
    public HorizontalMovementSettings AirMovement => airMovement;

    #endregion

    #region Serialized Variables

    [Header("Player settings")]
    [SerializeField] HorizontalMovementSettings walkingMovement;
    [SerializeField] HorizontalMovementSettings crouchingMovement;
    [SerializeField] HorizontalMovementSettings dashingMovement;
    [SerializeField] HorizontalMovementSettings airMovement;

    #endregion

    [RequireAndAssignComponent, SerializeField] Player player;
    
    private void Update() 
    {
        transform.forward = Camera.main.transform.forward.CollapseAxis(VectorAxis.Y);
    }

    public HorizontalMovementSettings GetAppropiateMovementSetting()
    {
        if(IsGrounded)
        {
            if(player.Input.IsButtonHeld("Sprint")) 
                return dashingMovement;
            else if(player.Input.IsButtonHeld("Crouch"))
                return crouchingMovement;
            else
                return walkingMovement;

        } else return airMovement;
    }
}
