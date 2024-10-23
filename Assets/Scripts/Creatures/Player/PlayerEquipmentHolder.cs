using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerEquipmentHolder : EquipmentHolder, IPlayerComponent
{
    [SerializeField] float pickupRaylength = 2f;

    [Header("Weapon visuals")]
    [SerializeField] float swayPosStep = 0.01f;
    [SerializeField] float swayRotStep = 4f;
    [SerializeField] float maxPosStepDistance = 5f;
    [SerializeField] float maxRotStepDistance = 0.06f;
    [SerializeField] float smoothPos = 10f;
    [SerializeField] float smoothRot = 12f;

    PlayerInput input;

    protected override void Awake()
    {
        base.Awake();

        input = GetComponent<PlayerInput>();
        input.SubscribeToButton("Drop", Drop);
        input.SubscribeToButton("Use", GetEquippable);
    }

    void GetEquippable()
    {
        if (HeldObject && HeldObject.Size == ObjectSize.Large) Drop();
        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, pickupRaylength)) return;
        if (!hit.transform.root.TryGetComponent(out Equippable equippable)) return;

        if (HeldObject) 
        { 
            Drop(); 
        }

        PickUp(equippable);
    }

    public override void PickUp(Equippable pickup)
    {
        base.PickUp(pickup);

        if (pickup.Size == ObjectSize.Small)
        {
            foreach (var renderer in pickup.GetComponentsInChildren<Renderer>())
            {
                renderer.gameObject.layer = 20;
            }
        }
    }

    public override void Drop()
    {
        foreach (var renderer in HeldObject.GetComponentsInChildren<Renderer>())
        {
            renderer.gameObject.layer = 11;
        }
        
        base.Drop();
    }

    private void Update() 
    {
        if(!HeldObject) return;

        if(input.IsButtonHeld("Fire"))
        {
            UseObject();
        }

        if(HeldObject.Size == ObjectSize.Small)
        {
            // Weapon Sway
            Vector3 invertLook = input.LookInput * -swayPosStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxPosStepDistance, maxPosStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxPosStepDistance, maxPosStepDistance);

            var swayPos = invertLook;

            invertLook = input.LookInput * -swayRotStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxRotStepDistance, maxRotStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxRotStepDistance, maxRotStepDistance);

            var swayRotEuler = Quaternion.Euler(new Vector3(invertLook.y, invertLook.x, invertLook.x));

            HeldObject.transform.localPosition = Vector3.Lerp(HeldObject.transform.localPosition, swayPos, Time.deltaTime * smoothPos);
            HeldObject.transform.localRotation = Quaternion.Slerp(HeldObject.transform.localRotation, swayRotEuler, Time.deltaTime * smoothRot);
        }
    }
}
