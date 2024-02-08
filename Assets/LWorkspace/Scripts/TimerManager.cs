using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float remainingTime;

    public event OnOutOfTime OnOutOfTimeEvent;
    public delegate void OnOutOfTime();

    private bool isRunning = false;

    public void Init()
    {
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
            }
            else if (remainingTime < 0)
            {
                remainingTime = 0;
                timerText.color = Color.red;
                isRunning = false;
                OnOutOfTimeEvent?.Invoke();
            }
        }
        
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        minutes = Mathf.Clamp(minutes, 0, minutes); 
        seconds = Mathf.Clamp(seconds, 0, seconds); 

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void IncreaseTime(float time)
    {
        if (isRunning)
        {
            remainingTime += time;
            UpdateTimerText();
        }
    }
}
