using System.Collections.Generic;
using UnityEngine;

public class BreakableWallPartScript : MonoBehaviour
{
    public float strength = 1;
    public float weight = 1;
    private Rigidbody rb;
    private bool broken;
    private int frameIndex;
    private float height;

    public Collider[] colliders;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, height * 0.8f);
    }

    void Start()
    {
        frameIndex = Random.Range(5, 10);
        height = GetComponent<Renderer>().bounds.size.y / 2 + 0.4f;
        Debug.Log("Vector3.down " + Vector3.down);

        weight = (GetComponent<Renderer>().bounds.size.x + GetComponent<Renderer>().bounds.size.y + GetComponent<Renderer>().bounds.size.z) / 2;
        strength = (GetComponent<Renderer>().bounds.size.x + GetComponent<Renderer>().bounds.size.y + GetComponent<Renderer>().bounds.size.z) * 2;
    }

    void Update()
    {
        if (!broken && Time.frameCount % frameIndex == 0)
        {
            /*if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, height))
            {
                Debug.DrawRay(transform.position, hit.point, Color.red, 0.5f);
                sides[0] = true;
            }
            else
            {
                sides[0] = false;
            }

            if (!Physics.Raycast(transform.position, -Vector3.down, out hit,  height))
            {
                Debug.DrawRay(transform.position, hit.point, Color.blue, 0.5f);
                sides[1] = true;
            }
            else
            {
                sides[1] = false;
            }

            if (!Physics.Raycast(transform.position, Vector3.right, out hit,  height))
            {
                Debug.DrawRay(transform.position, hit.point, Color.green, 0.5f);
                sides[2] = true;
            }
            else
            {
                sides[2] = false;
            }

            if (!Physics.Raycast(transform.position, -Vector3.right, out hit,  height))
            {
                Debug.DrawRay(transform.position, hit.point, Color.cyan, 0.5f);
                sides[3] = true;
            }
            else
            {
                sides[3] = false;
            }

            int sidesActive = 0;
            for (int i = 0; i < sides.Length; i++)
            {
                if (sides[i])
                {
                    Debug.Log("Side active");
                    sidesActive++;
                }
            }

            if (sidesActive < 2)
            {
                Debug.Log("Gravity " + gameObject.name);
                Break(true);
            }*/

            colliders = Physics.OverlapSphere(transform.position, height * 0.8f);
            if (colliders.Length < 2)
            {
                Break(true);
            }
        }
        
        /*Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - height, transform.position.z), Color.red, 0.5f);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + height, transform.position.z), Color.blue, 0.5f);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x - height, transform.position.y, transform.position.z), Color.green, 0.5f);
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + height, transform.position.y, transform.position.z), Color.cyan, 0.5f);*/
    }

    public void Break(bool remove)
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        broken = true;

        if (remove)
        {
            transform.parent.parent.GetComponent<BreakableWalllScript>().RemovePart(this);
        }
    }

    public bool IsBroken()
    {
        return broken;
    }
}
