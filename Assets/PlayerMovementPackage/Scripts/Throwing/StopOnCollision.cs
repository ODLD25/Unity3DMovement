using UnityEngine;
using UnityEngine.Events;

public class StopOnCollision : MonoBehaviour
{
    [SerializeField] private float minSpeedToStop;
    [SerializeField] private float minSpeedToMove;
    private Vector3 lastCollidedObjectPos;
    [SerializeField] private GameObject collidedObject;
    [SerializeField] private Rigidbody collidedRigidbody;
    [SerializeField] private UnityEvent onStopEvent;
    private bool stopped;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.frameCount % 3 == 0)
        {
            if (collidedRigidbody)
            {
                if (collidedRigidbody.linearVelocity.magnitude > minSpeedToMove)
                {
                    StartMoving();
                }
            }
            else if (collidedObject)
            {
                if (lastCollidedObjectPos != collidedObject.transform.position)
                {
                    StartMoving();
                }
                else
                {
                    lastCollidedObjectPos = collidedObject.transform.position;
                }
            }
            else if (!collidedObject)
            {
                StartMoving();
            }
            else if (!collidedObject.activeSelf)
            {
                StartMoving();
            }
        }
    }

    public void StartMoving()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        collidedObject = null;
        collidedRigidbody = null;

        stopped = false;
    }

    private void StopMoving(Collision collision)
    {
        rb.isKinematic = true;
        rb.useGravity = false;

        collidedObject = collision.gameObject;
        lastCollidedObjectPos = collidedObject.transform.position;
        collidedObject.TryGetComponent<Rigidbody>(out collidedRigidbody);

        onStopEvent.Invoke();

        stopped = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collidedObject) return;

        if (!stopped && !collision.gameObject.CompareTag("Throwable"))
        {
            if (collision.relativeVelocity.magnitude > minSpeedToStop)
            {
                StopMoving(collision);
            }
        }
        else
        {
            if (collision.relativeVelocity.magnitude > minSpeedToStop)
            {
                StartMoving();
            }
        }
    }
}
