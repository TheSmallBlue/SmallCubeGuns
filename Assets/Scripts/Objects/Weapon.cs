using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : Equippable
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Use()
    {
        Debug.Log("Fire");
    }

    public override void Equip(EquipmentHolder source)
    {
        base.Equip(source);

        rb.isKinematic = true;
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        transform.parent = Holder;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

    public override void UnEquip()
    {
        base.UnEquip();

        rb.isKinematic = false;
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }
    }
}
