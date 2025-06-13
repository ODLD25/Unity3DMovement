using System.Collections;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
#region Variables
    [Header("Movement")]
    public float moveSpeed;
    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
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
    [SerializeField]private float groundDrag = 5f;
    [SerializeField]private float airDrag = 0f;
    [SerializeField]private float slideDrag = 1f;

    [Header("Ground Check")]
    public bool grounded;
    [SerializeField]private float playerHeight = 2;
    [SerializeField, Tooltip("LayerMask containing all layers that acts as ground. Default is all.")]private LayerMask layerMask = ~0;

    [Header("Slope Handling")]
    [SerializeField, Tooltip("If the angle of a slope exceeds this number than script wont detect it as a slope.")]private float maxSlopeAngle = 45f;
    [SerializeField, Tooltip("How much drag is applied at different slope angles.")]private AnimationCurve dragToSlopeCurve = new AnimationCurve(
        new Keyframe(20, 5),
        new Keyframe(25, 7),
        new Keyframe(40, 10)
    );
    private RaycastHit slopeHit;

    [Header("References")]
    public Rigidbody rb;
    public Transform orientation;
    [HideInInspector]public InputSystem_Actions inputActions;
#endregion

#region Unity Mehod's
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

        if (desiredMoveSpeed < moveSpeed && Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f){
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else{
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void FixedUpdate() {
        Move();
    }
#endregion  

#region Movement
    private void StateHandler(){
        //Logic for movement states
        if (sliding){
            movementState = MovementState.Sliding;
            desiredMoveSpeed = slideSpeed;
        }
        else if (crouching){
            movementState = MovementState.Crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (sprinting && grounded){
            movementState = MovementState.Sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (dashing){
            movementState = MovementState.Dashing;
            desiredMoveSpeed = dashSpeed;
        }
        else if (grounded){
            movementState = MovementState.Walking;
            desiredMoveSpeed = walkSpeed;
        }
        else{
            movementState = MovementState.Air;
        }
    }

    private void DragHandler(){
        //Adjusts how quickly the player slows down based on where they are (slope, air, or ground)
        if (movementState == MovementState.Air || !grounded || movementState == MovementState.Dashing){
            rb.linearDamping = airDrag;
        }
        else if (movementState == MovementState.Sliding){
            rb.linearDamping = slideDrag;
        }
        else if (IsOnSlope()){
            rb.linearDamping = dragToSlopeCurve.Evaluate(GetSlopeAngle());
        }
        else{
            rb.linearDamping = groundDrag;
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed(){
        float time = 0f;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference){
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void Move()
    {
        //Reads input
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        //If player is on a slope it applies force to the direction of the slope. It helps with steeper slopes.
        if (IsOnSlope()){
            rb.AddForce(GetSlopeMoveDirection() * inputVector * moveSpeed * 30f, ForceMode.Force);
        
            if (rb.linearVelocity.y != 0 && inputVector.y != 0){
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (GetMovementSpeed() < desiredMoveSpeed){
            //Add move force based on input, rotation and if the player is grounded 
            if (grounded && !sliding) rb.AddForce(inputVector.y * orientation.forward * moveSpeed * 10 + inputVector.x * orientation.right * moveSpeed * 10, ForceMode.Force);
            else if (!sliding) rb.AddForce(inputVector.y * orientation.forward * moveSpeed * airControl + inputVector.x * orientation.right * moveSpeed * airControl, ForceMode.Force);
        }
        
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

    private float GetMovementSpeed(){
        return new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
    }

    private void GravityHandler(){
        if (sliding){
            rb.useGravity = true;
        }
        else{
            //If player is on slope gravity is turned off becose gravity makes the player go down the slope.
            rb.useGravity = !IsOnSlope();
        }
    }
#endregion

#region Slope Methods
    private bool IsOnSlope(){
        //Shoots Raycast down to detect slope(hopefully)
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.3f)){
            //Gets angle of the slope
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //Returns bool based on specified parameters
            return angle < maxSlopeAngle && angle != 0f;
        }

        return false;
    }

    private float GetSlopeAngle(){
        //Shoots Raycast down to detect slope(hopefully)
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.3f)){
            //Gets angle of the slope and returns it
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle;
        }
        //Lets hope code never gets here
        else return 0f;
    }

    public Vector3 GetSlopeMoveDirection(){
        //Finds the direction the player should move when standing on a slope (points downhill)
        return Vector3.ProjectOnPlane(Vector3.up, slopeHit.normal).normalized;
    }
#endregion
}
