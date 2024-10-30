using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A type of Rigidbody that an EquipmentHolder can pick up.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EquippableRigidbody : Equippable
{
    public Rigidbody RB => this.GetComponentIfVarNull(ref rb);
    Rigidbody rb;

    public override void Equip(EquipmentHolder source)
    {
        base.Equip(source);

        RB.excludeLayers = 1 << Source.gameObject.layer;
        RB.useGravity = false;
    }

    public override void UnEquip()
    {
        base.UnEquip();

        RB.excludeLayers = 0;
        RB.useGravity = true;
    }

    public override void Use(Vector3 direction)
    {
        Vector3 throwDir = direction;

        Source.Drop();
        RB.AddForce(throwDir * 500);
    }

    private void FixedUpdate() 
    {
        if (!Equipped) return;

        RB.velocity = (Holder.position - transform.position) * 10f;
    }
}
