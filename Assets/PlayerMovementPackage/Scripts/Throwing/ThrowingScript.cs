using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float throwForce = 1250;
    [SerializeField] private float throwUpForce = 100;
    [SerializeField] private bool useGravity = true;
    [SerializeField] private float cooldown = 0.25f;
    [SerializeField] private int maxThrows = 6;
    private int currentThrows;
    private bool canThrow;

    [Header("References")]
    [SerializeField] private GameObject throwPrefab;
    [SerializeField] private Transform throwPoint;

    private InputSystem_Actions inputActions;


    private void Start()
    {
        //Get Input
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.Attack.performed += Throw;

        canThrow = true;
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if (!canThrow || currentThrows >= maxThrows) return;

        currentThrows++;

        GameObject throwedObject = Instantiate(throwPrefab, throwPoint.position, Camera.main.transform.rotation);

        Vector3 forceDir;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 500))
        {
            forceDir = (hit.point - Camera.main.transform.position).normalized;
        }
        else
        {
            forceDir = Camera.main.transform.forward;
        }

        throwedObject.GetComponent<Rigidbody>().AddForce(forceDir * throwForce + Vector3.up * throwUpForce);
        throwedObject.GetComponent<Rigidbody>().useGravity = useGravity;

        canThrow = false;
        Invoke(nameof(ResetCanThrow), cooldown);
    }

    private void ResetCanThrow()
    {
        canThrow = true;
    }

    void OnDisable()
    {
        if (inputActions != null) inputActions.Player.Disable();
    }
}
