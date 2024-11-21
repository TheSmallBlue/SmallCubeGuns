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

    /// <summary>
    /// Returns the progress of point "value" between "a" and "b".
    /// </summary>
    /// <param name="a">From</param>
    /// <param name="b">To</param>
    /// <param name="value">Point to calculate</param>
    /// <returns></returns>
    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    /// <summary>
    /// Collapses the direction of a vector.
    /// </summary>
    /// <param name="source"> The vector to modify </param>
    /// <param name="direction"> The direction to nullify </param>
    /// <returns></returns>
    public static Vector3 CollapseDirection(this Vector3 source, Vector3 direction)
    {
        return source - (Vector3.Dot(source, direction) * direction);
    }
    //Vector3.Dot() It lets you get the magnitude of a component of a vector that is in a particular direction
}

public enum VectorAxis
{
    X = 0,
    Y = 1,
    Z = 2
}
