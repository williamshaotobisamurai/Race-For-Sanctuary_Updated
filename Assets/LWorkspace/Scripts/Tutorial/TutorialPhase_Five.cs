using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Five : TutorialPhaseBase
{
    [SerializeField] private GameObject meteorite;

    [SerializeField] private DefensiveBoost defensiveBoost;

    public override void Prepare()
    {
       base.Prepare();
    }

    public override bool IsSuccess()
    {
        return defensiveBoost.IsCollected;
    }

    public override void EndTrigger_OnSkullyEnterEvent()
    {
        base.EndTrigger_OnSkullyEnterEvent();
    }
}
