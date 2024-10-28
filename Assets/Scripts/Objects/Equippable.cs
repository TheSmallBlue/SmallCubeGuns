using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that defines something can be equipped / picked up.
/// </summary>
public abstract class Equippable : MonoBehaviour, IPlayerInteractable
{
    public ObjectSize Size;
    
    public bool Equipped => Source != null;
    public Transform Holder => Size == ObjectSize.Small ? Source.smallObjectHolder : Source.largeObjectHolder;

    protected EquipmentHolder Source;

    public virtual void Equip(EquipmentHolder source)
    {
        Source = source;
    }

    public virtual void UnEquip()
    {
        Source = null;
    }

    public void Interact(Player source)
    {
        source.Equipment.PickUp(this);
    }

    public abstract void Use(Vector3 direction);

    public virtual void AltUse(Vector3 direction) { }
}

public enum ObjectSize
{
    Small,
    Large
}