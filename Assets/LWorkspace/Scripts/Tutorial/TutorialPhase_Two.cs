using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPhase_Two : TutorialPhaseBase
{
    [SerializeField] private GameObject meteorite;

    public override void Prepare()
    {
        base.Prepare();
        LevelManager.Instance.Skully.DisableXYControl();
    }

    public override void StartPhase()
    {
        Skully skully = LevelManager.Instance.Skully;

        meteorite.transform.DOMove(skully.GetPosition() + Vector3.forward * 50f, 2f).OnComplete(() =>
        {
            skully.EnableXYControl();
        });
    }

    public override bool IsSuccess()
    {
        return true;
    }
}
