using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField]private float jumpForce = 25f;
    [SerializeField]private float jumpCooldown = 0.25f;
    [SerializeField]private bool readyToJump = true;
    [SerializeField]private int maxJumps = 1;
    private int curJump;

    [Header("References")]
    [SerializeField]private PlayerMovementScript pm;
    private Rigidbody rb;
    private InputSystem_Actions inputActions;

    private void Start() {
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
        if (pm.grounded && readyToJump){
            curJump = 0;
        }

        //Starts jump
        if (inputActions.Player.Jump.ReadValue<float>() > 0 && (pm.grounded || curJump < maxJumps) && readyToJump){
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
            readyToJump = false;
            curJump++;
        }
    }

    private void Jump(){
        //Resets up/down velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        //Adds force to jump
        rb.AddForce(Vector3.up * jumpForce * 10f, ForceMode.Force);
    }

    private void ResetJump(){
        readyToJump = true;
    }
}
