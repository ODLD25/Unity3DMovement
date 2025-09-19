using Unity.VisualScripting;
using UnityEngine;

public class WallRunScript : MonoBehaviour
{
    [Header("Wallrun")]
    [SerializeField] private float wallRunForce = 40f;
    [SerializeField] private float wallClimbSpeed = 4f;
    [SerializeField] private LayerMask whatIsGround = ~0;
    [SerializeField] private LayerMask whatIsWall = ~0;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpUpForce = 5f;
    [SerializeField] private float wallJumpSideForce = 5f;

    [Header("Exiting wallRun")]
    private bool exitingWallRun;
    [SerializeField] private float exitWallTime = 0.2f;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity = true;
    [SerializeField] private float gravityCounterForce = 4f;

    [Header("Camera")]
    [SerializeField, Tooltip("ForFoV will change the FoV and will ignore the FoV from Camera controller script.")] public bool forceFoV = false;
    [SerializeField, Tooltip("Camera. When empty the code will use the camera with the tag MainCamera.")] private Camera targetCamera;
    [SerializeField, Tooltip("Field of View while wall running.")] private float wallRunFoV = 85f;
    [SerializeField] private float foVChangeDuration = 0.5f;


    [Header("Detection")]
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField] private float minJumpHeight = 1f;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    [SerializeField] private PlayerMovementScript pm;
    [SerializeField] private Transform orientation;
    private Rigidbody rb;
    private InputSystem_Actions inputActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get Player Movement
        if (pm == null) pm = GetComponent<PlayerMovementScript>();

        orientation = pm.orientation;

        //Get rigidbody from player movement script
        rb = pm.rb;

        //Get Input Action Map and activate it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    void FixedUpdate()
    {
        if (pm.wallRunning) WallRunMovement();
    }

    private void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        if ((wallLeft || wallRight) && inputActions.Player.Move.ReadValue<Vector2>().y > 0 && AboveGround() && !exitingWallRun && pm.movementState != PlayerMovementScript.MovementState.Sliding && pm.movementState != PlayerMovementScript.MovementState.Crouching)
        {
            if (!pm.wallRunning)
            {
                StartWallRun();
            }

            if (inputActions.Player.Jump.ReadValue<float>() > 0)
            {
                WallJump();
            }
        }
        else if (exitingWallRun)
        {
            if (pm.wallRunning)
            {
                StopWallRun();
            }

            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if (exitWallTimer <= 0)
            {
                exitingWallRun = false;
            }
        }
        else
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallRunning = true;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (forceFoV)
        {
            if (targetCamera)
            {
                CameraFOVManager.ChangeFoV(wallRunFoV, foVChangeDuration, targetCamera);
            }
            else
            {
                CameraFOVManager.ChangeFoV(wallRunFoV, foVChangeDuration);
            }
        }

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward, ForceMode.Impulse);
    }

    private void WallRunMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (inputActions.Player.UpwardsWallRun.ReadValue<float>() > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        }

        if (inputActions.Player.DownwardsWallRun.ReadValue<float>() > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);
        }

        if ((wallLeft && inputActions.Player.Move.ReadValue<Vector2>().x > 0) || (wallRight && inputActions.Player.Move.ReadValue<Vector2>().x < 0))
        {
            rb.AddForce(wallRight ? -orientation.right : orientation.right * 100, ForceMode.Force);
            exitingWallRun = true;
            exitWallTimer = exitWallTime;
        }

        if (useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }
    }

    private void StopWallRun()
    {
        pm.wallRunning = false;

        if (forceFoV)
        {
            if (targetCamera)
            {
                StartCoroutine(CameraFOVManager.ResetFoV(foVChangeDuration, targetCamera));
            }
            else
            {
                StartCoroutine(CameraFOVManager.ResetFoV(foVChangeDuration));
            }
        }
    }

    private void WallJump()
    {
        exitingWallRun = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
