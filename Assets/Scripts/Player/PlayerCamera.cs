using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform headTransform;

    [SerializeField] bool overrideSettings;
    public CameraSettings settings = new();

    Vector2 _movement;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;

        if(overrideSettings) return;

        settings = SaveManager.Load<SettingsData>().cameraSettings;
    }

    private void LateUpdate() 
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Mouse X") * (settings.flipX ? -1 : 1), -Input.GetAxisRaw("Mouse Y") * (settings.flipY ? -1 : 1));

        transform.position = headTransform.position;

        if(input.magnitude == 0) return;
        
        _movement += input * (settings.sensitivity * 500f) * Time.deltaTime;
        _movement.y = Mathf.Clamp(_movement.y, -89f, 89f);

        transform.rotation = Quaternion.Euler(_movement.y, _movement.x, 0);
    }

    [System.Serializable]
    public class CameraSettings
    {
        public bool flipX, flipY;

        [Range(0f, 1f)]
        public float sensitivity = 0.5f;
    }
}
