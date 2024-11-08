using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneManager : LevelManager
{
    [SerializeField] private TutorialManager tutorialManager;

    public override void InitLevel()
    {
        skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
        skully.OnCollectItemEvent += Skully_OnCollectItemEvent;
        TimerManager.OnOutOfTimeEvent += TimerManager_OnOutOfTimeEvent;
        tutorialManager.OnAllTutorialPassedEvent += TutorialManager_OnAllTutorialPassedEvent;

    //    tutorialManager.StartRunningTutorial();

        //skully.DisableControl();

     //   cameraFollowPlayer.StartCoroutine(cameraFollowPlayer.CameraInterpolate(() =>
     //   {
            
            TimerManager.Init();
        // }));
    }


    public override void InitSkullyWithData(GameSaveData data, Checkpoint cp)
    {
        skully.HealthAmount = data.health;
        if (data.sirsActivated == 0)
        {
            skully.DisableSIRS();
        }
        else
        {
            skully.ActiveSIRS();
        }

        skully.WeaponManager.SetupWeapon((WeaponItem.EWeaponType)data.weaponType);
        skully.DisableControl();
        if (cp == null)
        {
            skully.transform.position = startTrans.position;
        }
        else
        {
            skully.transform.position = cp.RespawnTrans.position;
            Debug.Log("respawn skully from checkpoint " + cp.ID + " : " + skully.transform.position);
        }

  

        DOVirtual.DelayedCall(0.2f, () =>
        {
            skully.EnableControl();
        });
    }

    private void TutorialManager_OnAllTutorialPassedEvent()
    {
        Debug.Log("all tutorial passed");
        skully.SetMaxForwardSpeedFactor(5f);
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
        EndGame();
    }
}
