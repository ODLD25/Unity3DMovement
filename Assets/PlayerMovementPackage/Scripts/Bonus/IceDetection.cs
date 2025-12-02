using UnityEngine;

public class IceDetection : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]private IceDetectionMethod iceDetectionMethod = IceDetectionMethod.Tag;
    [SerializeField] private string iceTag = "Ice";
    [SerializeField] private int iceDetectionInterval = 2;

    [Header("References")]
    [SerializeField] private PlayerMovementScript pm;

    private void Start()
    {
        if (!pm) pm = GetComponent<PlayerMovementScript>();
    }

    private void Update()
    {
        if (Time.frameCount % iceDetectionInterval == 0)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, pm.playerHeight / 2 + 0.3f))
            {
                if (iceDetectionMethod == IceDetectionMethod.Tag)
                {
                    if (hit.collider.gameObject.CompareTag(iceTag))
                    {
                        pm.onIce = true;
                    }
                }
                else if (iceDetectionMethod == IceDetectionMethod.Component)
                {
                    if (hit.collider.gameObject.GetComponent<IceScript>())
                    {
                        pm.onIce = true;
                    }
                }
            }
        }
    }
}

enum IceDetectionMethod
{
    None,
    Tag,
    Component
}

