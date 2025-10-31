using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private bool changeColor;
    [SerializeField] private int colorMaterialIndex;

    [SerializeField] private Color claimedColor;
    [SerializeField] private float colorEmmisionStrength;

    private void OnTriggerEnter(Collider other)
    {
        ResetPosScript.Instance.SetCheckpoint(transform);
        ChangeColor();
    }
    
    private void ChangeColor()
    {
        GetComponent<MeshRenderer>().materials[colorMaterialIndex].color = claimedColor;
        GetComponent<MeshRenderer>().materials[colorMaterialIndex].SetColor("_EmissionColor", claimedColor * colorEmmisionStrength);
    }
}
