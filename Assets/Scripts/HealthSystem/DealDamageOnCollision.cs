using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    [SerializeField] private float minSpeedToDealDamage = 10f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minSpeedToDealDamage)
        {
            GetComponent<HealthSystem>().DealDamage(damage);
        }
    }
}
