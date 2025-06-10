using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraControllerScript : MonoBehaviour
{
    [Header("Settings")]

    [Tooltip("Code accesible bool that determines if the player can rotate or not. Useful if you want the player to not be able to look when the game is pused or when they are doing certain things.")]
    public bool canRotate = true;

    [SerializeField]private float xSensitivity = 100;
    [SerializeField]private float ySensitivity = 100;

    [Tooltip("If this is true it will hide the mouse and keep it in the center of the screen so the mouse wont go to other windows when this game is active. For first person I recommend to leave this on")]
    [SerializeField]private bool lockMouse = true;
    [SerializeField, Tooltip("How quickly will the camera rotate. If you want to do it instantly set it to 1."), Range(0, 1)]private float rotationSpeed = 0.05f;
    private float yRotation;
    private float xRotation;


    [Header("References")]

    [Tooltip("This should not be a player but rather an empty gameObject set at 0, 0, 0 that is child of the player.")]
    [SerializeField]private Transform orientation;
    private InputSystem_Actions inputActions;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Locks or unlocks the mouse
        if (lockMouse){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Get Input
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate) Rotate();
    }

    private void Rotate(){
        //Gets input and multiply it by Time.deltaTime so its not frame-dependent and by sensitivity
        float mouseX = inputActions.Player.Look.ReadValue<Vector2>().x * Time.deltaTime * xSensitivity;
        float mouseY = inputActions.Player.Look.ReadValue<Vector2>().y * Time.deltaTime * ySensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotates orientation object and Slerps the camera orientation for smoother look.
        orientation.rotation = Quaternion.Euler(orientation.eulerAngles.x, yRotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xRotation, yRotation, 0f), rotationSpeed);
    }
}
