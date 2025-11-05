using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private bool activateOnPlayerTouch;
    [SerializeField] private bool activateOnStart;
    [SerializeField] private bool stopOnFirstWaypoint;
    [SerializeField] private float waitTime;
    [SerializeField] private float activateDelay = 0.25f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Vector3> waypoints;
    private bool playerOnPlatform;
    private int currentWaypint;
    private bool active;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        currentWaypint = 0;

        if (activateOnPlayerTouch) active = false;
        else if (activateOnStart) active = true;
        else active = false;

        transform.position = waypoints[0];

        StartCoroutine(LoadWaypoints());
    }

    void FixedUpdate()
    {
        if (!active) return;

        rb.MovePosition(Vector3.MoveTowards(rb.position, transform.InverseTransformPoint(waypoints[currentWaypint]), moveSpeed * Time.fixedDeltaTime));
    }

    IEnumerator LoadWaypoints()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, waypoints[currentWaypint]) < 0.05f)
            {
                if (!playerOnPlatform && activateOnPlayerTouch) DeActivate();

                yield return new WaitForSeconds(waitTime);

                if (currentWaypint == 0 && stopOnFirstWaypoint && !playerOnPlatform)
                {
                    currentWaypint = 0;
                }
                else if (currentWaypint >= waypoints.Count - 1)
                {
                    currentWaypint = 0;
                }
                else
                {
                    currentWaypint++;
                }
            }

            yield return null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementScript>())
        {
            playerOnPlatform = true;
            Invoke(nameof(Activate), activateDelay);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementScript>())
        {
            playerOnPlatform = false;
        }
    }

    public void Activate()
    {
        active = true;
    }

    public void DeActivate()
    {
        active = false;
    }

    void OnDrawGizmos()
    {
        Vector3 lastPos = transform.InverseTransformPoint(transform.position);
        
        foreach (Vector3 waypointPos in waypoints)
        {
            Gizmos.DrawLine(transform.InverseTransformPoint(lastPos), transform.InverseTransformPoint(waypointPos));
            lastPos = waypointPos;
        }
    }
}
