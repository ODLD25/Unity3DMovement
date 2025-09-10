using UnityEngine;
using UnityEngine.InputSystem;

public class ResetPosScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private Vector3 defaultPos = new Vector3(0, 2, 0);

    [Header("References")]
    private InputSystem_Actions inputActions;

    public void Start()
    {
        //Get Input Action Map and activate it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.ResetPos.performed += ResetPos;
    }

    private void ResetPos(InputAction.CallbackContext context)
    {
        if (!currentCheckpoint)
        {
            player.transform.position = defaultPos;
        }
        else
        {
            player.transform.position = currentCheckpoint.position;
        }
    }
}
