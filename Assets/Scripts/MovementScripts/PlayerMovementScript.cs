using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField, Tooltip("Max speed while walking")]private float walkSpeed = 5f;
    [SerializeField, Tooltip("Max speed while sprinting")]private float sprintSpeed = 7.5f;
    [SerializeField, Tooltip("Max speed while crouching")]private float crouchSpeed = 3.5f;
    [SerializeField, Tooltip("Max speed while dashing")]private float dashSpeed = 20f;
    [SerializeField, Tooltip("Max speed while sliding")]private float slideSpeed = 20f;
    [SerializeField, Tooltip("How much can player move in air.")]private float airControl = 0.25f;

    public enum MovementState{
        Walking,
        Sprinting,
        Crouching,
        Dashing,
        Sliding,
        Air
    }

    [SerializeField]private MovementState movementState;

    [HideInInspector]public bool sprinting;
    [HideInInspector]public bool crouching;
    [HideInInspector]public bool dashing;
    [HideInInspector]public bool sliding;

    [Header("Drag")]
    [SerializeField]private float groundDrag = 2f;
    [SerializeField]private float airDrag = 0f;

    [Header("Ground Check")]
    public bool grounded;
    [SerializeField]private float playerHeight = 2;
    [SerializeField, Tooltip("LayerMask containing all layers that acts as ground. Default is all.")]private LayerMask layerMask = ~0;

    [Header("Slope Handling")]
    [SerializeField, Tooltip("If the angle of a slope exceeds this number than player won't be able to walk on it.")]private float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("References")]
    public Rigidbody rb;
    public Transform orientation;
    [HideInInspector]public InputSystem_Actions inputActions;

    public bool slope;
    
    public void Start()
    {
        //Get Input
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        //Get rigidbody component
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is on the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.3f, layerMask);

        //Basic methods
        StateHandler();
        DragHandler();
        SpeedControl();
        GravityHandler();

        slope = IsOnSlope();
    }

    private void FixedUpdate() {
        Move();
    }

    private void GravityHandler(){
        rb.useGravity = !IsOnSlope();
    }

    private void StateHandler(){
        //Logic for movement states
        if (sliding){
            movementState = MovementState.Sliding;
            moveSpeed = slideSpeed;
        }
        else if (crouching){
            movementState = MovementState.Crouching;
            moveSpeed = crouchSpeed;
        }
        else if (sprinting && grounded){
            movementState = MovementState.Sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (dashing){
            movementState = MovementState.Dashing;
            moveSpeed = dashSpeed;
        }
        else if (grounded){
            movementState = MovementState.Walking;
            moveSpeed = walkSpeed;
        }
        else if (!grounded){
            movementState = MovementState.Air;
        }
    }

    private void DragHandler(){
        //Handles the drag for better movement
        if (movementState == MovementState.Air || movementState == MovementState.Dashing){
            rb.linearDamping = airDrag;
        }
        else{
            rb.linearDamping = groundDrag;
        }
    }

    private void Move()
    {
        //Reads input
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        if (IsOnSlope()){
            rb.AddForce(GetSlopeMoveDirection() * inputVector * moveSpeed * 30f, ForceMode.Force);
        
            if (rb.linearVelocity.y != 0){
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //Add move force based on input, rotation and if the player is grounded 
        if (grounded) rb.AddForce(inputVector.y * orientation.forward * moveSpeed * 10 + inputVector.x * orientation.right * moveSpeed * 10, ForceMode.Force);
        else rb.AddForce(inputVector.y * orientation.forward * moveSpeed * airControl + inputVector.x * orientation.right * moveSpeed * airControl, ForceMode.Force);

    }

    private void SpeedControl(){
        //If player is on slope it will set 3 axis instead of 2 becose player is faster on slopes
        if (IsOnSlope()){
            //Check if player is moving faster than it should
            if (rb.linearVelocity.magnitude > moveSpeed){
                //Sets player velocity
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        else{
            //Gets current velocity without up/down velocity axis
            Vector3 curVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            //Check if player is moving faster than it should
            if (curVel.magnitude > moveSpeed){
                //Create new velocity that is set to the max speed 
                Vector3 fixedVel = curVel.normalized * moveSpeed;
                //Sets the new velocity
                rb.linearVelocity = new Vector3(fixedVel.x, rb.linearVelocity.y, fixedVel.z);
            }
        }
    }

    private bool IsOnSlope(){
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.3f)){
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0f;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection(){
        return Vector3.ProjectOnPlane(Vector3.up, slopeHit.normal).normalized;
    }
}
