using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EquipmentHolder : MonoBehaviour
{
    public Transform largeObjectHolder, smallObjectHolder;

    [Space]
    public Equippable HeldObject;

    [Space]
    [SerializeField] protected float throwPower;

    protected virtual void Awake() 
    {
        if(HeldObject != null) HeldObject.Equip(this);
    }

    public virtual void PickUp(Equippable pickup)
    {
        if(HeldObject != null) Drop();

        pickup.Equip(this);
        HeldObject = pickup;
    }

    public virtual void Drop()
    {
        if(HeldObject == null) return;

        HeldObject.UnEquip();
        HeldObject = null;
    }

    public virtual void UseObject(Vector3 direction)
    {
        if (HeldObject == null) return;

        HeldObject.Use(direction);
    }

    public virtual void AltUseObject(Vector3 direction)
    {
        if (HeldObject == null) return;

        HeldObject.AltUse(direction);
    }
}