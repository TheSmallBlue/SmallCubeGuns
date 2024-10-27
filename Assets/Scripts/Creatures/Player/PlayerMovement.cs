using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : CharacterMovement, IPlayerComponent
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

    Player player;

    private void Awake() 
    {
        player = GetComponent<Player>();
    }
    
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
            else
                return walkingMovement;
            
        } else return airMovement;
    }
}
