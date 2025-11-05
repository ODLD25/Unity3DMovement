using TMPro;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI speedText;
    [SerializeField]private TextMeshProUGUI stateText;
    [SerializeField]private PlayerMovementScript pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speedText) speedText.text = pm.GetMovementSpeed() + "m/s";
        if (stateText) stateText.text = "State: " + pm.movementState;
    }
}
