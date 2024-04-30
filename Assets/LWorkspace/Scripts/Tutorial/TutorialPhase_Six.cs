using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Six : TutorialPhaseBase
{
    [SerializeField] private List<Turret> enemiesTurretList;

    public override void Prepare()
    {
        base.Prepare();
        GameManager.Instance.Skully.OnSkullyDiedEvent += Skully_OnSkullyDiedEvent;
    }

    private void Skully_OnSkullyDiedEvent()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            OnReachEndTrigger?.Invoke(false);
        });
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            GameManager.Instance.Skully.Revive();

        });
        seq.Play();
    }

    public override bool IsSuccess()
    {
        return enemiesTurretList.FindAll(t => t.IsKilled).Count >= 1;
    }  
}
