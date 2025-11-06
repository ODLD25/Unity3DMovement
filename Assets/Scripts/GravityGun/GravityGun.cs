using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGun : MonoBehaviour
{
    [Header("PickUp")]
    [SerializeField] private float maxPickUpDistance = 5;
    [SerializeField]private LayerMask pickupbleLayerMask;

    [Header("Holding")]
    [SerializeField] private Transform objectHoldTransform;
    [SerializeField] private float lerpSpeed = 15;
    
    [Header("Scrolling")]
    [SerializeField, Tooltip("When true player can change the distance of the object when holding an object.")] private bool canAdjustHoldDistance = true;
    [SerializeField] private Vector2 minMaxHoldDistance = new Vector2(3, 5);
    [SerializeField] private float scrollSensitivity = 0.75f;
    private float holdingDistance;

    [Header("Throwing")]
    [SerializeField, Tooltip("Force used when throwing object.")] private float throwForce = 10;

    private bool hodlingObject;
    private GameObject currentObject;
    
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.GravityGun.Enable();
        inputActions.GravityGun.StartHoldingObject.performed += GravityGunInteract;
        if (canAdjustHoldDistance) inputActions.GravityGun.ChangeObjectHoldingDistance.performed += Scroll;
    }

    void FixedUpdate()
    {
        if (hodlingObject)
        {
            Vector3 targetPos = transform.position + transform.forward.normalized * holdingDistance;
            Vector3 toTarget = targetPos - currentObject.GetComponent<Rigidbody>().position;

            currentObject.GetComponent<Rigidbody>().linearVelocity = toTarget * lerpSpeed;
        }
    }

    private void GravityGunInteract(InputAction.CallbackContext context)
    {
        if (hodlingObject) ThrowObject();
        else CheckHoldingObject();
    }

    private void CheckHoldingObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxPickUpDistance, pickupbleLayerMask))
        {
            if (hit.transform.GetComponent<Rigidbody>() != null && !hit.transform.GetComponent<UnpickableScript>())
            {
                StartHoldingObject(hit);
            }
        }
    }

    private void StartHoldingObject(RaycastHit hit)
    {
        currentObject = hit.transform.gameObject;

        Rigidbody objectRb = hit.transform.GetComponent<Rigidbody>();

        objectRb.useGravity = false;

        holdingDistance = minMaxHoldDistance.x;
        hodlingObject = true;
    }

    private void Scroll(InputAction.CallbackContext context)
    {
        holdingDistance += inputActions.GravityGun.ChangeObjectHoldingDistance.ReadValue<float>() * scrollSensitivity;
        holdingDistance = Mathf.Clamp(holdingDistance, minMaxHoldDistance.x, minMaxHoldDistance.y);
    }

    private void ThrowObject()
    {
        Rigidbody objectRb = currentObject.GetComponent<Rigidbody>();

        objectRb.isKinematic = false;
        objectRb.useGravity = true;

        currentObject.transform.parent = null;
        currentObject.transform.localScale = Vector3.one;

        foreach (Collider collider in currentObject.transform.GetComponents<Collider>())
        {
            collider.enabled = true;
        }

        objectRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        hodlingObject = false;
        currentObject = null;
    }
}