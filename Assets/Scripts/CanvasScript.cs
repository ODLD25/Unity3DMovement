using TMPro;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI speedText;
    [SerializeField]private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = rb.linearVelocity.magnitude + "m/s";
    }
}
