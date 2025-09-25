using UnityEngine;

public class LadderClimbingScript : MonoBehaviour
{
    [Header("Ladder Climbing")]
    [SerializeField] private float climbSpeed;
    [SerializeField] private float downForceOnLadder;

    [Header("Settings")]
    [SerializeField] private bool stopClimbingOnStopInput = false;
    [SerializeField] private bool jumpOnLadder = false;
    [SerializeField] private bool useGravityWhileNotMoving = true;

    [Header("Detection")]
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private float ladderDetectionDistance;

    [Header("Ladder Jump")]
    [SerializeField] private float forwardJumpForce;
    [SerializeField] private float forwardUpForce;

    [Header("References")]
    [SerializeField] private PlayerMovementScript pm;
    [SerializeField] private Rigidbody rb;
    private InputSystem_Actions inputActions;
    private RaycastHit ladderHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get Player Movement
        if (pm == null) pm = GetComponent<PlayerMovementScript>();

        //Get rigidbody from player movement script
        rb = pm.rb;

        //Get Input Action Map and activate it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out ladderHit, ladderDetectionDistance, ladderLayer) && !pm.climbingLadder)
        {
            StartClimbing();
        }
        else
        {
            StopClimbing();
        }
    }

    void FixedUpdate()
    {
        if (pm.climbingLadder)
        {
            Climb();
        }
    }

    private void Climb()
    {
        rb.AddForce(Vector3.up * climbSpeed * inputActions.Player.Climb.ReadValue<float>(), ForceMode.Force);
    }

    private void StartClimbing()
    {
        pm.climbingLadder = true;
    }

    private void StopClimbing()
    {
        pm.climbingLadder = false;
    }

    private void LadderJump()
    {

    }
}

public enum WhenToUseGravity
{
    Never,
    WhileMoving,
    WhileNotMoving,
    Both
}