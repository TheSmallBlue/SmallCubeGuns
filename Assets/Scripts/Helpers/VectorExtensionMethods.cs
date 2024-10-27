using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensionMethods
{
    /// <summary>
    /// Sets the value of an axis to another.
    /// </summary>
    /// <param name="vector"> The vector to modify </param>
    /// <param name="axis"> The axis to change </param>
    /// <param name="number"> The value to set in the axis </param>
    /// <returns></returns>
    public static Vector3 SetAxis(this Vector3 vector, VectorAxis axis, float number)
    {
        vector[(int)axis] = number;
        return vector;
    }

    /// <summary>
    /// Adds a value to the value in a vector's axis.
    /// </summary>
    /// <param name="vector"> The vector to modify </param>
    /// <param name="axis"> The axis to change </param>
    /// <param name="number"> The value to add to the axis </param>
    /// <returns></returns>
    public static Vector3 AddToAxis(this Vector3 vector, VectorAxis axis, float number)
    {
        vector[(int)axis] += number;
        return vector;
    }

    /// <summary>
    /// Sets the value of an axis to 0.
    /// </summary>
    /// <param name="vector"> The vector to modify </param>
    /// <param name="axis"> The axis to change </param>
    /// <returns></returns>
    public static Vector3 CollapseAxis(this Vector3 vector, VectorAxis axis)
    {
        return vector.SetAxis(axis, 0);
    }
}

public enum VectorAxis
{
    X = 0,
    Y = 1,
    Z = 2
}
