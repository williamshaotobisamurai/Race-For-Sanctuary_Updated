using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattleManager : MonoBehaviour
{
    [SerializeField] private Boss boss;

    [SerializeField] private Skully skully;
    [SerializeField] private Transform bossStartTrans;
    [SerializeField] private Image bossWarning;
    [SerializeField] private TimerManager timerManager;

    public void StartBossBattle()
    {
        timerManager.StopAndHideTimer();
        boss.OnExplodeEvent += Boss_OnExplodeEvent;
        boss.gameObject.SetActive(true);
        skully.EnterBossMode();

        Sequence warningSeq = DOTween.Sequence();
        warningSeq.Append(bossWarning.DOFade(1, 0.3f));
        warningSeq.Append(bossWarning.DOFade(0, 0.3f));
        warningSeq.SetLoops(3);


        Sequence seq = DOTween.Sequence();
        seq.Append(warningSeq);
        seq.Append(boss.transform.DOMove(bossStartTrans.position, 4f));
        seq.AppendInterval(3f);
        seq.AppendCallback(() => boss.StartFighting());
        seq.Play();
    }

    private void Boss_OnExplodeEvent()
    {
        boss.OnExplodeEvent -= Boss_OnExplodeEvent;

        DOVirtual.DelayedCall(0.2f, () =>
        {
            skully.ExitBossMode();
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Time.timeScale = 0.25f;
            });

            DOVirtual.DelayedCall(4f, () =>
            {
                Time.timeScale = 1f;
            });
        });
    }
}
