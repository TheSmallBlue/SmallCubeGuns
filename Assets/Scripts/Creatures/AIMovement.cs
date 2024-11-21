using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : CreatureMovement
{
    [Header("Wall Avoidance")]
    [SerializeField] float avoidanceChecksHeight = 0f;
    [SerializeField] float avoidanceChecksLength = 1f;
    [SerializeField] float avoidanceChecksDistance = 0.25f;
    [SerializeField] float avoidanceForce = 10f;


    public Vector3 WallAvoidance()
    {
        Vector3 rayStart = transform.position + transform.up * avoidanceChecksHeight;

        Vector3 rayPos = rayStart - transform.right * avoidanceChecksDistance * 0.5f;
        bool leftRay = Physics.Raycast(rayPos, transform.forward, avoidanceChecksLength);

        rayPos = rayStart + transform.right * avoidanceChecksDistance * 0.5f;
        bool rightRay = Physics.Raycast(rayPos, transform.forward, avoidanceChecksLength);

        Vector3 dir = Vector3.zero;
        if(leftRay && !rightRay)
        {
            dir = transform.right;
        } else if(rightRay && !leftRay)
        {
            dir = -transform.right;
        }

        return dir * avoidanceForce;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Vector3 rayStart = transform.position + transform.up * avoidanceChecksHeight;

        Vector3 rayPos = rayStart - transform.right * avoidanceChecksDistance * 0.5f;
        Gizmos.DrawRay(rayPos, transform.forward * avoidanceChecksLength);
        rayPos = rayStart + transform.right * avoidanceChecksDistance * 0.5f;
        Gizmos.DrawRay(rayPos, transform.forward * avoidanceChecksLength);
    }
}
