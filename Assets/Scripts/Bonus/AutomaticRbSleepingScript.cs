using UnityEngine;

public class AutomaticRbSleepingScript : MonoBehaviour
{
    [SerializeField] private float timeNeededToSleep = 5f;
    [SerializeField] private bool debug;
    private float timer;
    [SerializeField] private Material originalMat;
    [SerializeField] private Material redMat;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        timer = 0f;
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude == 0)
        {
            rb.Sleep();
        }

        if (debug)
        {
            if (rb.IsSleeping())
            {
                GetComponent<MeshRenderer>().material = redMat;
            }
            else
            {
                GetComponent<MeshRenderer>().material = originalMat;
            }
        }

        if (rb.linearVelocity.magnitude < 0.01f)
        {
            timer += Time.deltaTime;

            if (timer >= timeNeededToSleep)
            {
                rb.Sleep();
            }
        }
        else
        {
            timer = 0f;
        }
    }

}
