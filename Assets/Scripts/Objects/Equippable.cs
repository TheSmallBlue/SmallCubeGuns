using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equippable : MonoBehaviour
{
    public bool Equipped => Source != null;

    protected EquipmentHolder Source;

    public virtual void Equip(EquipmentHolder source)
    {
        Source = source;
    }

    public virtual void UnEquip()
    {
        Source = null;
    }

    public abstract void Use();

    public virtual void AltUse() { }
}
