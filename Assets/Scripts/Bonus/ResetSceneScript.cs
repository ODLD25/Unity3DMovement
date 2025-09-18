using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetSceneScript : MonoBehaviour
{
    [Header("References")]
    private InputSystem_Actions inputActions;

    void Start()
    {
        //Get Input Action Map and activate it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.ResetPos.performed += ResetScene;
    }
    
    private void ResetScene(InputAction.CallbackContext context)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
