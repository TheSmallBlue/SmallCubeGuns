using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHelpers
{
    public static Vector3 GetInputForward(Camera relativeView, string horizontalAxisName, string verticalAxisName)
    {
        return relativeView.GetCameraBasedForward(Input.GetAxisRaw(horizontalAxisName), Input.GetAxisRaw(verticalAxisName)).normalized;
    }
}
