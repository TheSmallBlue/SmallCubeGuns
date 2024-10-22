using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] bool overrideSettings;
    public CameraSettings settings = new();

    Transform _camera;
    Vector2 _movement;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;

        _camera = Camera.main.transform.root;

        if(overrideSettings) return;

        settings = SaveManager<SettingsData>.Load().cameraSettings;
    }

    private void LateUpdate() 
    {
        Vector2 input = new Vector2(PlayerInput.Instance.LookInput.x * (settings.flipX ? -1 : 1), -PlayerInput.Instance.LookInput.y * (settings.flipY ? -1 : 1));

        _camera.position = transform.position;

        if(input.magnitude == 0) return;
        
        _movement += input * (settings.sensitivity * 100f) * Time.deltaTime;
        _movement.y = Mathf.Clamp(_movement.y, -89f, 89f);

        transform.rotation = Quaternion.Euler(_movement.y, _movement.x, 0);
        _camera.rotation = transform.rotation;
    }

    [System.Serializable]
    public class CameraSettings
    {
        public bool flipX, flipY;

        [Range(0f, 1f)]
        public float sensitivity = 0.5f;
    }
}

#if UNITY_EDITOR

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
        EditorGUILayout.PropertyField(overrideSettings);

        if (overrideSettings.boolValue)
        {
            EditorGUILayout.PropertyField(cameraSettings);
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}

#endif
