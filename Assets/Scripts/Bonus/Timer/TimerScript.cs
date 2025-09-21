using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private TextMeshProUGUI text;
    private bool timerEnded;

    void Start()
    {
        timer = 0f;
        timerEnded = false;
    }

    void Update()
    {
        if (!timerEnded)
        {
            timer += Time.deltaTime;
        }

        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;
        int miliSeconds = (int)(timer * 1000 % 1000);

        text.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, miliSeconds);
    }

    public void EndTimer()
    {
        timerEnded = true;
        Time.timeScale = 0f;
    }
}
