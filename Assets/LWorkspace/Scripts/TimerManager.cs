using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float remainingTime;

    public event OnOutOfTime OnOutOfTimeEvent;
    public delegate void OnOutOfTime();

    [SerializeField] private CanvasGroup fuelIconRoot;
    [SerializeField] private Image progressImg;

    private bool isRunning = false;

    private float maxTime = 90;

    [SerializeField] private string warningText;

    private bool messageSent = false;

    public void Init(float startTime = 90f)
    {
        timerText.color = Color.white;

        UpdateTimerUI();
        remainingTime = startTime;
        isRunning = true;
    }

    public void StopAndHideTimer()
    {
        Debug.Log("stop and hide timer");
        isRunning = false;
        timerText.text = "";
        fuelIconRoot.DOFade(0, 0.5f);
    }

    public void ShowTimer()
    {
        timerText.DOFade(1f, 0.5f);
        fuelIconRoot.DOFade(1, 0.5f);
    }

    public void HideTimer()
    {
        fuelIconRoot.DOFade(1, 0.5f);
        timerText.DOFade(0f, 0.5f);
    }

    void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;

                if (remainingTime <= 20 && !messageSent)
                {
                    messageSent = true;
                    InstructionManager.ShowText(warningText);
                }
            }
            else if (remainingTime < 0)
            {
                remainingTime = 0;
                timerText.color = Color.red;
                isRunning = false;
                OnOutOfTimeEvent?.Invoke();
            }

            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        minutes = Mathf.Clamp(minutes, 0, minutes);
        seconds = Mathf.Clamp(seconds, 0, seconds);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        progressImg.fillAmount = remainingTime / maxTime;
    }

    public void IncreaseTime(float time)
    {
        if (isRunning)
        {
            remainingTime += time;
            remainingTime = Mathf.Clamp(remainingTime, 0, maxTime);
            UpdateTimerUI();
            RectTransform textRect = timerText.GetComponent<RectTransform>();
            textRect.DOLocalJump(textRect.localPosition, 30f, 1, 0.25f);
        }
    }
}
