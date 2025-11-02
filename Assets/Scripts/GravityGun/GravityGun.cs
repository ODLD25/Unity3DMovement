using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGun : MonoBehaviour
{
    [Header("PickUp")]
    [SerializeField] private float maxPickUpDistance;
    [SerializeField] private PickUpDetectionOption pickUpDetectionOption;

    [Header("Holding")]
    [SerializeField] private Transform objectHoldTransform;
    
    [Header("Scrolling")]
    [SerializeField, Tooltip("When true player can change the distance of the object when holding an object.")] private bool canAdjustHoldDistance;
    [SerializeField] private Vector2 minMaxHoldDistance = new Vector2(2, 5);
    [SerializeField] private float scrollSensitivity = 1;
    private float holdingDistance;

    [Header("Throwing")]
    [SerializeField, Tooltip("Force used when throwing object.")] private float throwForce = 15f;

    private bool hodlingObject;
    private GameObject currentObject;
    
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.GravityGun.Enable();
        inputActions.GravityGun.StartHoldingObject.performed += GravityGunInteract;
        inputActions.GravityGun.ChangeObjectHoldingDistance.performed += Scroll;
    }

    private void GravityGunInteract(InputAction.CallbackContext context)
    {
        if (hodlingObject) ThrowObject();
        else StartHoldingObject();
    }

    private void StartHoldingObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxPickUpDistance))
        {
            if (pickUpDetectionOption == PickUpDetectionOption.Both || pickUpDetectionOption == PickUpDetectionOption.RigidBody)
            {
                if (hit.transform.GetComponent<Rigidbody>() != null)
                {
                    currentObject = hit.transform.gameObject;

                    Rigidbody objectRb = hit.transform.GetComponent<Rigidbody>();

                    currentObject.transform.position = objectHoldTransform.position;
                    objectRb.isKinematic = true;
                    objectRb.useGravity = false;
                    currentObject.transform.parent = transform;

                    foreach (Collider collider in currentObject.transform.GetComponents<Collider>())
                    {
                        collider.enabled = false;
                    }

                    holdingDistance = 2f;
                    hodlingObject = true;
                }
            }
            else if (pickUpDetectionOption == PickUpDetectionOption.Both || pickUpDetectionOption == PickUpDetectionOption.Script)
            {

            }
        }
    }

    private void Scroll(InputAction.CallbackContext context)
    {
        holdingDistance += inputActions.GravityGun.ChangeObjectHoldingDistance.ReadValue<float>() * scrollSensitivity;
        holdingDistance = Mathf.Clamp(holdingDistance, minMaxHoldDistance.x, minMaxHoldDistance.y);

        currentObject.transform.localPosition = new Vector3(currentObject.transform.localPosition.x, currentObject.transform.localPosition.y, holdingDistance);
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

enum PickUpDetectionOption
{
    None,
    RigidBody,
    Script,
    Both
}
