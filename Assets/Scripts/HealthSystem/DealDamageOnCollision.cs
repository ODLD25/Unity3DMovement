using UnityEngine;

public class DealDamageOnCollision : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        GetComponent<HealthSystem>().DealDamage(damage);
    }
}
