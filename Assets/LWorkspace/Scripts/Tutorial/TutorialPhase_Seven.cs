using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Seven : TutorialPhaseBase
{
    public override void StartPhase()
    {
        base.StartPhase();
        GameManager.Instance.TimerManager.ShowTimer();
    }

    public override void Prepare()
    {
        base.Prepare();
    }

    public override bool IsSuccess()
    {
        throw new System.NotImplementedException();
    }
}
