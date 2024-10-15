using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensionMethods
{
    public static Vector3 CollapseAxis(this Vector3 vector, VectorAxis axis)
    {
        vector[(int)axis] = 0;
        return vector;
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
