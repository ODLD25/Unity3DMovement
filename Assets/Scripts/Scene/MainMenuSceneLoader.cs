using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSceneLoader : MonoBehaviour
{
    [Header("Scene loader")]
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private Transform btnParent;
    [SerializeField] private List<string> sceneNames;

    void Start()
    {
        LoadBtns();
    }

    private void LoadBtns()
    {
        /*foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
                Debug.Log(sceneName);

                GameObject spawnedBtn = Instantiate(btnPrefab, btnParent);

                spawnedBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneName;
                spawnedBtn.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
            }
        }*/

        /*for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            
            GameObject spawnedBtn = Instantiate(btnPrefab, btnParent);

            spawnedBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneName;
            spawnedBtn.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
        }*/

        for (int i = 0; i < sceneNames.Count; i++)
        {
            string sceneName = sceneNames[i];
            
            GameObject spawnedBtn = Instantiate(btnPrefab, btnParent);

            spawnedBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = sceneName;
            spawnedBtn.GetComponent<Button>().onClick.AddListener(() => LoadScene(sceneName));
        }
    }

    private void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
