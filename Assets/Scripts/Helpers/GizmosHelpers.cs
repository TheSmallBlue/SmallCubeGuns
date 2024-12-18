using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosHelpers
{
    /// <summary>
    /// Draws a gizmo capsule
    /// </summary>
    /// <param name="point1"> Top point </param>
    /// <param name="point2"> Bottom Point </param>
    /// <param name="radius"> Capsule raidius </param>
    public static void DrawWireCapsule(Vector3 point1, Vector3 point2, float radius)
    {
        // Spheres
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);

        var points = new Vector3[8]
        {
            // Forward line
            point1 + Vector3.forward * radius,
            point2 + Vector3.forward * radius,
            // Back line
            point1 - Vector3.forward * radius,
            point2 - Vector3.forward * radius,
            // Right line
            point1 + Vector3.right * radius,
            point2 + Vector3.right * radius,
            // Left line
            point1 - Vector3.right * radius,
            point2 - Vector3.right * radius,
        };

        Gizmos.DrawLineList(points);
    }
}
