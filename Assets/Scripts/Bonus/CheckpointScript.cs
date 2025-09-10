using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        ResetPosScript.Instance.SetCheckpoint(this.transform);
    }
}
