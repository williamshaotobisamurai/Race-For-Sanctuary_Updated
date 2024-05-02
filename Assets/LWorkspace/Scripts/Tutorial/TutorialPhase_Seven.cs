using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Seven : TutorialPhaseBase
{
    public override void StartPhase()
    {
        base.StartPhase();
        GameManager.Instance.TimerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
    }

    private void TimerManager_OnOutOfTimeEvent()
    {
        OnReachEndTrigger?.Invoke(false);
    }

    public override void Prepare()
    {
        base.Prepare();
    }

    public override void EndTrigger_OnSkullyEnterEvent()
    {
        OnReachEndTrigger?.Invoke(true);
    }

    public override bool IsSuccess()
    {
        throw new System.NotImplementedException();
    }
}
