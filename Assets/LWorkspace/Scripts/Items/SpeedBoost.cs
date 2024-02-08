using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : ItemBase
{
    [SerializeField] private float speedUpDuration;
    [SerializeField] private float speedBoostFactor;

    public float GetSpeedUpDuration()
    {
        return speedUpDuration;
    }

    public float GetSpeedBoostFactor()
    {
        return speedBoostFactor;
    }
}
