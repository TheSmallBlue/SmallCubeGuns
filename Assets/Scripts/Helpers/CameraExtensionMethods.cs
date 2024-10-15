using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraExtensionMethods
{
    public static Vector3 GetCameraBasedForward(this Camera camera, float horMultiplier, float verMultiplier, bool collapseY = false)
    {
        return (collapseY ? camera.transform.forward.CollapseAxis(VectorAxis.Y) : camera.transform.forward) * verMultiplier + (collapseY ? camera.transform.right.CollapseAxis(VectorAxis.Y) : camera.transform.right) * horMultiplier;
    }
}
