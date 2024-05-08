using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Three : TutorialPhaseBase
{
    public override void Prepare()
    {
        base.Prepare();
        TimerManager timerManager = GameManager.Instance.TimerManager;
        timerManager.Init(20);
        timerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
        timerManager.ShowTimer();
    }

    public override void EndTrigger_OnSkullyEnterEvent()
    {
        base.EndTrigger_OnSkullyEnterEvent();
        TimerManager timerManager = GameManager.Instance.TimerManager;
        timerManager.OnOutOfTimeEvent -= TimerManager_OnOutOfTimeEvent;
    }

    public override bool IsSuccess()
    {
        return true;
    }

    private void TimerManager_OnOutOfTimeEvent()
    {
        OnReachEndTrigger?.Invoke(false);
    }

}
