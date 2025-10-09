using UnityEngine;

public class DebugBreakableWallPart : MonoBehaviour
{
    [SerializeField] private bool debug;
    [SerializeField] private Color sleepingColor = Color.grey;
    [SerializeField] private Color defaultColor = Color.red;
    [SerializeField] private Color brokenColor = Color.blue;
    [SerializeField] private float force;
    private BreakableWallPartScript breakableWallPartScript;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        breakableWallPartScript = transform.GetComponent<BreakableWallPartScript>();
    }

    void Update()
    {
        if (breakableWallPartScript.IsBroken())
        {
            GetComponent<MeshRenderer>().material.color = brokenColor;
        }
        else if (rb.IsSleeping())
        {
            GetComponent<MeshRenderer>().material.color = sleepingColor;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = defaultColor;
        }
    }

    void OnMouseDown()
    {
        GetComponent<BreakableWallPartScript>().Break(true);
        rb.AddForce(Camera.main.transform.forward * force, ForceMode.Impulse);
    }
}
