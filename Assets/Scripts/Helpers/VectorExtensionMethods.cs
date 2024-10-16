using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensionMethods
{
    public static Vector3 SetAxis(this Vector3 vector, VectorAxis axis, float number)
    {
        vector[(int)axis] = number;
        return vector;
    }

    public static Vector3 AddToAxis(this Vector3 vector, VectorAxis axis, float number)
    {
        vector[(int)axis] += number;
        return vector;
    }

    public static Vector3 CollapseAxis(this Vector3 vector, VectorAxis axis)
    {
        return vector.SetAxis(axis, 0);
    }

    enum IndexType
    {
        X,
        Y,
        Z
    }
}

public enum VectorAxis
{
    X = 0,
    Y = 1,
    Z = 2
}
