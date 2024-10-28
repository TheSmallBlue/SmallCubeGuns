using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorGunModule : GunModule
{
    [SerializeField] DamageEventTypes type;
    public override Fireable Apply(Fireable fireable)
    {
        switch (type)
        {
            case DamageEventTypes.Fire:
                break;
            case DamageEventTypes.Ice:
                break;
            case DamageEventTypes.Poison:
                break;
            case DamageEventTypes.Shock:
                break;
        }

        return fireable;
    }
}

enum DamageEventTypes
{
    Fire,
    Ice,
    Poison,
    Shock
}
