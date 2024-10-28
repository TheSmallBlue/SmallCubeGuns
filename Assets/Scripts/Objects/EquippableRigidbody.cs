using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of Rigidbody that an EquipmentHolder can pick up.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EquippableRigidbody : Equippable
{
    Rigidbody rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Equip(EquipmentHolder source)
    {
        base.Equip(source);

        rb.excludeLayers = 1 << Source.gameObject.layer;
        rb.useGravity = false;
    }

    public override void UnEquip()
    {
        base.UnEquip();

        rb.excludeLayers = 0;
        rb.useGravity = true;
    }

    public override void Use(Vector3 direction)
    {
        Vector3 throwDir = direction;

        Source.Drop();
        rb.AddForce(throwDir * 500);
    }

    private void FixedUpdate() 
    {
        if (!Equipped) return;

        rb.velocity = (Holder.position - transform.position) * 10f;
    }
}
