using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class SaveData
{
    
}

/// <summary>
/// Class intended for the saving of system data. Like preferences.
/// </summary>
[System.Serializable]
public class SettingsData : SaveData
{
    // Camera
    public PlayerCamera.CameraSettings cameraSettings = new();
}
