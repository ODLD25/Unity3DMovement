using UnityEngine;

public class SprintScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private PlayerMovementScript pm;
    private InputSystem_Actions inputActions;

    void Start() {
        //Get Player Movement
        if (pm == null) pm = GetComponent<PlayerMovementScript>();

        //Get Input Action Map and activates it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Sprint logic
        if (inputActions.Player.Sprint.ReadValue<float>() > 0f){
            pm.sprinting = true;
        }
        else{
            pm.sprinting = false;
        }
    }
}
