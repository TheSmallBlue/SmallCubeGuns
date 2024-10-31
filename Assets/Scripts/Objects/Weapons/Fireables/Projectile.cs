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

    Transform sourceRoot;

    Vector3 direction;

    public override void Create(EquipmentHolder source, Vector3 position, Vector3 direction)
    {
        transform.position = position;
        this.direction = direction;

        sourceRoot = source.transform.root;
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
        if(other.transform.root == sourceRoot) return;
        if(other.transform.root.TryGetComponent(out Health health))
            Damage(health);
        
        OnHit();
    }
}
