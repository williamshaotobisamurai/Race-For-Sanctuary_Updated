using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Four : TutorialPhaseBase
{
    public override void Prepare()
    {
        TimerManager timerManager = GameManager.Instance.TimerManager;
        timerManager.Init(20);
        timerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
    }

    public override bool IsSuccess()
    {
        return true;
    }

    public override void EndTrigger_OnSkullyEnterEvent()
    {
        OnReachEndTrigger?.Invoke(true);
    }

    private void TimerManager_OnOutOfTimeEvent()
    {
        OnReachEndTrigger?.Invoke(false);
    }
}
