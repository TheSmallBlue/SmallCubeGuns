using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeGuns.Sight
{
    public static class SightHelpers
    {
        public static bool InLineOfSight(Vector3 source, Vector3 target, LayerMask obstacleMask)
        {
            var dirToTarget = target - source;

            return !Physics.Raycast(source, dirToTarget.normalized, dirToTarget.magnitude, obstacleMask);
        }

        public static bool InFieldOfView(Transform source, Vector3 target, float viewRadius, float viewAngle, LayerMask obstacleMask)
        {
            var dirToTarget = target - source.position;
            if(!InLineOfSight(source.position, target, obstacleMask)) return false;
            if(dirToTarget.magnitude > viewRadius) return false;

            return Vector3.Angle(source.forward, dirToTarget) < viewAngle / 2f;
        }
    }
}

