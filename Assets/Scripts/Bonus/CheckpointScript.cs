using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private bool changeColor;
    [SerializeField] private int colorMaterialIndex;

    [SerializeField] private Color claimedColor;
    [SerializeField] private Color unclaimedColor;
    [SerializeField] private float colorEmmisionStrength;

    [SerializeField] private bool disableOtherCheckpoint = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResetScript resetScript))
        {
            resetScript.SetCheckpoint(transform);
        }
        
        EnableCheckPoint();
    }

    public void EnableCheckPoint()
    {
        GetComponent<MeshRenderer>().materials[colorMaterialIndex].color = claimedColor;
        GetComponent<MeshRenderer>().materials[colorMaterialIndex].SetColor("_EmissionColor", claimedColor * colorEmmisionStrength);

        if (disableOtherCheckpoint)
        {
            foreach (CheckpointScript checkpointScript in FindObjectsByType<CheckpointScript>(FindObjectsSortMode.None))
            {
                if (checkpointScript != this)
                {
                    checkpointScript.DisableCheckPoint();
                }
            }
        }
    }

    public void DisableCheckPoint()
    {
        GetComponent<MeshRenderer>().materials[colorMaterialIndex].color = unclaimedColor;
        GetComponent<MeshRenderer>().materials[colorMaterialIndex].SetColor("_EmissionColor", unclaimedColor * colorEmmisionStrength);
    }
}
