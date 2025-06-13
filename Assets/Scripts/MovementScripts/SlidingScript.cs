using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SlidingScript : MonoBehaviour
{
    [Header("Slide settings")]
    [SerializeField, Tooltip("Speed needed to start sliding.")]private float minSpeedToStartSlide = 6f;
    [SerializeField, Tooltip("Speed needed to force stop sliding.")]private float minSpeedToStopSlide = 1f;
    [SerializeField]private float slideYScale = 0.5f;
    private float startSlideYScale;

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
        startSlideYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude > minSpeedToStartSlide && inputActions.Player.Slide.ReadValue<float>() > 0f && !pm.sliding){
            StartSliding();
        }
        else if ((new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude < minSpeedToStopSlide || inputActions.Player.Slide.ReadValue<float>() <= 0f) && pm.sliding){
            StopSliding();
        }
    }

    private void FixedUpdate() {
        Slide();
    }

    private void StartSliding(){
        pm.sliding = true;
        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);

        rb.AddForce(pm.orientation.forward * 50, ForceMode.Impulse);
    }

    private void Slide(){
        
    }

    private void StopSliding(){
        pm.sliding = false;
        transform.localScale = new Vector3(transform.localScale.x, startSlideYScale, transform.localScale.z);
    }
}