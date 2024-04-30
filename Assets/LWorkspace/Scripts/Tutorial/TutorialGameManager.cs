using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameManager : GameManager
{
    protected override void Start()
    {
        collectedCoinsManager.Init();
   
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent += Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        PoliceShip.OnCaughtSkullyEvent += PoliceShip_OnCaughtSkullyEvent;
        TimerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
    }

    protected override void PoliceShip_OnCaughtSkullyEvent()
    {
        //base.PoliceShip_OnCaughtSkullyEvent();
    }

    protected override void TimerManager_OnOutOfTimeEvent()
    {
 
    }

    protected override void Skully_OnSkullyDiedEvent()
    {
        
    }
}
