using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraControllerScript : MonoBehaviour
{
    [Header("Settings")]

    [Tooltip("Code accesible bool that determines if the player can rotate or not. Useful if you want the player to not be able to look when the game is pused or when they are doing certain things.")]
    public bool canRotate = true;

    [SerializeField] private float xSensitivity = 100;
    [SerializeField] private float ySensitivity = 100;

    [SerializeField, Tooltip("If true than camera FoV will change based on speed and ")] private bool changeFoVWithSpeed;

    [SerializeField]
    private AnimationCurve cameraFoVAtSpeedCurve = new AnimationCurve(
        new Keyframe(0, 60),
        new Keyframe(15, 60),
        new Keyframe(20, 70),
        new Keyframe(40, 80),
        new Keyframe(80, 95)
    );
    private float fovVelocity;

    [Tooltip("If this is true it will hide the mouse and keep it in the center of the screen so the mouse wont go to other windows when this game is active. For first person I recommend to leave this on")]
    [SerializeField] private bool lockMouse = true;
    [SerializeField, Tooltip("How quickly will the camera rotate. If you want to do it instantly set it to 1."), Range(0, 100)] private float rotationSpeed = 25f;
    private float yRotation;
    private float xRotation;


    [Header("References")]

    [Tooltip("This should not be a player but rather an empty gameObject set at 0, 0, 0 that is child of the player.")]
    [SerializeField] private Transform orientation;
    [SerializeField] private PlayerMovementScript pm;
    private InputSystem_Actions inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Locks or unlocks the mouse
        ChangeMouseLock(lockMouse);

        //Get Input
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate) Rotate();
        if (changeFoVWithSpeed)
        {
            Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, cameraFoVAtSpeedCurve.Evaluate(pm.GetMovementSpeed()), ref fovVelocity, 0.35f);
        }
    }

    private void Rotate()
    {
        //Gets input and multiply it by Time.deltaTime so its not frame-dependent and by sensitivity
        float mouseX = inputActions.Player.Look.ReadValue<Vector2>().x * xSensitivity;
        float mouseY = inputActions.Player.Look.ReadValue<Vector2>().y * ySensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotates orientation object and Slerps the camera orientation for smoother look.
        orientation.rotation = Quaternion.Euler(orientation.eulerAngles.x, yRotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xRotation, yRotation, 0f), rotationSpeed * Time.deltaTime);
    }


    public void ChangeMouseLock(bool mouseLocked)
    {
        if (mouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnDisable()
    {
        if (inputActions != null) inputActions.Player.Disable();
    }
}