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
    }

    public override bool IsSuccess()
    {
        return enemiesTurretList.FindAll(t => t.IsKilled).Count >= 1;
    }  
}
