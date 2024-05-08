using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameManager : GameManager
{
    [SerializeField] private TutorialManager tutorialManager;

    protected override void Start()
    {
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent += Skully_OnCollectItemEvent;
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        PoliceShip.OnCaughtSkullyEvent += PoliceShip_OnCaughtSkullyEvent;
        TimerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
        tutorialManager.OnAllTutorialPassedEvent += TutorialManager_OnAllTutorialPassedEvent;

        collectedCoinsManager.Init();
        tutorialManager.StartRunningTutorial();

        skully.DisableControl();

        cameraFollowPlayer.StartCoroutine(cameraFollowPlayer.CameraInterpolate(() =>
        {
            skully.EnableControl();
            TimerManager.Init();
        }));
    } 


    private void TutorialManager_OnAllTutorialPassedEvent()
    {
        Debug.Log("all tutorial passed");
        skully.SetMaxSpeedFactor(5f);
        endTrigger.gameObject.SetActive(true);
        endTrigger.transform.position = new Vector3(0, 0, skully.transform.position.z + 800f);
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
