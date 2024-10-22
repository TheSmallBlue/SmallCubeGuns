using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equippable : MonoBehaviour
{
    public abstract void Equip(EquipmentHolder source);

    public abstract void Use();

    public virtual void AltUse() { }
    
    public abstract void UnEquip();
}
