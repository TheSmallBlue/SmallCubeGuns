using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gun module that instantiates an item.
/// </summary>
public class ConstructorGunModule : GunModule
{
    [SerializeField] Fireable fireableToConstruct;

    public override Fireable Apply(Fireable fireable)
    {
        if(fireable != null) Destroy(fireable);

        // TODO: Use factory to instantiate objects instead for less frame drops.
        return Instantiate(fireableToConstruct);
    }
}
