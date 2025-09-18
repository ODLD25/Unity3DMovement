using UnityEngine;

public class ExplodingScript : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionUpVelocity;

    [Header("Settings")]
    [SerializeField] private bool explodeOnImpact;

    [Header("Reference")]
    [SerializeField] private GameObject brokenObject;

    private bool exploded;

    void Start()
    {
        exploded = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!exploded && explodeOnImpact) Explode();
    }

    public void Explode()
    {
        exploded = true;
        if (brokenObject)
        {
            Instantiate(brokenObject, transform.position, transform.rotation);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<Rigidbody>())
            {
                collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpVelocity);
            }
        }

        if (brokenObject)
        {
            Destroy(gameObject);
        }
    }
}
