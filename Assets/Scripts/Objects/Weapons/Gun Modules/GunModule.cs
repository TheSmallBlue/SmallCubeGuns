using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component for a ModularGun.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class GunModule : MonoBehaviour, IPlayerInteractable
{
    public Rigidbody RB => this.GetComponentIfVarNull(ref rb);
    Rigidbody rb;

    public void Interact(Player source)
    {
        if(source.Equipment.HeldObject is not ModularGunBase modularGun) return;
        if(!modularGun.AddModule(this)) return;

        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.gameObject.layer = 20;
        }

        RB.isKinematic = true;
    }

    /// <summary>
    /// Applies the component's modifications and outputs a modified component
    /// </summary>
    /// <param name="fireable"> The object that the component should modify. </param>
    /// <returns></returns>
    public abstract Fireable Apply(Fireable fireable);
}
