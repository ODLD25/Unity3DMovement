using UnityEngine;

public class EndTimerScript : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TimerScript.instance.EndTimer();
        }
    }
}
