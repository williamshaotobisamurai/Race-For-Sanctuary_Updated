using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleManager : MonoBehaviour
{
    [SerializeField] private BossBattleTrigger bossBattleTrigger;
    [SerializeField] private Boss boss;
    [SerializeField] private Transform bossBattleStage;

    [SerializeField] private Skully skully;
    [SerializeField] private Animator bossBattleAnimator;

    public void StartBossBattle()
    {
        boss.gameObject.SetActive(true);
        bossBattleAnimator.Play("BossBattleIntro");
        bossBattleStage.position = new Vector3(0, 0, skully.transform.position.z);
        skully.SetMaxSpeedFactor(0);
    }
}
