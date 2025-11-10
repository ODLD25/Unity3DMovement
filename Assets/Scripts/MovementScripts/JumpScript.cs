using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField, Tooltip("Force applied to player when jumping.")] private float jumpForce = 35f;
    [SerializeField, Tooltip("Time between jumps.")] private float jumpCooldown = 0.25f;
    [Tooltip("Did the jump cooldown end?")] private bool readyToJump = true;
    [SerializeField, Tooltip("Maximum number of jumps in the air.")] private int maxJumpsInAir = 0;
    [Tooltip("Current jump")] private int curJump;

    [Header("References")]
    [SerializeField, Tooltip("Player movement script, if empty than it will use GetComponent on this object.")] private PlayerMovementScript pm;
    [SerializeField, Tooltip("Rigidbody on the player, if empty it will get the rigidbody from player movement script.")] private Rigidbody rb;
    private InputSystem_Actions inputActions;

    private void Start()
    {
        //Get Player Movement
        if (pm == null) pm = GetComponent<PlayerMovementScript>();

        //Get Input Action Map and activates it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        //Get rigidbody from player movement script
        rb = pm.rb;

        //Set default values
        curJump = 0;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inputActions.Player.Jump.ReadValue<float>() > 0)
        {
            if (pm.grounded && readyToJump && !pm.wallRunning)
            {
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
                readyToJump = false;
                curJump = 0;
            }
            else if (!pm.grounded && readyToJump && !pm.wallRunning && curJump < maxJumpsInAir)
            {
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
                readyToJump = false;
                curJump++;
            }
        }
    }

    private void Jump()
    {
        if (pm.IsOnSlope()) pm.ExitSlope();

        //Resets up/down velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        //Adds force to jump
        rb.AddForce(Vector3.up * jumpForce * 10f, ForceMode.Force);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
    
    void OnDisable()
    {
        if (inputActions != null) inputActions.Player.Disable();
    }
}
