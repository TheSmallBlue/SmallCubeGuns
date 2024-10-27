using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerEquipmentHolder : EquipmentHolder
{
    [SerializeField] float pickupRaylength = 2f;

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
        if (HeldObject == null) return;

        foreach (var renderer in HeldObject.GetComponentsInChildren<Renderer>())
        {
            renderer.gameObject.layer = 11;
        }
        
        base.Drop();
    }

    private void Update() 
    {
        if (HeldObject == null) return;

        if(input.IsButtonDown("Fire"))
        {
            HeldObject.Use();
        }

        if (input.IsButtonDown("AltFire"))
        {
            HeldObject.AltUse();
        }
    }
}
