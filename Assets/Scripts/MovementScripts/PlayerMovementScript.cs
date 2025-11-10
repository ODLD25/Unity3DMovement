using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovementScript : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("Speed")]
    [SerializeField, Tooltip("Max speed while walking")] private float walkSpeed = 5f;
    [SerializeField, Tooltip("Max speed while sprinting")] private float sprintSpeed = 7.5f;
    [SerializeField, Tooltip("Max speed while crouching")] private float crouchSpeed = 3.5f;
    [SerializeField, Tooltip("Max speed while dashing")] private float dashSpeed = 100f;
    [SerializeField, Tooltip("Max speed while sliding")] private float slideSpeed = 100f;
    [SerializeField, Tooltip("Max speed while wallRunning")] private float wallRunSpeed = 50f;
    [SerializeField, Tooltip("After reaching this speed player wont be able to go faster using controls.")] private float airMoveSpeed = 5f;

    [Header("Control")]
    [SerializeField, Tooltip("How much control does player have."), Range(0, 1)] private float moveControl = 1f;
    [SerializeField, Tooltip("How much control does player have while in air."), Range(0, 1)] private float airControl = 0.25f;
    [SerializeField, Tooltip("How much control does player have while sliding."), Range(0, 1)] private float slideControl = 0.25f;
    [SerializeField, Tooltip("How much control does player have while being on ice."), Range(0, 1)] private float iceControl = 0.25f;

    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Dashing,
        Sliding,
        WallRunning,
        Air
    }

    public MovementState movementState;

    [HideInInspector] public bool sprinting;
    [HideInInspector] public bool crouching;
    [HideInInspector] public bool dashing;
    [HideInInspector] public bool sliding;
    [HideInInspector] public bool wallRunning;
    [HideInInspector] public bool onIce;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 3f;
    [SerializeField] private float airDrag = 0f;
    [SerializeField] private float slideDrag = 0.25f;
    [SerializeField] private float dashDrag = 0.5f;
    [SerializeField] private float wallRunDrag = 2f;
    [SerializeField] private float airDragMultiplier = 0.1f;

    [Header("VFX")]
    [SerializeField] private bool useSpeedVFX;
    [SerializeField] private VisualEffect speedVFX;

    [Header("Ground Check")]
    public bool grounded;
    [SerializeField, Tooltip("LayerMask containing all layers that acts as ground. Default is all.")] private LayerMask groundLayer = ~0;
    public float playerHeight = 2;
    [SerializeField] private Transform extraRaycastParent;
    [SerializeField, Tooltip("How often (in frames) to run extra ground raycasts around the player’s feet when the main center raycast does not detect ground. Lower values = faster detection but higher CPU cost; higher values = slower detection but better performance."), Range(1, 60)] private int extraGroundCheckInterval = 5;
    [Tooltip("List containing bools for each extra raycast")] private List<bool> extraRaycastHitList;
    private List<Transform> extraRaycastTransformList;
    private bool extraRaycastHit;
    private bool jumping;

    [Header("Slope Handling")]
    [SerializeField, Tooltip("If the angle of a slope exceeds this number than script wont detect it as a slope.")] private float maxSlopeAngle = 45f;
    private RaycastHit slopeHit;
    [SerializeField] private float exitSlopeTime = 0.2f;

    [Header("References")]
    public Rigidbody rb;
    public Transform orientation;
    private InputSystem_Actions inputActions;
    #endregion

    #region Unity Mehod's
    public void Start()
    {
        //Get Input
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        //Get rigidbody component
        if (rb == null) rb = GetComponent<Rigidbody>();

        extraRaycastTransformList = new List<Transform>(extraRaycastParent.childCount);
        for (int i = 0; i < extraRaycastParent.childCount; i++)
        {
            extraRaycastTransformList.Add(extraRaycastParent.GetChild(i));
        }

        extraRaycastHitList = new List<bool>(extraRaycastTransformList.Count);
        for (int i = 0; i < extraRaycastTransformList.Count; i++)
        {
            extraRaycastHitList.Add(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Basic methods
        GroundCheck();
        StateHandler();
        DragHandler();
        SpeedControl();
        GravityHandler();
        CheckForMovingPlatform();
        CounterMovingPlatform();
        if (useSpeedVFX) ChangeSpeedVFXValue();

        if (desiredMoveSpeed < moveSpeed && Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void FixedUpdate()
    {
        Move();

        if (movementState == MovementState.Air)
        {
            CustomAirDrag();
        }
    }
    #endregion

    #region Movement
    private void StateHandler()
    {
        //Logic for movement states
        if (wallRunning)
        {
            movementState = MovementState.WallRunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        else if (sliding)
        {
            movementState = MovementState.Sliding;
            desiredMoveSpeed = slideSpeed;
        }
        else if (crouching && grounded)
        {
            movementState = MovementState.Crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (dashing)
        {
            movementState = MovementState.Dashing;
            desiredMoveSpeed = dashSpeed;
        }
        else if (sprinting && grounded)
        {
            movementState = MovementState.Sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            movementState = MovementState.Walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            movementState = MovementState.Air;
            desiredMoveSpeed = 5;
        }
    }

    private void DragHandler()
    {
        //Adjusts how quickly the player slows down based on where they are (slope, air, or ground)
        if (movementState == MovementState.WallRunning)
        {
            rb.linearDamping = wallRunDrag;
        }
        else if ((movementState == MovementState.Air) || (movementState == MovementState.Dashing && !grounded))
        {
            rb.linearDamping = airDrag;
        }
        else if (movementState == MovementState.Dashing && grounded)
        {
            rb.linearDamping = dashDrag;
        }
        else if (movementState == MovementState.Sliding)
        {
            rb.linearDamping = slideDrag;
        }
        else
        {
            rb.linearDamping = groundDrag;
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference && sliding)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;

            moveSpeed -= Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }
    
    private void Move()
    {
        if (wallRunning || dashing) return;

        //Reads input
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        //If player is on a slope it applies force to the direction of the slope. It helps with steeper slopes.
        if (IsOnSlope() && !sliding && !jumping)
        {
            rb.AddForce(Vector3.down * 20f);

            rb.AddForce(GetSlopeMoveDirection(orientation.forward) * moveSpeed * (onIce ? iceControl : moveControl) * 55f * inputVector.y, ForceMode.Force);
            rb.AddForce(orientation.right * moveSpeed * 55 * (onIce ? iceControl : moveControl) * inputVector.x, ForceMode.Force);

            if (rb.linearVelocity.y > 0 && inputVector != Vector2.zero)
            {
                rb.AddForce(Vector3.down * 90f * Mathf.Abs(GetSlopeAngle() / 20), ForceMode.Force);
            }
            else if (rb.linearVelocity.y < 0 && inputVector != Vector2.zero)
            {
                rb.AddForce(Vector3.down * 90f * Mathf.Abs(GetSlopeAngle() / 10), ForceMode.Force);
            }
        }

        if (GetMovementSpeed() < desiredMoveSpeed)
        {
            //Add move force based on input and rotation
            if (grounded && movementState != MovementState.Sliding)
            {
                rb.AddForce(inputVector.y * orientation.forward * moveSpeed * 10 * (onIce ? iceControl : moveControl) + inputVector.x * orientation.right * moveSpeed * 10 * (onIce ? iceControl : moveControl), ForceMode.Force);
            }
            else if (movementState == MovementState.Sliding)
            {
                if (grounded)
                    rb.AddForce(inputVector.y * orientation.forward * moveSpeed * 0.25f * slideControl + inputVector.x * orientation.right * moveSpeed * 0.25f * slideControl, ForceMode.Force);
            }
            else
            {
                rb.AddForce(inputVector.y * orientation.forward * 20 * airControl + inputVector.x * orientation.right * 20 * airControl, ForceMode.Force);
            }
        }
    }

    private void SpeedControl()
    {
        //If player is on slope it will set 3 axis instead of 2 becose player is faster on slopes
        if (IsOnSlope() && !jumping)
        {
            //Check if player is moving faster than it should
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                //Sets player velocity
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        else if (grounded)
        {
            //Gets current velocity without up/down velocity axis
            Vector3 curVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            //Check if player is moving faster than he should
            if (curVel.magnitude > moveSpeed)
            {
                //Create new velocity that is set to the max speed 
                Vector3 fixedVel = curVel.normalized * moveSpeed;
                //Sets the new velocity
                rb.linearVelocity = new Vector3(fixedVel.x, rb.linearVelocity.y, fixedVel.z);
            }
        }
    }

    public float GetMovementSpeed()
    {
        if (movingPlatform)
        {
            Vector3 platformVel = movingPlatform.GetComponent<Rigidbody>().linearVelocity;
            return Mathf.Clamp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude - new Vector3(platformVel.x, 0, platformVel.z).magnitude, 0, Mathf.Infinity);
        }
        return new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
    }

    private void GravityHandler()
    {
        if (!sliding) {
            //If player is on slope gravity is turned off becose gravity makes the player go down the slope.
            if (!wallRunning) rb.useGravity = !IsOnSlope();
        }
    }

    private void GroundCheck()
    {
        //Check if player is on the ground
        if (wallRunning) return;

        if (Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit, playerHeight / 2 + 0.3f,  groundLayer))
        {
            grounded = true;
            extraRaycastHit = false;
            return;
        }

        if (extraRaycastHit == true)
        {
            for (int i = 0; i < extraRaycastHitList.Count; i++)
            {
                if (extraRaycastHitList[i] == true)
                {
                    if (Physics.Raycast(extraRaycastTransformList[i].position, Vector3.down, playerHeight / 2 + 0.3f, groundLayer))
                    {
                        grounded = true;

                        return;
                    }

                    extraRaycastHitList[i] = false;
                    extraRaycastHit = false;
                    grounded = false;
                }

                extraRaycastHit = false;
            }
        }
        else if (Time.frameCount % extraGroundCheckInterval == 0)
        {
            int pos = 0;
            foreach (Transform t in extraRaycastTransformList)
            {
                Debug.DrawRay(t.position, Vector3.down, Color.red, 2f);

                if (Physics.Raycast(t.position, Vector3.down, playerHeight / 2 + 0.3f, groundLayer))
                {
                    grounded = true;
                    extraRaycastHit = true;
                    extraRaycastHitList[pos] = true;

                    return;
                }

                extraRaycastHitList[pos] = false;

                pos++;
            }

            grounded = false;
            extraRaycastHit = false;

            extraRaycastParent.transform.localRotation = Quaternion.Euler(0, extraRaycastParent.transform.localEulerAngles.y + 45f, 0);
        }
    }

    private void CustomAirDrag()
    {
        Vector3 customAirDrag = -rb.linearVelocity * airDragMultiplier;
        customAirDrag.y = 0;

        rb.AddForce(customAirDrag); 
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector3.zero;
    }
    #endregion

    #region Moving Platforms
    private GameObject movingPlatform;
    private void CheckForMovingPlatform()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, playerHeight / 2 + 0.3f))
        {
            if (hit.collider.gameObject.GetComponent<MovingPlatformScript>())
            {
                movingPlatform = hit.collider.gameObject;
                if (!jumping || !grounded)
                {
                    rb.AddForce(Vector3.down * 2);
                }
            }
            else
            {
                movingPlatform = null;
            }
        }
        else
        {
            lastVelocity = Vector3.zero;
        }
    }
    private Vector3 lastVelocity;
    private void CounterMovingPlatform()
    {
        if (movingPlatform)
        {
            Vector3 curVel = movingPlatform.GetComponent<Rigidbody>().linearVelocity;
            if (curVel != lastVelocity && !jumping)
            {
                rb.AddForce(curVel - lastVelocity);
            }

            lastVelocity = movingPlatform.GetComponent<Rigidbody>().linearVelocity;
        }
    }
    #endregion

    #region Slope Methods
    public bool IsOnSlope()
    {
        //Shoots Raycast down to detect slope(hopefully)
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.3f))
        {
            //Gets angle of the slope
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //Returns bool based on specified parameters
            return angle < maxSlopeAngle && angle != 0f;
        }

        return false;
    }

    private float GetSlopeAngle()
    {
        //Shoots Raycast down to detect slope(hopefully)
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.3f))
        {
            //Gets angle of the slope and returns it
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle;
        }
        //Lets hope code never gets here
        else return 0f;
    }

    public Vector3 GetSlopeMoveDirection()
    {
        //Finds the direction the player should move when standing on a slope (points uphill)
        return Vector3.ProjectOnPlane(Vector3.up, slopeHit.normal).normalized;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 inputDir)
    {
        //Finds the direction the player should move when standing on a slope (points uphill)
        return Vector3.ProjectOnPlane(inputDir, slopeHit.normal).normalized;
    }

    public void ExitSlope()
    {
        jumping = true;
        Invoke(nameof(ResetExitSlope), exitSlopeTime);
    }

    private void ResetExitSlope()
    {
        jumping = false;
    }
    #endregion

    #region VFX
    private void ChangeSpeedVFXValue()
    {
        speedVFX.SetFloat("Speed", GetMovementSpeed());
    }
    #endregion

    void OnDisable()
    {
        if (inputActions != null) inputActions.Player.Disable();
    }
}
