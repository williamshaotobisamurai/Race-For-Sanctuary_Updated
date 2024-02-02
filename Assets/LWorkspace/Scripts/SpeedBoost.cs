using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private float speedUpDuration;
    [SerializeField] private float speedBoostFactor;

    private void Start()
    {
        //transform.DORotate(new Vector3(0, 90, 0), 1f);
        //transform.DOJump(transform.position, 1, 1, 2f).SetLoops(-1);
        //transform.DOLocalMove(transform.position)
    }

    public float GetSpeedUpDuration()
    {
        return speedUpDuration;
    }

    public float GetSpeedBoostFactor()
    {
        return speedBoostFactor;
    }
}
