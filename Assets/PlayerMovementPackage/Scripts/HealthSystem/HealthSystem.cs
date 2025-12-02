using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health;

    [SerializeField] private UnityEvent onDeathEvent;

    public void DealDamage(int damage)
    {
        health -= damage;

        if (health <= 0) onDeathEvent.Invoke();
    }

    public void HealHealth(int healedValue)
    {
        health += healedValue;
    }
}
