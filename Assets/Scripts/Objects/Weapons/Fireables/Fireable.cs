using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that can be fired by a weapon.
/// </summary>
public abstract class Fireable : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected int passthroughAmount;

    Queue<IDamageEvent> damageEvents;

    public void AddEvent(IDamageEvent newEvent)
    {
        damageEvents.Enqueue(newEvent);
    }

    // This method is not meant to be overriden, instead it serves as a manager for the methods that can!
    // It must also be called by the class that inherits this.
    public void Damage(Health target)
    {
        target.Damage(damage);

        for (int i = 0; i < damageEvents.Count; i++)
        {
            damageEvents.Dequeue().OnDamage(target);
        }

        OnHit();
    }

    protected void OnHit()
    {
        passthroughAmount--;
        if (passthroughAmount < 0)
        {
            Destroy(gameObject);
        }
    }

    public abstract void Create(Vector3 position, Vector3 direction);
}
