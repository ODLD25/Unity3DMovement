using UnityEngine;

public class IceDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]private string iceTag;

    [Header("References")]
    [SerializeField]private PlayerMovementScript pm;

    private void Start() {
        if (!pm) pm = GetComponent<PlayerMovementScript>();
    }

    private void Update() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, pm.playerHeight / 2 + 0.3f)){
            if (hit.collider.gameObject.CompareTag(iceTag)){
                pm.onIce = true;
            }
        }
    }
}
