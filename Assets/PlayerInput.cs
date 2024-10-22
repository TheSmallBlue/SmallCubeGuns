using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set;}

    // Axis
    public Vector2 MovementInput => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    public Vector2 LookInput =>     new Vector2(Input.GetAxisRaw("LookHorizontal"), Input.GetAxisRaw("LookVertical"));
    public float TiltInput =>       Input.GetAxisRaw("Tilt");

    // Button Bools
    public bool Sprint =>   Input.GetButton("Sprint");
    public bool Jump =>     Input.GetButton("Jump");
    public bool Fire =>     Input.GetButton("Fire");
    public bool AltFire =>  Input.GetButton("AltFire");

    // Button Actions
    public Action OnSprint =    delegate { };
    public Action OnJump =      delegate { };
    public Action OnFire =      delegate { };
    public Action OnAltFire =   delegate { };

    public Vector3 GetCameraBasedForward() => Camera.main.GetCameraBasedForward(MovementInput.x, MovementInput.y).normalized;

    private void Awake() 
    {
        Instance = this;
    }

    private void Update() 
    {
        if (Input.GetButtonDown("Sprint"))  OnSprint();
        if (Input.GetButtonDown("Jump"))    OnJump();
        if (Input.GetButtonDown("Fire"))    OnFire();
        if (Input.GetButtonDown("AltFire")) OnAltFire();
    }
}
