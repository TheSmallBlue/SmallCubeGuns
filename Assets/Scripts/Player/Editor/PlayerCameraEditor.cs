using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerCamera))]
public class PlayerCameraEditor : Editor
{
    SerializedProperty overrideSettings, headTransform, cameraSettings;

    private void OnEnable() 
    {
        overrideSettings = serializedObject.FindProperty("overrideSettings");
        headTransform = serializedObject.FindProperty("headTransform");
        cameraSettings = serializedObject.FindProperty("settings");
    }

    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(headTransform);
        EditorGUILayout.PropertyField(overrideSettings);

        if(overrideSettings.boolValue)
        {
            EditorGUILayout.PropertyField(cameraSettings);
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}