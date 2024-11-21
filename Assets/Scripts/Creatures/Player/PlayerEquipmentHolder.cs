using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerEquipmentHolder : EquipmentHolder
{
    PlayerInput input;

    protected override void Awake()
    {
        base.Awake();

        input = GetComponent<PlayerInput>();
        input.SubscribeToButton("Drop", Drop);
    }


    public override void PickUp(Equippable pickup)
    {
        base.PickUp(pickup);

        if (pickup.Size == ObjectType.Holdable)
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
            gameObject.RunEvent<PlayerVisuals>((v) => v.OnShoot());
            HeldObject.Use(Camera.main.transform.forward);
        }

        if (input.IsButtonDown("AltFire"))
        {
            HeldObject.AltUse(Camera.main.transform.forward);
        }
    }
}
