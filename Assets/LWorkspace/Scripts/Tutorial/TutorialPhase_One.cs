using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_One : TutorialPhaseBase
{
    [SerializeField] private float enablePlayerControllTimeStamp = 5f;

    private int collectedCoins = 0;

    public override void Prepare()
    {
        base.Prepare();
        LevelManager.Instance.Skully.OnCollectCoinEvent += Skully_OnCollectCoinEvent;
        LevelManager.Instance.TimerManager.StopAndHideTimer();
    }

    private void Skully_OnCollectCoinEvent(Coin coin)
    {
        collectedCoins++;
    }

    public override bool IsSuccess()
    {
        bool success = collectedCoins >= 5;
        Debug.Log(gameObject.name + " : " + success);
        return success;
    }
}
