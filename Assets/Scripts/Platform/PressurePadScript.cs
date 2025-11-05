using UnityEngine;
using UnityEngine.Events;

public class PressurePadScript : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private UnityEvent onTriggerEvent;
    private int collisionCount;
    private bool pressed;

    [Header("Colors")]
    [SerializeField] private Color unpressedColor = Color.red;
    [SerializeField] private Color pressedColor = Color.green;

    void Awake()
    {
        GetComponent<MeshRenderer>().material.color = unpressedColor;
        collisionCount = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        collisionCount++;
        UpdateButton();
    }

    void OnCollisionExit(Collision collision)
    {
        collisionCount--;
        UpdateButton();
    }

    private void UpdateButton()
    {
        if (collisionCount == 0)
        {
            pressed = false;
            GetComponent<MeshRenderer>().material.color = unpressedColor;
        }
        else if (!pressed && collisionCount > 0)
        {
            pressed = true;
            GetComponent<MeshRenderer>().material.color = pressedColor;

            onTriggerEvent.Invoke();
        }
    }
}
