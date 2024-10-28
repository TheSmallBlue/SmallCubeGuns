using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used in Fireables as a way to apply the Strategy coding pattern.
/// </summary>
public interface IDamageEvent
{
    public void OnDamage(Health target);
}
