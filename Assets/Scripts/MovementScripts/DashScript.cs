using System.Collections;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField, Tooltip("Minimum time in seconds between consecutive dashes, assuming at least one dash charge is available.")]private float dashCooldown = 0.3f;
    [SerializeField, Tooltip("Time in seconds it takes to regenerate one dash charge after a dash has been used.")]private float dashRechargeTime  = 2.5f;
    [SerializeField, Tooltip("How long is the dash in seconds.")]private float dashDuration = 0.35f;
    [SerializeField]private float dashForce = 3f;
    [SerializeField, Tooltip("Maximum number of dash charges the player can hold. Each dash consumes one charge. Charges regenerate one at a time after the dash cooldown period.")]private int maxDashAmount = 1;
    private int currentDashAmount;

    [Header("Camera")]
    [SerializeField, Tooltip("Camera. When empty the code will use the camera with the tag MainCamera.")]private Camera targetCamera;
    [SerializeField, Tooltip("Normal Field of View.")]private float normalFov = 60f;
    [SerializeField, Tooltip("Field of View while dashing.")]private float dashFov = 95f;
    
    [Header("Settings")]
    [SerializeField, Tooltip("If true, the player dashes in the direction of movement input (WASD) and if no input is given the player dashes upward. If false, the player always dashes forward based on the camera's facing direction.")]
    private bool omnidirectionalDash = false;
    [SerializeField, Tooltip("Use gravity while dashing.")]private bool useGravity = false;
    [SerializeField, Tooltip("Change Field of View while dashing.")]private bool changeFoV = true;

    private bool canDash;

    [Header("References")]
    [SerializeField]private PlayerMovementScript pm;
    private Transform orientation;
    private Rigidbody rb;
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

        Debug.Log(currentDashAmount);
        currentDashAmount = maxDashAmount;
        Debug.Log(currentDashAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Dash.ReadValue<float>() > 0f && canDash && currentDashAmount > 0){
            Dash();
            //Reset values set by dashing
            Invoke(nameof(ResetDash), dashDuration);
            //Recharge dash
            Invoke(nameof(AddDash), dashRechargeTime);
            //Enable dashing again 
            Invoke(nameof(EnableDash), dashCooldown);
        }
    }

    private void Dash(){
        pm.dashing = true;

        //Disables gravity based on useGravity variable
        if (!useGravity){
            pm.rb.useGravity = false;
        }

        //Calls coroutine to change FoV. If the camera is not assigned it will use camera with the tag MainCamera.
        if (changeFoV){
            if (targetCamera == null){
                StartCoroutine(ChangeFoV(dashDuration / 2f, dashFov));
            }
            else{
                StartCoroutine(ChangeFoV(dashDuration / 2f, dashFov, targetCamera));
            }
        }

        //Pick between omnidirectional dash and Camera-Based Dash
        if (omnidirectionalDash){
            Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

            if (inputVector == Vector2.zero){
                //Resets up/down velocity
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                //Dash up
                rb.AddForce(orientation.up * dashForce * 5, ForceMode.Impulse);
            }
            else{
                if (!pm.grounded){
                    //Resets up/down velocity
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                }

                //Dash based on input so right/left + forward/back
                rb.AddForce(orientation.forward * inputVector.y * dashForce * 10 + orientation.right * inputVector.x * dashForce * 10, ForceMode.Impulse);
            }
        }
        else{
            //Dash forward
            rb.AddForce(Camera.main.transform.forward * dashForce * 10, ForceMode.Impulse);
        }

        currentDashAmount -= 1;
        canDash = false;
    }

    private void ResetDash(){
        pm.dashing = false;

        //Enables gravity based on useGravity variable
        if (!useGravity){
            pm.rb.useGravity = true;
        }

        //Calls coroutine to change FoV. If the camera is not assigned it will use camera with the tag MainCamera.
        if (changeFoV){
            if (targetCamera == null){
                StartCoroutine(ChangeFoV(dashDuration, normalFov));
            }
            else{
                StartCoroutine(ChangeFoV(dashDuration, normalFov, targetCamera));
            }
        }
    }

    private void AddDash(){
        currentDashAmount++;
    }

    private void EnableDash(){
        canDash = true;
    }

    private IEnumerator ChangeFoV(float duration, float targetFoV){
        //Create variables
        float timer = 0f;
        float t;
        float startFoV = Camera.main.fieldOfView;

        //While loop to smoothly transition between the FoV
        while (timer < duration){
            t = timer / duration;

            Camera.main.fieldOfView = Mathf.Lerp(startFoV, targetFoV, t);

            timer += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator ChangeFoV(float duration, float targetFoV, Camera targetCamera){
        //Create variables
        float timer = 0f;
        float t;
        float startFoV = Camera.main.fieldOfView;

        //While loop to smoothly transition between the FoV
        while (timer < duration){
            t = timer / duration;

            targetCamera.fieldOfView = Mathf.Lerp(startFoV, targetFoV, t);

            timer += Time.deltaTime;

            yield return null;
        }
    }
}
