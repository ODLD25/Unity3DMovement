using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    [SerializeField] private Vector3 force;
    [SerializeField] private float forceMultiplier;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            AddForceToItem(collision.gameObject);
        }
    }

    private void AddForceToItem(GameObject item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();

        rb.AddForce(force.normalized * forceMultiplier);
    }

    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + force * 2, 1.5f);
    }
}
