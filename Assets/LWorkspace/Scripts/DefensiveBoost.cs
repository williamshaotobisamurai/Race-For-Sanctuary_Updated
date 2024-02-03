using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBoost : MonoBehaviour
{
    [SerializeField] private float defensiveDuration;
    [SerializeField] private AudioClip defensiveBoostAudio;
    public AudioClip AudioClip { get => defensiveBoostAudio; }

    public float GetDefensiveDuration()
    {
        return defensiveDuration;
    }
}
