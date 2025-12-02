using TMPro;
using UnityEngine;

public class BreakableWallInfoScript : MonoBehaviour
{
    [SerializeField] private BreakableWalllScript breakableWalllScript;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI layerText;
    [SerializeField] private int layer;

    void Update()
    {
        layerText.text = layer.ToString();
        strengthText.text =  breakableWalllScript.GetLayerStrength(layer).ToString();
        weightText.text = breakableWalllScript.GetWeightOnLayer(layer).ToString();
    }
}
