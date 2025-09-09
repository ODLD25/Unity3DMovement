using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Transform> waypoints;
    private int currentWaypint;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        currentWaypint = 0;
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, waypoints[currentWaypint].position, moveSpeed * Time.fixedDeltaTime));

        if (Vector3.Distance(transform.position, waypoints[currentWaypint].position) < 0.05f)
        {
            if (currentWaypint >= waypoints.Count - 1)
            {
                currentWaypint = 0;
            }
            else
            {
                currentWaypint++;
            }
        }
    }
}
