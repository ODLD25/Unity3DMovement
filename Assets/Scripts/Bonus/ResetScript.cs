using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    [SerializeField]private ResetType resetType = ResetType.LoadCheckpoint;

    [Header("References")]
    private InputSystem_Actions inputActions;

    void Start()
    {
        //Get Input Action Map and activate it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.ResetPos.performed += ResetScene;
    }

    public void Reset()
    {
        if (resetType == ResetType.ResetScene)
        {
            ResetScene();
        }
        else if (resetType == ResetType.LoadCheckpoint)
        {

        }
    }
    
    private void LoadLastCheckpoint()
    {
        
    }

    private void ResetScene()
    {
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void ResetScene(InputAction.CallbackContext context)
    {
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

enum ResetType
{
    None,
    LoadCheckpoint,
    ResetScene
}
