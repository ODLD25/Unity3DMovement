using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ExplodingScript : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionUpVelocity;
    [SerializeField] private float explosionCooldown = 0.1f;
    [SerializeField] private VisualEffect explosionVFX;

    [Header("Settings")]
    [SerializeField] private bool explodeOnImpact;
    [SerializeField] private bool explodeMultipleTimesOnCollision = false;
    [SerializeField] private float minForceToKnockOffThings;

    [Header("Reference")]
    [SerializeField] private GameObject brokenObject;

    private bool canExplode;

    void Start()
    {
        Invoke(nameof(resetCanExplode), explosionCooldown);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact && canExplode) Explode();
    }

    private void resetCanExplode()
    {
        canExplode = true;
    }

    public void Explode()
    {

        if (explodeMultipleTimesOnCollision)
        {
            canExplode = false;
            Invoke(nameof(resetCanExplode), explosionCooldown);
        }
        else
        {
            canExplode = false;
        }

        if (brokenObject)
        {
            Instantiate(brokenObject, transform.position, transform.rotation);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider != GetComponent<Collider>() && collider.GetComponent<StopOnCollision>())
            {
                if (explosionForce * (1 - (Vector3.Distance(transform.position, collider.transform.position) / explosionRadius)) + explosionUpVelocity > minForceToKnockOffThings)
                {
                    collider.GetComponent<StopOnCollision>().StartMoving();
                }
            }

            if (collider.GetComponent<Rigidbody>())
            {
                collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpVelocity);
            }

            if (collider.GetComponent<BreakableWallPartScript>())
            {
                collider.GetComponent<BreakableWallPartScript>().Break(true);
            }
        }

        if (explosionVFX)
        {
            explosionVFX.Stop();
            explosionVFX.transform.SetParent(null, true);
            Destroy(explosionVFX.gameObject, explosionVFX.GetFloat("LifeTime"));
            explosionVFX.Play();
        }

        if (brokenObject)
        {
            Destroy(gameObject);
        }
    }
}
