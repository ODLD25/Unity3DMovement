using System;
using UnityEngine;
using UnityEngine.Events;

public class LevelManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject endLevelCanvas;

    public UnityEvent onLevelEnd;

    void Start()
    {
        endLevelCanvas.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            endLevelCanvas.SetActive(true);
            if (onLevelEnd != null) onLevelEnd.Invoke();
        }
    }
}
