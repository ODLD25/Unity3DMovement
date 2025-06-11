using UnityEngine;

public class CrouchScript : MonoBehaviour
{
    [Header("Crouch")]
    [SerializeField, Tooltip("Object that this script is on will be set to this scale when crouching.")]private float crouchScale = 0.5f;
    [Tooltip("Scale when not crouching.")]private float defaultScale;

    [Header("References")]
    [SerializeField]private PlayerMovementScript pm;
    private Rigidbody rb;
    private InputSystem_Actions inputActions;

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

        //Sets starting y scale
        defaultScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Start or end crouch based on input
        if (inputActions.Player.Crouch.ReadValue<float>() > 0f && !pm.crouching && !pm.sliding){
            StartCrouch();
        }
        else if (inputActions.Player.Crouch.ReadValue<float>() == 0f && pm.crouching){
            EndCrouch();
        }
    }

    private void StartCrouch(){
        //Set scale
        transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);

        if (pm.grounded){
            //Resets up/down velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            //Adds force down so the player won't say in the air for short time 
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }

        pm.crouching = true;
    }

    private void EndCrouch(){
        //Set scale
        transform.localScale = new Vector3(transform.localScale.x, defaultScale, transform.localScale.z);

        //Add force upwards so the player wont bug in the ground
        rb.AddForce(Vector3.up * 10f, ForceMode.Force);

        pm.crouching = false;
    }
}
