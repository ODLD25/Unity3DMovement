using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashScript : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField, Tooltip("Minimum time in seconds between consecutive dashes, assuming at least one dash charge is available.")] private float dashCooldown = 0.3f;
    [SerializeField, Tooltip("Time in seconds it takes to regenerate one dash charge after a dash has been used.")] private float dashRechargeTime = 2.5f;
    [SerializeField, Tooltip("How long is the dash in seconds.")] private float dashDuration = 0.35f;
    [SerializeField] private float dashForce = 12.5f;
    [SerializeField, Tooltip("Maximum number of dash charges the player can hold. Each dash consumes one charge. Charges regenerate one at a time after the dash cooldown period.")] private int maxDashAmount = 1;
    private int currentDashAmount;

    [Header("Canvas")]
    public bool showDashCrosshair = true;
    [SerializeField] private Image dashCrosshair;
    [SerializeField] private float dashCrosshairSize = 15;
    [SerializeField] private GameObject dashCanvas;

    [Header("Camera")]
    [Tooltip("Force FoV ignoring FoV from camera controller.")] public bool forceFoV = false;
    [SerializeField, Tooltip("Camera. When empty the code will use the camera with the tag MainCamera.")] private Camera targetCamera;
    [SerializeField, Tooltip("Field of View while dashing.")] private float dashFov = 95f;

    [Header("Settings")]
    [SerializeField, Tooltip("If true, the player dashes in the direction of movement input (WASD) and if no input is given the player dashes upward. If false, the player always dashes forward based on the camera's facing direction.")] private bool omnidirectionalDash = false;
    [SerializeField, Tooltip("Use gravity while dashing.")] private bool useGravity = false;

    private bool canDash;

    [Header("References")]
    [SerializeField] private PlayerMovementScript pm;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    private InputSystem_Actions inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get Player Movement
        if (pm == null) pm = GetComponent<PlayerMovementScript>();

        //Get Input Action Map and activates it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        //Get rigidbody from player movement script
        rb = pm.rb;

        //Get Orientation from player movement script
        orientation = pm.orientation;

        canDash = true;

        currentDashAmount = maxDashAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Dash.ReadValue<float>() > 0f && canDash && currentDashAmount > 0)
        {
            Dash();
            //Reset values set by dashing
            Invoke(nameof(ResetDash), dashDuration);
            //Recharge dash
            Invoke(nameof(AddDash), dashRechargeTime);
            //Enable dashing again 
            Invoke(nameof(EnableDash), dashCooldown);
        }

        UpdateDashCanvas();
    }

    private void Dash()
    {
        pm.dashing = true;

        //Disables gravity based on useGravity variable
        if (!useGravity)
        {
            pm.rb.useGravity = false;
        }

        //Calls coroutine to change FoV. If the camera is not assigned it will use camera with the tag MainCamera.
        if (forceFoV)
        {
            if (targetCamera == null)
            {
                StartCoroutine(CameraFOVManager.ChangeFoV(dashDuration / 2f, dashFov));
            }
            else
            {
                StartCoroutine(CameraFOVManager.ChangeFoV(dashDuration / 2f, dashFov, targetCamera));
            }
        }

        //Pick between omnidirectional dash and Camera-Based Dash
        if (omnidirectionalDash)
        {
            Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

            if (inputVector == Vector2.zero)
            {
                //Resets up/down velocity
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                //Dash up
                rb.AddForce(orientation.up * dashForce * 5, ForceMode.Impulse);
            }
            else
            {
                if (!pm.grounded)
                {
                    //Resets up/down velocity
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                }

                //Dash based on input so right/left + forward/back
                rb.AddForce(orientation.forward * inputVector.y * dashForce + orientation.right * inputVector.x * dashForce, ForceMode.Impulse);
            }
        }
        else
        {
            //Dash forward
            rb.AddForce(Camera.main.transform.forward * dashForce, ForceMode.Impulse);
        }

        currentDashAmount -= 1;
        canDash = false;
    }

    private void ResetDash()
    {
        pm.dashing = false;

        //Enables gravity based on useGravity variable
        if (!useGravity)
        {
            pm.rb.useGravity = true;
        }

        //Calls coroutine to change FoV. If the camera is not assigned it will use camera with the tag MainCamera.
        if (forceFoV)
        {
            if (targetCamera == null)
            {
                StartCoroutine(CameraFOVManager.ResetFoV(dashDuration));
            }
            else
            {
                StartCoroutine(CameraFOVManager.ResetFoV(dashDuration, targetCamera));
            }
        }
    }

    private void AddDash()
    {
        currentDashAmount++;
    }

    private void EnableDash()
    {
        canDash = true;
    }

    private void UpdateDashCanvas()
    {
        if (showDashCrosshair)
        {
            dashCanvas.SetActive(true);

            dashCrosshair.fillAmount = (float)currentDashAmount / (float)maxDashAmount;
            dashCrosshair.rectTransform.sizeDelta = new Vector2(dashCrosshairSize, dashCrosshairSize);
        }
        else
        {
            dashCanvas.SetActive(false);
        }
    }
    
    void OnDisable()
    {
        inputActions.Player.Disable();
    }
}