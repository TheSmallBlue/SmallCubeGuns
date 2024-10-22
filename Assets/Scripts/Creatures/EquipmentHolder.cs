using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EquipmentHolder : MonoBehaviour
{
    public Transform objectHolder;

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

    public virtual void UseObject()
    {
        if (HeldObject == null) return;

        HeldObject.Use();
    }

    public virtual void AltUseObject()
    {
        if (HeldObject == null) return;

        HeldObject.AltUse();
    }
}