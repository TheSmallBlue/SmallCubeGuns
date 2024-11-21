using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CubeGuns.Sight;
using UnityEngine;

public class Sight : MonoBehaviour
{
    #region Serialized Variables

    [SerializeField] LayerMask sightMask, obstacleMask;

    [Space]
    [SerializeField] float sightRadius = 10f;
    [SerializeField] float sightAngle = 75f;
    [SerializeField] float sightMemory = 10f;

    #endregion

    #region Private Variables

    public Dictionary<Transform, float> ThingsInSight { get; private set; } = new();

    #endregion

    public bool CanSeeThing(Transform thing) => ThingsInSight.ContainsKey(thing);

    private void FixedUpdate() 
    {
        var seeableObjects = Physics.OverlapSphere(transform.position, sightRadius, sightMask);

        foreach (var seeableObject in seeableObjects)
        {
            if(seeableObject.transform == transform) continue;

            if(!SightHelpers.InFieldOfView(transform, seeableObject.transform.position, sightRadius, sightAngle, obstacleMask)) continue;

            if(ThingsInSight.ContainsKey(seeableObject.transform))
            {
                ThingsInSight[seeableObject.transform] = Time.time;
            } else 
            {
                ThingsInSight.Add(seeableObject.transform, Time.time);
            }
        }
    }

    private void Update() 
    {
        //Debug.Log(string.Join(", ", ThingsInSight));

        if(ThingsInSight.Count == 0) return;

        var oldestThingSeen = ThingsInSight.OrderByDescending(x => Time.time - x.Value).First();

        if(Time.time - oldestThingSeen.Value > sightMemory)
        {
            ThingsInSight.Remove(oldestThingSeen.Key);
        }
    }

    public struct SeenObject
    {
        public Transform transform;
        public float lastSeenTime;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRadius);

        Gizmos.DrawRay(transform.position, VectorHelpers.AngleToForward(transform.eulerAngles.y + sightAngle * 0.5f) * sightRadius);
        Gizmos.DrawRay(transform.position, VectorHelpers.AngleToForward(transform.eulerAngles.y - sightAngle * 0.5f) * sightRadius);
    }
}
