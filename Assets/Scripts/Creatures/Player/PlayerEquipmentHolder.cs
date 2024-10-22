using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerEquipmentHolder : EquipmentHolder
{
    [SerializeField] float pickupRaylength = 2f;
    protected override void Awake()
    {
        base.Awake();

        var input = GetComponent<PlayerInput>();
        input.SubscribeToButton("Fire", UseObject);
        input.SubscribeToButton("Drop", Drop);
        input.SubscribeToButton("Use", GetEquippable);
    }

    void GetEquippable()
    {
        if(HeldObject) { Drop(); return; }
        if(!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, pickupRaylength)) return;
        if(!hit.collider.TryGetComponent(out Equippable equippable)) return;

        PickUp(equippable);
    }
}
