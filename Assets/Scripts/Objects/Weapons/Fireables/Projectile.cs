using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A fireable rigidbody.
/// </summary>
[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Collider))]
public class Projectile : Fireable
{
    protected Rigidbody RB => this.GetComponentIfVarNull(ref rb);
    Rigidbody rb;

    [SerializeField] float speed, lifeLength = 5f;

    Vector3 direction;

    public override void Create(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        this.direction = direction;
    }

    private void FixedUpdate() 
    {
        RB.velocity = direction * speed;

        lifeLength -= Time.fixedDeltaTime;

        if(lifeLength <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.root.TryGetComponent(out Health health))
            Damage(health);
        
        OnHit();
    }
}
