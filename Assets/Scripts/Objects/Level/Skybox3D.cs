using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Skybox3D : MonoBehaviour
{
    [SerializeField] float size;

    Camera skyboxCamera;

    private void Awake()
    {
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.renderType = CameraRenderType.Overlay;

        skyboxCamera = GetComponentInChildren<Camera>(true);
        skyboxCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        skyboxCamera.transform.localPosition = Camera.main.transform.position / size;
        skyboxCamera.transform.rotation = Camera.main.transform.rotation;
    }
}
