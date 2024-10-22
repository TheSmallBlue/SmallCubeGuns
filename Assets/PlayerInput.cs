using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set;}

    [Header("Axis")]
    [SerializeField] InputAction MovementAction;
    [SerializeField] InputAction LookAction;

    [Header("Buttons")]
    [SerializeField] InputActionMap Buttons;

    // ---

    // Axis
    public Vector2 MovementInput => MovementAction.ReadValue<Vector2>();
    public Vector2 LookInput =>     LookAction.ReadValue<Vector2>();

    // Buttons
    public bool IsButton(string buttonName) => Buttons[buttonName].IsPressed();
    public bool IsButtonDown(string buttonName) => Buttons[buttonName].WasPressedThisFrame();
    public bool IsButtonUp(string buttonName) => Buttons[buttonName].WasReleasedThisFrame();

    public void SubscribeToButton<T>(string buttonName, Action<T> onButtonPressed) where T : struct => Buttons[buttonName].performed += ctx => onButtonPressed(ctx.ReadValue<T>());

    private void Awake() 
    {
        Instance = this;
    }

    private void Update() 
    {
        MovementAction.Enable();
        LookAction.Enable();
        
        Buttons.Enable();
    }

    public Vector3 GetCameraBasedForward() => Camera.main.GetCameraBasedForward(MovementInput.x, MovementInput.y).normalized;
}
