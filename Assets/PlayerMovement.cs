using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Rigidbody RB { get; private set; }
    public bool IsGrounded => GroundedCheck();

    [SerializeField] Vector3 Gravity;
    [SerializeField] LayerMask floorLayer;
    
    [HideInInspector] public float GravityMultiplier = 1;

    private void Awake() 
    {
        RB = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() 
    {
        RB.AddForce(Gravity * GravityMultiplier);
    }

    public void Move(float maxSpeed, float acceleration, float decceleration)
    {
        Vector3 forward = Camera.main.GetCameraBasedForward(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), true).normalized;

        Vector3 desiredVelocity = forward * maxSpeed;
        Vector3 velocityDif = desiredVelocity - RB.velocity.CollapseAxis(VectorAxis.Y);
        float accel = forward.magnitude > 0.01f ? acceleration : decceleration;

        RB.AddForce(velocityDif * accel);
    }

    public bool GroundedCheck()
    {
        return Physics.SphereCast(transform.position, 0.5f, Vector3.down, out RaycastHit hitInfo, 0.55f, floorLayer);
    }
}
