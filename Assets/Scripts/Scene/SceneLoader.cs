using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene loader")]
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private Transform btnParent;

    void Start()
    {
        LoadBtns();
    }

    private void LoadBtns()
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
                Debug.Log(sceneName);

                GameObject spawnedBtn = Instantiate(btnPrefab, btnParent);

                spawnedBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneName;
                spawnedBtn.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
            }
        }
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
