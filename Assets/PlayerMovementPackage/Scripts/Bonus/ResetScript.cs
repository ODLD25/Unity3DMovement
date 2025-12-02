using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject player;
    [SerializeField] private ResetType resetType = ResetType.LoadCheckpoint;

    [Header("Checkpoints")]
    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private Vector3 defaultPos = new Vector3(0, 2, 0);
    [SerializeField] private Vector3 resetOffset = new Vector3(0, 1, 0);

    [Header("References")]
    private InputSystem_Actions inputActions;

#region Main
    void Start()
    {
        //Get Input Action Map and activate it
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.ResetPos.performed += ResetPlayer;

        if (player == null) player = gameObject;
    }

    public void ResetPlayer()
    {
        if (resetType == ResetType.ResetScene)
        {
            ResetScene();
        }
        else if (resetType == ResetType.LoadCheckpoint)
        {
            LoadLastCheckpoint();
        }
    }
    
    public void ResetPlayer(InputAction.CallbackContext context)
    {
        if (resetType == ResetType.ResetScene)
        {
            ResetScene();
        }
        else if (resetType == ResetType.LoadCheckpoint)
        {
            LoadLastCheckpoint();
        }
    }
#endregion

#region ResetScene
    private void ResetScene()
    {
        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
#endregion

#region Checkpoint
    private void LoadLastCheckpoint()
    {
        player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        if (!currentCheckpoint)
        {
            player.transform.position = defaultPos;
        }
        else
        {
            player.transform.position = currentCheckpoint.position + resetOffset;
        }
    }
    
    public void SetCheckpoint(Transform newCheckpoint){
        currentCheckpoint = newCheckpoint;
    }
#endregion
}

enum ResetType
{
    None,
    LoadCheckpoint,
    ResetScene
}
