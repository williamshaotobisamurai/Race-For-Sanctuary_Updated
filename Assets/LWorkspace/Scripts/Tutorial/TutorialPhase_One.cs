using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_One : TutorialPhaseBase
{
    [SerializeField] private float enablePlayerControllTimeStamp = 5f;

    private int collectedCoins = 0;

    public override void Prepare()
    {
        endTrigger.OnSkullyEnterEvent += EndTrigger_OnSkullyEnterEvent;
        GameManager.Instance.Skully.OnCollectCoinEvent += Skully_OnCollectCoinEvent;
    }

    private void Skully_OnCollectCoinEvent(Coin coin)
    {
        collectedCoins++;
    }

    public override void StartPhase()
    {
        //   GameObject coinsInstance = Instantiate(coinsRoot);
        //   collectedCoins = 0;
        //    coinsInstance.transform.position = new Vector3(0, 0, GameManager.Instance.Skully.transform.position.z);
        //coinsRoot.SetActive(true);
    }

    public override bool IsSuccess()
    {
        bool success = collectedCoins >= 5;
        Debug.Log(gameObject.name + " : " + success);
        return success;
    }
}
