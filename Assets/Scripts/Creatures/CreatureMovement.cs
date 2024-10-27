using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CreatureMovement : MonoBehaviour
{
    #region Public Variables

    public Rigidbody RB 
    { 
        get
        {
            if(rb == null) rb = GetComponent<Rigidbody>();

            return rb;
        }
    }
    Rigidbody rb;

    public Vector3 Gravity => gravity;
    public bool IsGrounded => GroundedCheck(out RaycastHit ignoredHit);

    [HideInInspector] public float GravityMultiplier = 1;

    #endregion

    #region Serialized Variables

    [Header("Gravity")]
    [SerializeField] Vector3 gravity;

    [Header("Floor and Wall Checks")]
    [SerializeField] LayerMask floorLayer;

    [Space]
    [SerializeField] float wallCheckHeight = 0.5f;
    [SerializeField] float wallCheckRadius = 0.4f;
    [SerializeField] float wallCheckLength = 0.2f;

    [Space]
    [SerializeField] float stepRayHeight = -0.9f;
    [SerializeField] float stepRayLowerLength = 0.6f;
    [SerializeField] float stepRayUpperLength = 0.6f;

    [Space]
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    #endregion

    protected virtual void FixedUpdate() 
    {
        RB.AddForce(gravity * GravityMultiplier);
    }

    #region Floor Checks

    /// <summary>
    /// Returns true if the player is grounded. 
    /// </summary>
    /// <param name="hitInfo">If the player is grounded, returns info about the floor beneath them.</param>
    /// <returns></returns>
    public bool GroundedCheck(out RaycastHit hitInfo)
    {
        bool grounded = Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hitInfo, 0.55f, floorLayer);

        if(!grounded) return false;
        if(hitInfo.transform.root.TryGetComponent(out Equippable equippable) && equippable.Equipped) return false;

        return true;
    }

    Vector3 stepRayLowerPos => transform.position.AddToAxis(VectorAxis.Y, stepRayHeight);
    Vector3 stepRayUpperPos => transform.position.AddToAxis(VectorAxis.Y, stepRayHeight + stepHeight);

    /// <summary>
    /// Checks if there is an obstacle towards the direction given relative to the player. Will tell you if the obstacle is a wall, are step, or if there is no obstacle.
    /// </summary>
    /// <param name="checkDirection">The direction in which to check.</param>
    /// <returns></returns>
    public ObstacleType ObstacleCheck(Vector3 checkDirection)
    {
        bool lowerCheck = Physics.Raycast(stepRayLowerPos, checkDirection, stepRayLowerLength, floorLayer);
        bool higherCheck = Physics.Raycast(stepRayUpperPos, checkDirection, stepRayUpperLength, floorLayer);

        if (lowerCheck && !higherCheck) return ObstacleType.Step;
        if (lowerCheck && higherCheck || ObstructionCheck(checkDirection)) return ObstacleType.Wall;
        return ObstacleType.None;
    }

    /// <summary>
    /// Checks whether if there is ANY obstruction towards the desired direction.
    /// </summary>
    /// <param name="checkDirection">The direction in which to check.</param>
    /// <returns></returns>
    public bool ObstructionCheck(Vector3 checkDirection)
    {
        return Physics.CapsuleCast(transform.position.AddToAxis(VectorAxis.Y, wallCheckHeight), transform.position.AddToAxis(VectorAxis.Y, -wallCheckHeight), wallCheckRadius, checkDirection, wallCheckLength, floorLayer);
    }

    public enum ObstacleType
    {
        None,
        Step,
        Wall
    }

    #endregion

    #region Movement

    /// <summary>
    /// Move horizontally towards a direction.
    /// This method automatically applies Wall and Step checking.
    /// </summary>
    /// <param name="direction">The direction in which to move.</param>
    /// <param name="settings">Speed and acceleration settings.</param>
    public virtual void HorizontalMovement(Vector3 direction, HorizontalMovementSettings settings)
    {
        direction = direction.CollapseAxis(VectorAxis.Y);

        switch (ObstacleCheck(direction))
        {
            case ObstacleType.Step:
                RB.position += Vector3.up * stepSmooth;
                break;
            case ObstacleType.Wall:
                return;
        }

        Vector3 desiredVelocity = direction * settings.MaxSpeed;
        Vector3 velocityDif = desiredVelocity - RB.velocity.CollapseAxis(VectorAxis.Y);
        float accel = direction.magnitude > 0.01f ? settings.Acceleration : settings.Decceleration;

        RB.AddForce(velocityDif * accel);
    }

    [System.Serializable]
    public struct HorizontalMovementSettings
    {
        public float MaxSpeed;
        public float Acceleration;
        public float Decceleration;
    }

    #endregion

    #region Editor

    private void OnDrawGizmosSelected() 
    {
        // Walls
        Gizmos.color = Color.red;
        GizmosHelpers.DrawWireCapsule(transform.position.AddToAxis(VectorAxis.Y, wallCheckHeight), transform.position.AddToAxis(VectorAxis.Y, -wallCheckHeight), wallCheckRadius);

        // Steps
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(stepRayLowerPos, stepRayLowerPos + transform.forward * stepRayLowerLength);
        Gizmos.DrawLine(stepRayUpperPos, stepRayUpperPos + transform.forward * stepRayUpperLength);
    }

    #endregion
}
