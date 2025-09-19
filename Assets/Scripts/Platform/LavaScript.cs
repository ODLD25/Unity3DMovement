using UnityEngine;

public class LavaScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ResetSceneScript.ResetScene();
        }
    }
}
