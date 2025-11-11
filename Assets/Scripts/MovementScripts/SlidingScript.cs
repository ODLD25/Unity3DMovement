using System.Security.Cryptography;
using UnityEngine;

public class SlidingScript : MonoBehaviour
{
    [Header("Slide settings")]
    [SerializeField, Tooltip("Speed needed to start sliding.")] private float minSpeedToStartSlide = 4f;
    [SerializeField, Tooltip("Speed needed to force stop sliding.")] private float speedToStopSlide = 2f;
    [SerializeField, Tooltip("Scale when sliding.")] private float slideYScale = 0.25f;
    [SerializeField] private float slideCooldown = 0.25f;
    [SerializeField, Tooltip("Force applied at the start of slide.")] private float startSlideForce = 10f;
    [Tooltip("What player has to do to start sliding. When this is set to Input player has to hold sprint button and slide button to start sliding. When this is set to Speed than player has to hold slide button and has to go atleast x meters per second where x is the value you set in minSpeedToStartSlide variable.")] public StartSlideType startSlideType = StartSlideType.Input;
    [SerializeField, Tooltip("Force applied every FixedUpdate")] private float slideForce;
    private bool canSlide;

    [SerializeField, Tooltip("If slide should cancel when player jumps while sliding.")] private bool stopSlideOnJump = false;
    private float startSlideYScale;

    [Header("Slide Bypass")]
    [SerializeField, Tooltip("The movement speed below which the slide cooldown can be bypassed (only applies if bypassing is enabled). Lower values make bypassing less common; higher values make it easier to bypass.")] private float slideCooldownBypassSpeed = 5f;
    [Tooltip("If enabled, the player can bypass the slide cooldown when their speed falls below the defined threshold.")] public bool canBypassSlideCooldown = true;

    [Header("References")]
    [SerializeField] private PlayerMovementScript pm;
    [SerializeField] private Rigidbody rb;
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
        startSlideYScale = transform.localScale.y;

        canSlide = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canBypassSlideCooldown && pm.sliding == true)
        {
            if (pm.GetMovementSpeed() < slideCooldownBypassSpeed)
            {
                canSlide = true;
            }
        }

        if (((startSlideType == StartSlideType.Input || startSlideType == StartSlideType.Both) && inputActions.Player.Sprint.ReadValue<float>() > 0 && inputActions.Player.Slide.ReadValue<float>() > 0f && canSlide && !pm.sliding && pm.movementState != PlayerMovementScript.MovementState.Crouching) || ((startSlideType == StartSlideType.Speed || startSlideType == StartSlideType.Both) && pm.GetMovementSpeed() > minSpeedToStartSlide && inputActions.Player.Slide.ReadValue<float>() > 0f && !pm.sliding && canSlide && pm.movementState != PlayerMovementScript.MovementState.Crouching))
        {
            StartSliding();
            canSlide = false;
        }
        else if ((pm.GetMovementSpeed() < speedToStopSlide || inputActions.Player.Slide.ReadValue<float>() <= 0f) && pm.sliding || (inputActions.Player.Jump.ReadValue<float>() > 0 && stopSlideOnJump))
        {
            StopSliding();
            Invoke(nameof(ResetCanSlide), slideCooldown);
        }
    }

    void FixedUpdate()
    {
        if (pm.sliding && pm.grounded)
        {
            rb.AddForce(pm.orientation.forward * slideForce, ForceMode.Force);
        }
    }

    private void StartSliding()
    {
        pm.sliding = true;

        if (slideYScale < 0.5f)
        {
            transform.localScale = new Vector3(slideYScale * 2, slideYScale, slideYScale * 2);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        }

        if (pm.grounded)
        {
            rb.AddForce(pm.orientation.forward * startSlideForce, ForceMode.Impulse);
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }

        rb.useGravity = true;
    }

    private void StopSliding()
    {
        pm.sliding = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        transform.localScale = new Vector3(1, startSlideYScale, 1);
    }

    private void ResetCanSlide()
    {
        canSlide = true;
    }
    
    void OnDisable()
    {
        if (inputActions != null) inputActions.Player.Disable();
    }
}

public enum StartSlideType{
    None,
    Input,
    Speed,
    Both
}