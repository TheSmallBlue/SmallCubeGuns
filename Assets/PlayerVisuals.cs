using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [RequireAndAssignComponent, SerializeField] PlayerInput input;
    [RequireAndAssignComponent, SerializeField] PlayerMovement movement;
    [RequireAndAssignComponent, SerializeField] PlayerEquipmentHolder equipment;

    // Editor

    [SerializeField] Animator armsAnimator;

    [Space]
    [SerializeField] float movementSmoothTime = 0.1f;

    [Space]
    [SerializeField] float gunSwingTime = 0.1f;

    Vector2 gunSwing;
    Vector2 gunSwingVelocity;

    float movementProgress;
    float movementVelocity;

    private void Update() 
    {
        // Walk/Sprint
        var currentVelocity = movement.RB.velocity.CollapseAxis(VectorAxis.Y);
        var desiredVelocity = input.GetCameraBasedForward() * movement.WalkingMovement.MaxSpeed;
        movementProgress = Mathf.SmoothDamp(movementProgress, input.GetCameraBasedForward().magnitude == 0 ? 0 : VectorExtensionMethods.InverseLerp(Vector3.zero, desiredVelocity, currentVelocity), ref movementVelocity, movementSmoothTime);
        armsAnimator.SetFloat("XVelocityProgress", movementProgress);

        // Weapon
        armsAnimator.SetBool("WeaponHeld", equipment.HeldObject && equipment.HeldObject.Size == ObjectType.Holdable);


        // Swing
        var lookinput = input.LookInput.normalized;
        gunSwing = Vector2.SmoothDamp(gunSwing, lookinput, ref gunSwingVelocity, gunSwingTime);
        armsAnimator.SetFloat("SwingVelocityX", gunSwing.x);
        armsAnimator.SetFloat("SwingVelocityY", gunSwing.y);
    }
}
