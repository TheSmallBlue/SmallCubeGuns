using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    #region Public Variables

    public HorizontalMovementSettings WalkingMovement => walkingMovement;
    public HorizontalMovementSettings DashingMovement => dashingMovement;
    public HorizontalMovementSettings AirMovement => airMovement;

    #endregion

    #region Serialized Variables

    [Header("Player settings")]
    [SerializeField] HorizontalMovementSettings walkingMovement;
    [SerializeField] HorizontalMovementSettings dashingMovement;
    [SerializeField] HorizontalMovementSettings airMovement;

    #endregion
    
    private void Update() 
    {
        transform.forward = Camera.main.transform.forward.CollapseAxis(VectorAxis.Y);
    }

    public HorizontalMovementSettings GetAppropiateMovementSetting()
    {
        if(IsGrounded)
        {
            if(PlayerInput.Instance.IsButton("Sprint")) 
                return dashingMovement;
            else
                return walkingMovement;
            
        } else return airMovement;
    }
}
