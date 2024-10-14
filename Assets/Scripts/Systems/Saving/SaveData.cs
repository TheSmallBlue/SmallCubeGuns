using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SaveData
{
}

[System.Serializable]
public class SettingsData : SaveData
{
    // Camera
    public PlayerCamera.CameraSettings cameraSettings;
}
