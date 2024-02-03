using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private float speedUpDuration;
    [SerializeField] private float speedBoostFactor;
    [SerializeField] private AudioClip speedBoostAudio;
    public AudioClip AudioClip { get => speedBoostAudio; }


    public float GetSpeedUpDuration()
    {
        return speedUpDuration;
    }

    public float GetSpeedBoostFactor()
    {
        return speedBoostFactor;
    }

}
