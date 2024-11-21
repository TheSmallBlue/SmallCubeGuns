using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorHelpers
{
    /// <summary>
    /// Takes an angle and returns a forward vector facing that angle.
    /// </summary>
    /// <param name="angle"> The angle, from -180 to 180</param>
    /// <returns></returns>
    public static Vector3 AngleToForward(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
