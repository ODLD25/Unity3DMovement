using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    [SerializeField] private bool activateOnPlayerTouch = false;
    [SerializeField] private bool stopOnFirstWaypoint = false;
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float activateDelay = 0.25f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private List<Transform> waypoints;
    private bool playerOnPlatform;
    private int currentWaypint;
    private bool active;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWaypint = 0;

        if (activateOnPlayerTouch) active = false;
        else active = true;

        transform.position = waypoints[0].position;

        StartCoroutine(LoadWaypoints());
    }

    void FixedUpdate()
    {
        if (!active) return;

        rb.MovePosition(Vector3.MoveTowards(rb.position, waypoints[currentWaypint].position, moveSpeed * Time.fixedDeltaTime));
    }

    IEnumerator LoadWaypoints()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, waypoints[currentWaypint].position) < 0.05f)
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

    private void Activate()
    {
        active = true;
    }

    private void DeActivate()
    {
        active = false;
    }

    void OnDrawGizmos()
    {
        if (waypoints[0] == null) return;

        for (int i = 1; i < waypoints.Count; i++)
        {
            if (i == currentWaypint)
            {
                Handles.color = Color.green;
            }
            else
            {
                Handles.color = Color.white;
            }

            Handles.DrawLine(waypoints[i - 1].position, waypoints[i].position, 1.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoints[i].position, 0.25f);
        }

        if (0 == currentWaypint)
        {
            Handles.color = Color.green;
        }
        else
        {
            Handles.color = Color.white;
        }

        Handles.DrawLine(waypoints[0].position, waypoints[waypoints.Count - 1].position, 1.5f);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(waypoints[0].position, 0.25f);
    }
}
