using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An equippable object that has a different holding position than a base equippable.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Weapon : Equippable
{
    protected Rigidbody RB => this.GetComponentIfVarNull(ref rb);
    Rigidbody rb;

    public override void Equip(EquipmentHolder source)
    {
        base.Equip(source);

        RB.isKinematic = true;

        foreach (var collider in GetComponents<Collider>())
        {
            collider.isTrigger = true;
        }

        transform.parent = Holder;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public override void UnEquip()
    {
        base.UnEquip();

        transform.parent = null;

        RB.isKinematic = false;

        foreach (var collider in GetComponents<Collider>())
        {
            collider.isTrigger = false;
        }
    }

    public override void Use(Vector3 direction)
    {
        Fire(direction);
    }

    public override void AltUse(Vector3 direction)
    {
        AltFire(direction);
    }

    protected abstract void Fire(Vector3 direction);

    protected virtual void AltFire(Vector3 direction) { }
}
